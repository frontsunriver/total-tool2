package com.innomalist.taxi.common.utils

import android.content.Context
import com.innomalist.taxi.common.models.Request

object TravelRepository {
    @JvmStatic
    operator fun get(context: Context?, app: AppType): Request {
        return Adapters.moshi.adapter<Request>(Request::class.java).fromJson(MyPreferenceManager.getInstance(context!!).getString(app.value + "_travel", "{}")!!)!!
    }

    @JvmStatic
    operator fun set(context: Context?, app: AppType, request: Request) {

        MyPreferenceManager.getInstance(context!!).putString(app.value + "_travel", Adapters.moshi.adapter<Request>(Request::class.java).toJson(request))
    }

    enum class AppType(val value: String) {
        DRIVER("driver"), RIDER("rider");

        companion object {
            operator fun get(code: String): AppType {
                for (s in values()) {
                    if (s.value == code) return s
                }
                return DRIVER
            }
        }

    }
}