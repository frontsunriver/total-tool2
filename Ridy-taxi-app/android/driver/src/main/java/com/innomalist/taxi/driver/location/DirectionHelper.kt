package com.innomalist.taxi.driver.location

import android.content.Context
import android.os.AsyncTask
import android.os.Handler
import android.util.DisplayMetrics
import android.util.Log
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.model.*
import com.innomalist.taxi.driver.location.DirectionHelper
import org.w3c.dom.Document
import org.w3c.dom.NodeList
import org.xml.sax.SAXException
import java.io.BufferedReader
import java.io.IOException
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import java.util.*
import javax.xml.parsers.DocumentBuilderFactory
import javax.xml.parsers.ParserConfigurationException

class DirectionHelper(context: Context?) {
    private var mDirectionListener: OnDirectionResponseListener? = null
    private var mAnimateListener: OnAnimateListener? = null
    private var isLogging = false
    private var animateMarkerPosition: LatLng? = null
    private var beginPosition: LatLng? = null
    private var endPosition: LatLng? = null
    private var animatePositionList: ArrayList<LatLng>? = null
    var animateMarker: Marker? = null
        private set
    var animatePolyline: Polyline? = null
        private set
    private var gm: GoogleMap? = null
    private var step = -1
    private var animateSpeed = -1
    private var zoom = -1
    private var animateDistance = -1.0
    private var animateCamera = -1.0
    private var totalAnimateDistance = 0.0
    private var cameraLock = false
    private var drawMarker = false
    private var drawLine = false
    private var flatMarker = false
    private var isCameraTilt = false
    private var isCameraZoom = false
    var isAnimated = false
        private set
    private var mContext: Context? = null
    fun request(start: LatLng, end: LatLng, mode: String): String {
        val url = ("http://maps.googleapis.com/maps/api/directions/xml?"
                + "origin=" + start.latitude + "," + start.longitude
                + "&destination=" + end.latitude + "," + end.longitude
                + "&sensor=false&units=metric&mode=" + mode)
        if (isLogging) Log.i("DirectionHelper", "URL : $url")
        RequestTask().execute(*arrayOf(url))
        return url
    }

    private inner class RequestTask : AsyncTask<String, Void, Document>() {
        override fun doInBackground(vararg uri: String): Document? {
            try {
                val url = URL(uri[0])
                val client = url.openConnection() as HttpURLConnection
                client.requestMethod = "POST"
                client.doInput = true
                val reader = BufferedReader(InputStreamReader(client.inputStream))
                val sb = StringBuilder()
                var line: String?
                while (reader.readLine().also { line = it } != null) sb.append(line)
                val builder = DocumentBuilderFactory.newInstance().newDocumentBuilder()
                return builder.parse(sb.toString())
                /*HttpClient httpClient = new DefaultHttpClient();
                HttpContext localContext = new BasicHttpContext();
                HttpPost httpPost = new HttpPost(url[0]);
                HttpResponse response = httpClient.execute(httpPost, localContext);
                InputStream in = response.getEntity().getContent();
                DocumentBuilder builder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
                return builder.parse(in);*/
            } catch (e: IOException) {
                e.printStackTrace()
            } catch (e: ParserConfigurationException) {
                e.printStackTrace()
            } catch (e: SAXException) {
                e.printStackTrace()
            }
            return null
        }

        override fun onPostExecute(doc: Document?) {
            super.onPostExecute(doc)
            if (mDirectionListener != null) mDirectionListener!!.onResponse(getStatus(doc), doc, this@DirectionHelper)
        }

        private fun getStatus(doc: Document?): String {
            val nl1 = doc!!.getElementsByTagName("status")
            val node1 = nl1.item(0)
            if (isLogging) Log.i("DirectionHelper", "Status : " + node1.textContent)
            return node1.textContent
        }
    }

    fun setLogging(state: Boolean) {
        isLogging = state
    }

    fun getStatus(doc: Document): String {
        val nl1 = doc.getElementsByTagName("status")
        val node1 = nl1.item(0)
        if (isLogging) Log.i("DirectionHelper", "Status : " + node1.textContent)
        return node1.textContent
    }

    fun getDurationText(doc: Document): Array<String?> {
        val nl1 = doc.getElementsByTagName("duration")
        val arr_str = arrayOfNulls<String>(nl1.length - 1)
        for (i in 0 until nl1.length - 1) {
            val node1 = nl1.item(i)
            val nl2 = node1.childNodes
            val node2 = nl2.item(getNodeIndex(nl2, "text"))
            arr_str[i] = node2.textContent
            if (isLogging) Log.i("DirectionHelper", "DurationText : " + node2.textContent)
        }
        return arr_str
    }

