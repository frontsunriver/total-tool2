package com.innomalist.taxi.common.networking.http.interfaces

import android.annotation.SuppressLint
import com.innomalist.taxi.common.Config
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import org.json.JSONObject
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL
import java.nio.charset.Charset
import java.util.*


class HTTPNetworkDispatcher: HTTPNetworkDispatcherBase {
    companion object {
        var instance = HTTPNetworkDispatcher()
    }

    @SuppressLint("CheckResult")
    override fun dispatch(path: String, params: Map<String, Any>?, completionHandler: (RemoteResponse<Any, HTTPStatusCode>) -> Unit) {
        GlobalScope.launch(Dispatchers.Main) {
            val result = this@HTTPNetworkDispatcher.doIt(path, params)
            completionHandler(result)
        }
    }
    private suspend fun doIt(path: String, params: Map<String, Any>?): RemoteResponse<Any, HTTPStatusCode> = withContext(Dispatchers.IO) {
        val url = URL("${Config.Backend}$path")
        val client = url.openConnection() as HttpURLConnection
        client.requestMethod = "POST"
        client.doOutput = true
        client.doInput = true
        client.setRequestProperty("Accept", "application/json")
        client.setRequestProperty("Content-Type", "application/json")
        val postDataParams = HashMap<String, Any>()
        for (param in params!!.iterator()) {
            postDataParams[param.key] = param.value
        }
        val bts = JSONObject(params).toString().toByteArray(Charset.forName("UTF-8"))
        client.outputStream.write(bts)
        client.outputStream.close()
        if(client.responseCode == 200) {
            val reader = BufferedReader(InputStreamReader(client.inputStream))
            val sb = StringBuilder()
            var line: String?
            while (reader.readLine().also { line = it } != null) sb.append(line)
            return@withContext RemoteResponse.createSuccess<Any, HTTPStatusCode>(sb.toString())
        } else {
            return@withContext RemoteResponse.createError<Any, HTTPStatusCode>(HTTPStatusCode.invoke(client.responseCode) ?: HTTPStatusCode.Unknown)
        }
    }
}