package com.example.squirrel.http

import com.android.volley.AuthFailureError

import com.android.volley.NetworkResponse
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.example.squirrel.Program
import java.util.*


class StringRequest(
    method: Int,
    url: String?,
    listener: Response.Listener<String?>?,
    errorListener: Response.ErrorListener?,
    private val requestBody: String
) : StringRequest(method, url, listener, errorListener) {

    override fun parseNetworkResponse(response: NetworkResponse): Response<String> {
        Program.get()!!.checkSessionCookie(response.headers!!)
        return super.parseNetworkResponse(response)
    }

    @Throws(AuthFailureError::class)
    override fun getHeaders(): Map<String, String> {
        var headers = super.getHeaders()
        if (headers == null || headers == Collections.emptyMap<String, String>())
            headers = HashMap()
        Program.get()!!.addSessionCookie(headers)
        return headers
    }
    override fun getBodyContentType(): String = "application/json; charset=utf-8"

    override fun getBody(): ByteArray = requestBody.toByteArray(Charsets.UTF_8)
}