    fun getDurationValue(doc: Document): IntArray {
        val nl1 = doc.getElementsByTagName("duration")
        val arr_int = IntArray(nl1.length - 1)
        for (i in 0 until nl1.length - 1) {
            val node1 = nl1.item(i)
            val nl2 = node1.childNodes
            val node2 = nl2.item(getNodeIndex(nl2, "value"))
            arr_int[i] = node2.textContent.toInt()
            if (isLogging) Log.i("DirectionHelper", "Duration : " + node2.textContent)
        }
        return arr_int
    }

    fun getTotalDurationText(doc: Document): String {
        val nl1 = doc.getElementsByTagName("duration")
        val node1 = nl1.item(nl1.length - 1)
        val nl2 = node1.childNodes
        val node2 = nl2.item(getNodeIndex(nl2, "text"))
        if (isLogging) Log.i("DirectionHelper", "TotalDuration : " + node2.textContent)
        return node2.textContent
    }

    fun getTotalDurationValue(doc: Document): Int {
        val nl1 = doc.getElementsByTagName("duration")
        val node1 = nl1.item(nl1.length - 1)
        val nl2 = node1.childNodes
        val node2 = nl2.item(getNodeIndex(nl2, "value"))
        if (isLogging) Log.i("DirectionHelper", "TotalDuration : " + node2.textContent)
        return node2.textContent.toInt()
    }

    fun getDistanceText(doc: Document): Array<String?> {
        val nl1 = doc.getElementsByTagName("distance")
        val arr_str = arrayOfNulls<String>(nl1.length - 1)
        for (i in 0 until nl1.length - 1) {
            val node1 = nl1.item(i)
            val nl2 = node1.childNodes
            val node2 = nl2.item(getNodeIndex(nl2, "text"))
            arr_str[i] = node2.textContent
            if (isLogging) Log.i("DirectionHelper", "DurationText : " + node2.textContent)
        }
        return arr_str
    }

    fun getDistanceValue(doc: Document): IntArray {
        val nl1 = doc.getElementsByTagName("distance")
        val arr_int = IntArray(nl1.length - 1)
        for (i in 0 until nl1.length - 1) {
            val node1 = nl1.item(i)
            val nl2 = node1.childNodes
            val node2 = nl2.item(getNodeIndex(nl2, "value"))
            arr_int[i] = node2.textContent.toInt()
            if (isLogging) Log.i("DirectionHelper", "Duration : " + node2.textContent)
        }
        return arr_int
    }

    fun getTotalDistanceText(doc: Document): String {
        val nl1 = doc.getElementsByTagName("distance")
        val node1 = nl1.item(nl1.length - 1)
        val nl2 = node1.childNodes
        val node2 = nl2.item(getNodeIndex(nl2, "text"))
        if (isLogging) Log.i("DirectionHelper", "TotalDuration : " + node2.textContent)
        return node2.textContent
    }

    fun getTotalDistanceValue(doc: Document): Int {
        val nl1 = doc.getElementsByTagName("distance")
        val node1 = nl1.item(nl1.length - 1)
        val nl2 = node1.childNodes
        val node2 = nl2.item(getNodeIndex(nl2, "value"))
        if (isLogging) Log.i("DirectionHelper", "TotalDuration : " + node2.textContent)
        return node2.textContent.toInt()
    }

    fun getStartAddress(doc: Document): String {
        val nl1 = doc.getElementsByTagName("start_address")
        val node1 = nl1.item(0)
        if (isLogging) Log.i("DirectionHelper", "StartAddress : " + node1.textContent)
        return node1.textContent
    }

    fun getEndAddress(doc: Document): String {
        val nl1 = doc.getElementsByTagName("end_address")
        val node1 = nl1.item(0)
        if (isLogging) Log.i("DirectionHelper", "StartAddress : " + node1.textContent)
        return node1.textContent
    }

    fun getCopyRights(doc: Document): String {
        val nl1 = doc.getElementsByTagName("copyrights")
        val node1 = nl1.item(0)
        if (isLogging) Log.i("DirectionHelper", "CopyRights : " + node1.textContent)
        return node1.textContent
    }

