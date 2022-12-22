package com.example.squirrel.util

import android.content.SharedPreferences
import android.util.Log
import io.ktor.http.*

class PreferenceCookiesStorage(private val preferences: SharedPreferences) :
    io.ktor.client.plugins.cookies.CookiesStorage {
    override suspend fun addCookie(requestUrl: Url, cookie: Cookie) {
        with(preferences.edit()) {
            this.putString(cookie.name, cookie.value)
            apply()
        }
    }

    override fun close() {}

    override suspend fun get(requestUrl: Url): List<Cookie> = mutableListOf<Cookie>().apply {
        preferences.all.forEach { cookie ->
            try {
                add(Cookie(cookie.key, cookie.value.toString()))
            } catch (e: Exception) {
                Log.e("Cookie", "Bad cookie")
            }
        }
    }
}