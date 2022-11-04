package com.example.squirrel

import android.app.Application
import android.content.SharedPreferences
import android.preference.PreferenceManager
import com.android.volley.RequestQueue
import com.android.volley.toolbox.Volley

class Program : Application()  {

    private var _preferences: SharedPreferences? = null

    companion object {
        private const val SET_COOKIE_KEY = "Set-Cookie"
        private const val COOKIE_KEY = "Cookie"
        private const val SESSION_COOKIE = ".AspNetCore.Identity.Application"
        private var instance: Program? = null
        private var requestQueue: RequestQueue? = null

        fun get(): Program? = instance
        fun getRequestQueue(): RequestQueue? = requestQueue
    }

    fun checkSessionCookie(headers: Map<String?, String>) {
        if (headers.containsKey(SET_COOKIE_KEY) && headers[SET_COOKIE_KEY]!!.contains(SESSION_COOKIE)) {
            var cookie = headers[SET_COOKIE_KEY]
            if (cookie!!.length > 0) {
                val splitCookie = cookie.split(";").toTypedArray()
                val splitSessionId = splitCookie[0].split("=").toTypedArray()
                cookie = splitSessionId[1]
                val prefEditor: SharedPreferences.Editor = _preferences!!.edit()
                prefEditor.putString(SESSION_COOKIE, cookie)
                prefEditor.commit()
            }
        }
    }

    fun addSessionCookie(headers: MutableMap<String?, String?>) {
        val sessionId = _preferences!!.getString(SESSION_COOKIE, "")
        if (sessionId!!.length > 0) {
            val builder = StringBuilder()
            builder.append(SESSION_COOKIE)
            builder.append("=")
            builder.append(sessionId)
            if (headers.containsKey(COOKIE_KEY)) {
                builder.append("; ")
                builder.append(headers[COOKIE_KEY])
            }
            headers[COOKIE_KEY] = builder.toString()
        }
    }
    override fun onCreate() {
        super.onCreate()
        instance = this
        _preferences = PreferenceManager.getDefaultSharedPreferences(this)
        requestQueue = Volley.newRequestQueue(this)
    }

}