package com.innomalist.taxi.common.networking.http.interfaces

import com.innomalist.taxi.common.networking.socket.interfaces.RemoteResponse
import android.content.Context
import android.util.Log
import com.innomalist.taxi.common.networking.socket.interfaces.EmptyClass
import com.innomalist.taxi.common.networking.socket.interfaces.ErrorStatus
import com.innomalist.taxi.common.networking.socket.interfaces.RemoteError
import com.innomalist.taxi.common.utils.Adapters
import com.innomalist.taxi.common.utils.AlertDialogBuilder
import com.squareup.moshi.JsonReader
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONObject
import java.io.IOException
import java.lang.Exception

abstract class HTTPRequest {
    abstract val path: String
    var params: Map<String, Any>? = mapOf()
    constructor()

    constructor(params: Map<String, Any>) {
        this.params = params
    }

    inline fun <reified T> execute(dispatcher: HTTPNetworkDispatcher = HTTPNetworkDispatcher.instance, crossinline completionHandler: (RemoteResponse<T, HTTPStatusCode>) -> Unit) {
        dispatcher.dispatch(this.path, this.params) {
            when(it) {
                is RemoteResponse.Success -> {
                    if(it.body is T) {
                        completionHandler(RemoteResponse.createSuccess(it.body as T))
                        return@dispatch
                    }
                    try {
                        var json: String;
                        if(it.body.toString() == "OK") {
                            json = "{}"
                        } else {
                            json = JSONObject(it.body.toString()).toString()
                        }

                        val item = Adapters.moshi.adapter<T>(T::class.java).fromJson(json)
                        completionHandler(RemoteResponse.createSuccess(item!!))
                    } catch (exception: Exception) {
                        Log.e("Failed to decode", exception.message!!)
                        GlobalScope.launch(Dispatchers.Main) {
                            completionHandler(RemoteResponse.createError(HTTPStatusCode.FailedToDecode))
                        }
                    }
                }

                is RemoteResponse.Error -> {
                    completionHandler(RemoteResponse.createError(it.error))
                }
            }
        }
    }
}

enum class HTTPStatusCode(val rawValue: Int) {
    InvalidCredentials(403), HardReject(411), NotFound(404), Unknown(666), InvalidURL(701), NoData(702), FailedToDecode(703), Networking(704);

    companion object {
        operator fun invoke(rawValue: Int) = values().firstOrNull { it.rawValue == rawValue }
        fun showAlert(context: Context) {
            AlertDialogBuilder.show(context, this.toString(), AlertDialogBuilder.DialogButton.OK, null)
        }
    }

    val localizedDescription: String
        get() {
            when (this) {
                    FailedToDecode -> return "Failed to decode."
                    Networking -> return "Networking Error."
                    NoData -> return "No Data Received."
                    InvalidURL -> return "Invalid URL."
                    Unknown -> return "Unknown."
                    NotFound -> return "Not Found."
                    HardReject -> return "Access has been disabled by admin."
                    InvalidCredentials -> return "Invalid Credentials."
            }
        }
}
