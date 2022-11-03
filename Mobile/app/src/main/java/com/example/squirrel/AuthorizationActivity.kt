package com.example.squirrel

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Response
import com.android.volley.VolleyError
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import com.example.squirrel.databinding.ActivityAuthorizationBinding
import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject
import java.io.UnsupportedEncodingException


class AuthorizationActivity : AppCompatActivity()  {

    private lateinit var binding : ActivityAuthorizationBinding

    override fun onCreate(savedInstanceState: Bundle?) {

        supportActionBar?.hide()

        super.onCreate(savedInstanceState)
        binding = ActivityAuthorizationBinding.inflate(layoutInflater)
        setContentView(binding.root)
    }

    fun onClickGoRegister (view: View) {

        val intent = Intent(this, RegisterActivity::class.java)
        startActivity(intent)

    }

    fun checkOnEmpty () : Boolean {
        return binding.loginPrompt.text.toString() == "" || binding.passwordPrompt.text.toString() == ""
    }

    fun onClickSignIn (view: View) {
        val intent = Intent(this, RegisterActivity::class.java)
        if (checkOnEmpty()) {
            Toast.makeText(this, "Fill in all the fields", Toast.LENGTH_LONG).show()
            return
        }
        var context = this;
        val queue = Volley.newRequestQueue(this)
        val url = "http://geneirodan.zapto.org:23451/api/account/authenticate?callbackurl=geneirodan.zapto.org"
        val jsonBody = JSONObject()
        jsonBody.put("email", binding.loginPrompt.text.toString())
        jsonBody.put("password", binding.passwordPrompt.text.toString())
        var requestBody = jsonBody.toString()
        val stringRequest: StringRequest = object : StringRequest(
            Method.POST,
            url,
            object : Response.Listener<String?> {
                override fun onResponse(response: String?) {
                    Toast.makeText(context, "Welcome!", Toast.LENGTH_LONG).show()
                    startActivity(intent)
                }
            },
            object : Response.ErrorListener {
                override fun onErrorResponse(error: VolleyError) {
                    var response = error.networkResponse;
                    if (response != null) {
                        try {
                            var res = String(response.data, Charsets.UTF_8)
                            var arr = JSONArray(res).toList()
                            for(x in arr)
                                Toast.makeText(context, x.toString(), Toast.LENGTH_SHORT).show()
                        } catch (e1 : UnsupportedEncodingException) {
                            e1.printStackTrace();
                        } catch (e2 : JSONException) {
                            e2.printStackTrace();
                        }
                    }
                }
            }) {
            override fun getBodyContentType(): String = "application/json; charset=utf-8"
            override fun getBody(): ByteArray = requestBody.toByteArray(Charsets.UTF_8)
        }
        queue.add(stringRequest)
    }
    @Throws(JSONException::class)
    fun JSONObject.toMap(): Map<String, Any> {
        val map = mutableMapOf<String, Any>()
        val keysItr: Iterator<String> = this.keys()
        while (keysItr.hasNext()) {
            val key = keysItr.next()
            var value: Any = this.get(key)
            when (value) {
                is JSONArray -> value = value.toList()
                is JSONObject -> value = value.toMap()
            }
            map[key] = value
        }
        return map
    }

    @Throws(JSONException::class)
    fun JSONArray.toList(): List<Any> {
        val list = mutableListOf<Any>()
        for (i in 0 until this.length()) {
            var value: Any = this[i]
            when (value) {
                is JSONArray -> value = value.toList()
                is JSONObject -> value = value.toMap()
            }
            list.add(value)
        }
        return list
    }
}