    fun getDirection(doc: Document): ArrayList<LatLng> {
        val nl1: NodeList
        var nl2: NodeList
        var nl3: NodeList
        val listGeopoints = ArrayList<LatLng>()
        nl1 = doc.getElementsByTagName("step")
        if (nl1.length > 0) {
            for (i in 0 until nl1.length) {
                val node1 = nl1.item(i)
                nl2 = node1.childNodes
                var locationNode = nl2.item(getNodeIndex(nl2, "start_location"))
                nl3 = locationNode.childNodes
                var latNode = nl3.item(getNodeIndex(nl3, "lat"))
                var lat = latNode.textContent.toDouble()
                var lngNode = nl3.item(getNodeIndex(nl3, "lng"))
                var lng = lngNode.textContent.toDouble()
                listGeopoints.add(LatLng(lat, lng))
                locationNode = nl2.item(getNodeIndex(nl2, "polyline"))
                nl3 = locationNode.childNodes
                latNode = nl3.item(getNodeIndex(nl3, "points"))
                val arr = decodePoly(latNode.textContent)
                for (j in arr.indices) {
                    listGeopoints.add(LatLng(arr[j].latitude
                            , arr[j].longitude))
                }
                locationNode = nl2.item(getNodeIndex(nl2, "end_location"))
                nl3 = locationNode.childNodes
                latNode = nl3.item(getNodeIndex(nl3, "lat"))
                lat = latNode.textContent.toDouble()
                lngNode = nl3.item(getNodeIndex(nl3, "lng"))
                lng = lngNode.textContent.toDouble()
                listGeopoints.add(LatLng(lat, lng))
            }
        }
        return listGeopoints
    }

    fun getSection(doc: Document): ArrayList<LatLng> {
        val nl1: NodeList
        var nl2: NodeList
        var nl3: NodeList
        val listGeopoints = ArrayList<LatLng>()
        nl1 = doc.getElementsByTagName("step")
        if (nl1.length > 0) {
            for (i in 0 until nl1.length) {
                val node1 = nl1.item(i)
                nl2 = node1.childNodes
                val locationNode = nl2.item(getNodeIndex(nl2, "end_location"))
                nl3 = locationNode.childNodes
                val latNode = nl3.item(getNodeIndex(nl3, "lat"))
                val lat = latNode.textContent.toDouble()
                val lngNode = nl3.item(getNodeIndex(nl3, "lng"))
                val lng = lngNode.textContent.toDouble()
                listGeopoints.add(LatLng(lat, lng))
            }
        }
        return listGeopoints
    }

    fun getPolyline(doc: Document, width: Int, color: Int): PolylineOptions {
        val arr_pos = getDirection(doc)
        val rectLine = PolylineOptions().width(dpToPx(width).toFloat()).color(color)
        for (i in arr_pos.indices) rectLine.add(arr_pos[i])
        return rectLine
    }

    private fun getNodeIndex(nl: NodeList, nodename: String): Int {
        for (i in 0 until nl.length) {
            if (nl.item(i).nodeName == nodename) return i
        }
        return -1
    }

    private fun decodePoly(encoded: String): ArrayList<LatLng> {
        val poly = ArrayList<LatLng>()
        var index = 0
        val len = encoded.length
        var lat = 0
        var lng = 0
        while (index < len) {
            var b: Int
            var shift = 0
            var result = 0
            do {
                b = encoded[index++].toInt() - 63
                result = result or (b and 0x1f) shl shift
                shift += 5
            } while (b >= 0x20)
            val dlat = if (result and 1 != 0) (result shr 1).inv() else result shr 1
            lat += dlat
            shift = 0
            result = 0
            do {
                b = encoded[index++].toInt() - 63
                result = result or (b and 0x1f) shl shift
                shift += 5
            } while (b >= 0x20)
            val dlng = if (result and 1 != 0) (result shr 1).inv() else result shr 1
            lng += dlng
            val position = LatLng(lat.toDouble() / 1E5, lng.toDouble() / 1E5)
            poly.add(position)
        }
        return poly
    }

    private fun dpToPx(dp: Int): Int {
        val displayMetrics = mContext!!.resources.displayMetrics
        return Math.round(dp * (displayMetrics.xdpi / DisplayMetrics.DENSITY_DEFAULT))
    }

    fun setOnDirectionResponseListener(listener: OnDirectionResponseListener?) {
        mDirectionListener = listener
    }

    fun setOnAnimateListener(listener: OnAnimateListener?) {
        mAnimateListener = listener
    }

    interface OnDirectionResponseListener {
        fun onResponse(status: String?, doc: Document?, gd: DirectionHelper?)
    }

    interface OnAnimateListener {
        fun onFinish()
        fun onStart()
        fun onProgress(progress: Int, total: Int)
    }

