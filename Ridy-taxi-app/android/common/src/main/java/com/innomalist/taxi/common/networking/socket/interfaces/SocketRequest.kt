package com.innomalist.taxi.common.networking.socket.interfaces

import android.util.Log
import com.innomalist.taxi.common.utils.Adapters
import com.squareup.moshi.JsonAdapter
import com.squareup.moshi.Types
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject
import java.lang.reflect.Type


abstract class SocketRequest {
    val event: String
        get() {
            return this.javaClass.name.split('.').last()
        }
    var params: Array<Any>? = null


    constructor()

    constructor(params: Array<Any>) {
        this.params = params
    }

    inline fun <reified T> execute(dispatcher: NetworkDispatcher = SocketNetworkDispatcher.instance, crossinline completionHandler: (RemoteResponse<T, RemoteError>) -> Unit) {
        dispatcher.dispatch(event, params = params) {
            when(it) {
                is RemoteResponse.Success -> {
                    if(it.body is T) {
                        GlobalScope.launch(Dispatchers.Main) {
                            completionHandler(RemoteResponse.createSuccess(it.body as T))
                        }
                        return@dispatch
                    }
                    if(it.body is JSONObject) {
                        try {
                            val json = JSONObject(it.body.toString())
                            if (json.has("message") && json.has("status")) {
                                val error = Adapters.moshi.adapter<RemoteError>(RemoteError::class.java).fromJson(json.toString())
                                GlobalScope.launch(Dispatchers.Main) {
                                    completionHandler(RemoteResponse.createError(error!!))
                                }

                            } else {
                                val item = Adapters.moshi.adapter<T>(T::class.java).fromJson(json.toString())
                                GlobalScope.launch(Dispatchers.Main) {
                                    completionHandler(RemoteResponse.createSuccess(item!!))
                                }
                            }
                            return@dispatch
                        } catch (exception: Exception) {
                            Log.e("Failed to decode object", exception.message!!)
                            GlobalScope.launch(Dispatchers.Main) {
                                completionHandler(RemoteResponse.createError(RemoteError(ErrorStatus.FailedEncoding)))
                            }
                            return@dispatch
                        }
                    }
                    GlobalScope.launch(Dispatchers.Main) {
                        completionHandler(RemoteResponse.createError(RemoteError(ErrorStatus.FailedEncoding)))
                    }
                }

                is RemoteResponse.Error -> {
                    completionHandler(RemoteResponse.createError(RemoteError(ErrorStatus.Networking)))
                }
            }
        }
    }
    inline fun <reified T> executeArray(dispatcher: NetworkDispatcher = SocketNetworkDispatcher.instance, crossinline completionHandler: (RemoteResponse<ArrayList<T>, RemoteError>) -> Unit) {
        dispatcher.dispatch(event, params = params) {
            when(it) {
                is RemoteResponse.Success -> {
                    if(it.body is JSONArray) {
                        try {
                            val type: Type = Types.newParameterizedType(MutableList::class.java, T::class.java)
                            val adapter: JsonAdapter<ArrayList<T>> = Adapters.moshi.adapter(type)
                            val item = adapter.fromJson(it.body.toString())
                            GlobalScope.launch(Dispatchers.Main) {
                                completionHandler(RemoteResponse.createSuccess(item!!))
                            }
                        } catch (exception: Exception) {
                            Log.e("Failed to decode array", exception.message!!)
                            GlobalScope.launch(Dispatchers.Main) {
                                completionHandler(RemoteResponse.createError(RemoteError(ErrorStatus.FailedEncoding)))
                            }
                            return@dispatch
                        }
                    } else {
                        GlobalScope.launch(Dispatchers.Main) {
                            completionHandler(RemoteResponse.createError(RemoteError(ErrorStatus.FailedEncoding)))
                        }
                    }
                }

                is RemoteResponse.Error -> {
                    completionHandler(RemoteResponse.createError(RemoteError(ErrorStatus.Networking)))
                }
            }
        }
    }
}

class EmptyClass
