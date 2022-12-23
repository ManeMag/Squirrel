package com.example.squirrel

import android.app.Application
import android.content.SharedPreferences
import com.example.squirrel.util.PreferenceCookiesStorage
import io.ktor.client.*
import io.ktor.client.engine.cio.*
import io.ktor.client.plugins.contentnegotiation.*
import io.ktor.client.plugins.cookies.*
import io.ktor.serialization.kotlinx.json.*

class Program : Application()  {
    companion object {
        lateinit var preferences: SharedPreferences
        val client: HttpClient
            get() = HttpClient(CIO) {
                expectSuccess = false
                install(HttpCookies){
                    storage = PreferenceCookiesStorage(preferences)
                }
                install(ContentNegotiation){
                    json()
                }
            }
        const val protocol = "http"
        val domain = "geneirodan.zapto.org"
        val port = "23451"
    }
}
