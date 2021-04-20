package com.innomalist.taxi.common.utils

import android.graphics.Color
import android.os.AsyncTask
import android.util.Log
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Polyline
import com.google.android.gms.maps.model.PolylineOptions
import org.json.JSONArray
import org.json.JSONObject
import java.io.BufferedReader
import java.io.IOException
import java.io.InputStream
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import java.util.*

class DirectionsJSONParser(private val googleMap: GoogleMap, private val origin: LatLng, private val destination: LatLng) {
    private var line: Polyline? = null
    fun run() {
        val url = getDirectionsUrl(origin, destination)
        val downloadTask = DownloadTask()
        // Start downloading json data from Google Directions API
        downloadTask.execute(url)
    }

    fun removeLine() {
        if (line != null) line!!.remove()
    }

    private fun parse(jObject: JSONObject): List<List<HashMap<String, String>>> {
        val routes: MutableList<List<HashMap<String, String>>> = ArrayList()
        val jRoutes: JSONArray
        var jLegs: JSONArray
        var jSteps: JSONArray
        var jDistance: JSONObject
        var jDuration: JSONObject
        try {
            jRoutes = jObject.getJSONArray("routes")
            // Traversing all routes
            for (i in 0 until jRoutes.length()) {
                jLegs = (jRoutes[i] as JSONObject).getJSONArray("legs")
                val path: MutableList<HashMap<String, String>> = ArrayList()
                // Traversing all legs
                for (j in 0 until jLegs.length()) { // Getting distance from the json data
                    jDistance = (jLegs[j] as JSONObject).getJSONObject("distance")
                    val hmDistance = HashMap<String, String>()
                    hmDistance["distance"] = jDistance.getString("text")
                    // Getting duration from the json data
                    jDuration = (jLegs[j] as JSONObject).getJSONObject("duration")
                    val hmDuration = HashMap<String, String>()
                    hmDuration["duration"] = jDuration.getString("text")
                    // Adding distance object to the path
                    path.add(hmDistance)
                    // Adding duration object to the path
                    path.add(hmDuration)
                    jSteps = (jLegs[j] as JSONObject).getJSONArray("steps")
                    // Traversing all steps
                    for (k in 0 until jSteps.length()) {
                        var polyline: String
                        polyline = ((jSteps[k] as JSONObject)["polyline"] as JSONObject)["points"] as String
                        val list = decodePoly(polyline)
                        // Traversing all points
                        for (l in list.indices) {
                            val hm = HashMap<String, String>()
                            hm["lat"] = java.lang.Double.toString(list[l].latitude)
                            hm["lng"] = java.lang.Double.toString(list[l].longitude)
                            path.add(hm)
                        }
                    }
                }
                routes.add(path)
            }
        } catch (e: Exception) {
            e.printStackTrace()
        }
        return routes
    }

    private fun decodePoly(encoded: String): List<LatLng> {
        val poly: MutableList<LatLng> = ArrayList()
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
            val p = LatLng(lat.toDouble() / 1E5,
                    lng.toDouble() / 1E5)
            poly.add(p)
        }
        return poly
    }

    private fun getDirectionsUrl(origin: LatLng, dest: LatLng): String { // Origin of route
        val str_origin = "origin=" + origin.latitude + "," + origin.longitude
        // Destination of route
        val str_dest = "destination=" + dest.latitude + "," + dest.longitude
        // Sensor enabled
        val sensor = "sensor=false"
        // Building the parameters to the web service
        val parameters = "$str_origin&$str_dest&$sensor"
        // Output format
        val output = "json"
        // Building the url to the web service
        return "https://maps.googleapis.com/maps/api/directions/$output?$parameters"
    }

    @Throws(IOException::class)
    private fun downloadUrl(strUrl: String): String {
        var data = ""
        var iStream: InputStream? = null
        var urlConnection: HttpURLConnection? = null
        try {
            val url = URL(strUrl)
            // Creating an http connection to communicate with url
            urlConnection = url.openConnection() as HttpURLConnection
            // Connecting to url
            urlConnection.connect()
            // Reading data from url
            iStream = urlConnection.inputStream!!
            val br = BufferedReader(InputStreamReader(iStream))
            val sb = StringBuilder()
            var line: String?
            while (br.readLine().also { line = it } != null) {
                sb.append(line)
            }
            data = sb.toString()
            br.close()
        } catch (e: Exception) {
            Log.d("Exception downloading", e.toString())
        } finally {
            iStream?.close()
            urlConnection?.disconnect()
        }
        return data
    }

    private inner class DownloadTask : AsyncTask<String, Void, String>() {
        // Downloading data in non-ui thread
        override fun doInBackground(vararg url: String): String { // For storing data from web service
            var data = ""
            try { // Fetching the data from web service
                data = downloadUrl(url[0])
            } catch (e: Exception) {
                Log.d("Background Task", e.toString())
            }
            return data
        }

        // Executes in UI thread, after the execution of
// doInBackground()
        override fun onPostExecute(result: String) {
            super.onPostExecute(result)
            val parserTask = ParserTask()
            // Invokes the thread for parsing the JSON data
            parserTask.execute(result)
        }
    }

    private inner class ParserTask : AsyncTask<String, Int, List<List<HashMap<String, String>>>>() {
        // Parsing the data in non-ui thread
        override fun doInBackground(vararg jsonData: String): List<List<HashMap<String, String>>>? {
            val jObject: JSONObject
            var routes: List<List<HashMap<String, String>>>? = null
            try {
                jObject = JSONObject(jsonData[0])
                // Starts parsing data
                routes = parse(jObject)
            } catch (e: Exception) {
                e.printStackTrace()
            }
            return routes
        }

        // Executes in UI thread, after the parsing process
        override fun onPostExecute(result: List<List<HashMap<String, String>>>?) {
            var points: ArrayList<LatLng?>
            val lineOptions = PolylineOptions().width(10f).color(Color.parseColor("#2196F3")).geodesic(true)
            //var distance = ""
            //var duration = ""
            if (result == null || result.isEmpty()) { //TODO:Do Something About No Way Found
                return
            }
            // Traversing through all the routes
            for (i in result.indices) {
                points = ArrayList()
                // Fetching i-th route
                val path = result[i]
                // Fetching all the points in i-th route
                for (j in path.indices) {
                    val point = path[j]
                    if (j == 0) { // Get distance from the list
                        //distance = point["distance"]!!
                        continue
                    } else if (j == 1) { // Get duration from the list
                        //duration = point["duration"]!!
                        continue
                    }
                    val lat = point["lat"]!!.toDouble()
                    val lng = point["lng"]!!.toDouble()
                    val position = LatLng(lat, lng)
                    points.add(position)
                }
                // Adding all the points in the route to LineOptions
                lineOptions.addAll(points)
            }
            //tvDistanceDuration.setText("Distance:" + distance + ", Duration:" + duration);
// Drawing polyline in the Google Map for the i-th route
            line = googleMap.addPolyline(lineOptions)
        }
    }

}