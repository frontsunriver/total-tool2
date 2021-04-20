package com.innomalist.taxi.common.networking.socket.interfaces

interface NetworkDispatcher {
    fun dispatch(event: String, params: Array<Any>?, completionHandler: (RemoteResponse<Any, SocketClientError>) -> Unit)
}

sealed class RemoteResponse<T, E> {
    data class Success<T, E>(val body: T) : RemoteResponse<T, E>()
    data class Error<T, E>(val error: E) : RemoteResponse<T, E>()
    companion object {
        fun <T, E> createError(error: E): RemoteResponse<T, E> {
            return Error(error)
        }
        fun <T, E> createSuccess(data: T): RemoteResponse<T, E> {
            return Success(data)
        }


    }
}