    fun animateDirection(gm: GoogleMap, direction: ArrayList<LatLng>, speed: Int
                         , cameraLock: Boolean, isCameraTilt: Boolean, isCameraZoom: Boolean
                         , drawMarker: Boolean, mo: MarkerOptions?, flatMarker: Boolean
                         , drawLine: Boolean, po: PolylineOptions?) {
        if (direction.size > 1) {
            isAnimated = true
            animatePositionList = direction
            animateSpeed = speed
            this.drawMarker = drawMarker
            this.drawLine = drawLine
            this.flatMarker = flatMarker
            this.isCameraTilt = isCameraTilt
            this.isCameraZoom = isCameraZoom
            step = 0
            this.cameraLock = cameraLock
            this.gm = gm
            setCameraUpdateSpeed(speed)
            beginPosition = animatePositionList!![step]
            endPosition = animatePositionList!![step + 1]
            animateMarkerPosition = beginPosition
            if (mAnimateListener != null) mAnimateListener!!.onProgress(step, animatePositionList!!.size)
            if (cameraLock) {
                val bearing = getBearing(beginPosition, endPosition)
                val cameraBuilder = CameraPosition.Builder()
                        .target(animateMarkerPosition).bearing(bearing)
                if (isCameraTilt) cameraBuilder.tilt(90f) else cameraBuilder.tilt(gm.cameraPosition.tilt)
                if (isCameraZoom) cameraBuilder.zoom(zoom.toFloat()) else cameraBuilder.zoom(gm.cameraPosition.zoom)
                val cameraPosition = cameraBuilder.build()
                gm.animateCamera(CameraUpdateFactory.newCameraPosition(cameraPosition))
            }
            if (drawMarker) {
                animateMarker = if (mo != null) gm.addMarker(mo.position(beginPosition!!)) else gm.addMarker(MarkerOptions().position(beginPosition!!))
                if (flatMarker) {
                    animateMarker!!.setFlat(true)
                    val rotation = getBearing(animateMarkerPosition, endPosition) + 180
                    animateMarker!!.setRotation(rotation)
                }
            }
            if (drawLine) {
                if (po != null) animatePolyline = gm.addPolyline(po.add(beginPosition)
                        .add(beginPosition).add(endPosition)
                        .width(dpToPx(po.width.toInt()).toFloat())) else animatePolyline = gm.addPolyline(PolylineOptions()
                        .width(dpToPx(5).toFloat()))
            }
            Handler().postDelayed(r, speed.toLong())
            if (mAnimateListener != null) mAnimateListener!!.onStart()
        }
    }

    fun cancelAnimated() {
        isAnimated = false
    }

    private val r: Runnable = object : Runnable {
        override fun run() {
            animateMarkerPosition = getNewPosition(animateMarkerPosition, endPosition)
            if (drawMarker) animateMarker!!.position = animateMarkerPosition!!
            if (drawLine) {
                val points = animatePolyline!!.points
                points.add(animateMarkerPosition)
                animatePolyline!!.points = points
            }
            if (animateMarkerPosition!!.latitude == endPosition!!.latitude
                    && animateMarkerPosition!!.longitude == endPosition!!.longitude) {
                if (step == animatePositionList!!.size - 2) {
                    isAnimated = false
                    totalAnimateDistance = 0.0
                    if (mAnimateListener != null) mAnimateListener!!.onFinish()
                } else {
                    step++
                    beginPosition = animatePositionList!![step]
                    endPosition = animatePositionList!![step + 1]
                    animateMarkerPosition = beginPosition
                    if (flatMarker && step + 3 < animatePositionList!!.size - 1) {
                        val rotation = getBearing(animateMarkerPosition, animatePositionList!![step + 3]) + 180
                        animateMarker!!.rotation = rotation
                    }
                    if (mAnimateListener != null) mAnimateListener!!.onProgress(step, animatePositionList!!.size)
                }
            }
            if (cameraLock && (totalAnimateDistance > animateCamera || !isAnimated)) {
                totalAnimateDistance = 0.0
                val bearing = getBearing(beginPosition, endPosition)
                val cameraBuilder = CameraPosition.Builder()
                        .target(animateMarkerPosition).bearing(bearing)
                if (isCameraTilt) cameraBuilder.tilt(90f) else cameraBuilder.tilt(gm!!.cameraPosition.tilt)
                if (isCameraZoom) cameraBuilder.zoom(zoom.toFloat()) else cameraBuilder.zoom(gm!!.cameraPosition.zoom)
                val cameraPosition = cameraBuilder.build()
                gm!!.animateCamera(CameraUpdateFactory.newCameraPosition(cameraPosition))
            }
            if (isAnimated) {
                Handler().postDelayed(this, animateSpeed.toLong())
            }
        }
    }

