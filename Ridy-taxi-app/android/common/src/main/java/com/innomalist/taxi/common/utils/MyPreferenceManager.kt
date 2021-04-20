package com.innomalist.taxi.common.utils

import android.content.Context
import android.content.SharedPreferences
import androidx.preference.PreferenceManager
import com.innomalist.taxi.common.models.Driver
import com.innomalist.taxi.common.models.Rider
import com.innomalist.taxi.common.models.Service
import com.squareup.moshi.JsonAdapter
import com.squareup.moshi.Types
import java.lang.reflect.Type

class MyPreferenceManager(mContext: Context?) {
    private val sharedPreferences: SharedPreferences = PreferenceManager.getDefaultSharedPreferences(mContext)
    var driver: Driver?
        get() {
            val str = getString("driver", null)
            return if(str != null) {
                Adapters.moshi.adapter(Driver::class.java).fromJson(str)
            } else {
                null
            }
        }
        set(value) {
            val str = Adapters.moshi.adapter(Driver::class.java).toJson(value)
            putString("driver", str)
        }

    var rider: Rider?
        get() {
            val str = getString("rider", null)
            return if(str != null) {
                Adapters.moshi.adapter(Rider::class.java).fromJson(str)
            } else {
                null
            }
        }
        set(value) {
            val str = Adapters.moshi.adapter(Rider::class.java).toJson(value)
            putString("rider", str)
        }

    var token: String?
        get() {
            return getString("token", null)
        }
        set(value) {
            putString("token", value)
        }

    var services: ArrayList<Service>?
        get() {
            val str = getString("services", null)
            return if(str != null) {
                val type: Type = Types.newParameterizedType(MutableList::class.java, Service::class.java)
                val adapter: JsonAdapter<ArrayList<Service>> = Adapters.moshi.adapter(type)
                adapter.fromJson(str)
            } else {
                null
            }
        }
        set(value) {
            val type: Type = Types.newParameterizedType(MutableList::class.java, Service::class.java)
            val adapter: JsonAdapter<ArrayList<Service>> = Adapters.moshi.adapter(type)
            putString("services", adapter.toJson(value))
        }

    private val editor: SharedPreferences.Editor
        get() = sharedPreferences.edit()

    fun putString(key: String?, value: String?): Boolean {
        return editor.putString(key, value).commit()
    }

    fun getString(key: String?, defValue: String?): String? {
        return sharedPreferences.getString(key, defValue)
    }

    fun putBoolean(key: String?, value: Boolean): Boolean {
        return editor.putBoolean(key, value).commit()
    }

    fun getBoolean(key: String?, defValue: Boolean): Boolean {
        return sharedPreferences.getBoolean(key, defValue)
    }

    fun remove(key: String?) {
        editor.remove(key).commit()
    }

    fun clearPreferences() {
        editor.clear().commit()
    }

    companion object {
        private var instance: MyPreferenceManager? = null
        @JvmStatic
        fun getInstance(context: Context): MyPreferenceManager {
            return if (instance == null) {
                MyPreferenceManager(context)
            } else {
                instance!!
            }
        }
    }

}