    private fun getNewPosition(begin: LatLng?, end: LatLng?): LatLng? {
        val lat = Math.abs(begin!!.latitude - end!!.latitude)
        val lng = Math.abs(begin.longitude - end.longitude)
        val dis = Math.sqrt(Math.pow(lat, 2.0) + Math.pow(lng, 2.0))
        return if (dis >= animateDistance) {
            var angle = -1.0
            if (begin.latitude <= end.latitude && begin.longitude <= end.longitude) angle = Math.toDegrees(Math.atan(lng / lat)) else if (begin.latitude > end.latitude && begin.longitude <= end.longitude) angle = 90 - Math.toDegrees(Math.atan(lng / lat)) + 90 else if (begin.latitude > end.latitude && begin.longitude > end.longitude) angle = Math.toDegrees(Math.atan(lng / lat)) + 180 else if (begin.latitude <= end.latitude && begin.longitude > end.longitude) angle = 90 - Math.toDegrees(Math.atan(lng / lat)) + 270
            val x = Math.cos(Math.toRadians(angle)) * animateDistance
            val y = Math.sin(Math.toRadians(angle)) * animateDistance
            totalAnimateDistance += animateDistance
            val finalLat = begin.latitude + x
            val finalLng = begin.longitude + y
            LatLng(finalLat, finalLng)
        } else {
            end
        }
    }

    private fun getBearing(begin: LatLng?, end: LatLng?): Float {
        val lat = Math.abs(begin!!.latitude - end!!.latitude)
        val lng = Math.abs(begin.longitude - end.longitude)
        if (begin.latitude < end.latitude && begin.longitude < end.longitude) return Math.toDegrees(Math.atan(lng / lat)).toFloat() else if (begin.latitude >= end.latitude && begin.longitude < end.longitude) return (90 - Math.toDegrees(Math.atan(lng / lat)) + 90).toFloat() else if (begin.latitude >= end.latitude && begin.longitude >= end.longitude) return (Math.toDegrees(Math.atan(lng / lat)) + 180).toFloat() else if (begin.latitude < end.latitude && begin.longitude >= end.longitude) return (90 - Math.toDegrees(Math.atan(lng / lat)) + 270).toFloat()
        return (-1).toFloat()
    }

    fun setCameraUpdateSpeed(speed: Int) {
        if (speed == SPEED_VERY_SLOW) {
            animateDistance = 0.000005
            animateSpeed = 20
            animateCamera = 0.0004
            zoom = 19
        } else if (speed == SPEED_SLOW) {
            animateDistance = 0.00001
            animateSpeed = 20
            animateCamera = 0.0008
            zoom = 18
        } else if (speed == SPEED_NORMAL) {
            animateDistance = 0.00005
            animateSpeed = 20
            animateCamera = 0.002
            zoom = 16
        } else if (speed == SPEED_FAST) {
            animateDistance = 0.0001
            animateSpeed = 20
            animateCamera = 0.004
            zoom = 15
        } else if (speed == SPEED_VERY_FAST) {
            animateDistance = 0.0005
            animateSpeed = 20
            animateCamera = 0.004
            zoom = 13
        } else {
            animateDistance = 0.00005
            animateSpeed = 20
            animateCamera = 0.002
            zoom = 16
        }
    }

    companion object {
        const val MODE_DRIVING = "driving"
        const val MODE_WALKING = "walking"
        const val MODE_BICYCLING = "bicycling"
        const val STATUS_OK = "OK"
        const val STATUS_NOT_FOUND = "NOT_FOUND"
        const val STATUS_ZERO_RESULTS = "ZERO_RESULTS"
        const val STATUS_MAX_WAYPOINTS_EXCEEDED = "MAX_WAYPOINTS_EXCEEDED"
        const val STATUS_INVALID_REQUEST = "INVALID_REQUEST"
        const val STATUS_OVER_QUERY_LIMIT = "OVER_QUERY_LIMIT"
        const val STATUS_REQUEST_DENIED = "REQUEST_DENIED"
        const val STATUS_UNKNOWN_ERROR = "UNKNOWN_ERROR"
        private const val SPEED_VERY_FAST = 1
        private const val SPEED_FAST = 2
        private const val SPEED_NORMAL = 3
        private const val SPEED_SLOW = 4
        private const val SPEED_VERY_SLOW = 5
    }

    init {
        mContext = context
    }
}