package com.example.squirrel

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.android.volley.Request
import com.android.volley.toolbox.Volley
import com.example.squirrel.databinding.ActivityAuthorizationBinding
import com.example.squirrel.http.StringRequest
import org.json.JSONArray
import org.json.JSONException
import org.json.JSONObject
import java.io.UnsupportedEncodingException


class AuthorizationActivity : AppCompatActivity()  {

    private lateinit var binding : ActivityAuthorizationBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        supportActionBar?.hide()
        super.onCreate(savedInstanceState)
        val intent = Intent(this, MainActivity::class.java)
        val stringRequest = StringRequest(
            Request.Method.GET,
            "http://geneirodan.zapto.org:23451/api/account/email",
            { startActivity(intent) },
            {
                binding = ActivityAuthorizationBinding.inflate(layoutInflater)
                setContentView(binding.root)
            },
            ""
        )
        Program.getRequestQueue()!!.add(stringRequest)
    }

    fun onClickGoRegister (view: View) = startActivity(Intent(this, RegisterActivity::class.java))

    fun onClickSignIn (view: View) {
        val intent = Intent(this, MainActivity::class.java)
        if (binding.loginPrompt.text.toString() == "" || binding.passwordPrompt.text.toString() == "") {
            Toast.makeText(this, "Fill in all the fields", Toast.LENGTH_LONG).show()
            return
        }
        val context = this
        val jsonBody = JSONObject()
        jsonBody.put("email", binding.loginPrompt.text.toString())
        jsonBody.put("password", binding.passwordPrompt.text.toString())
        val requestBody = jsonBody.toString()
        val stringRequest = StringRequest(
            Request.Method.POST,
            "http://geneirodan.zapto.org:23451/api/account/authenticate?callbackurl=geneirodan.zapto.org",
            {
                Toast.makeText(context, "Welcome!", Toast.LENGTH_LONG).show()
                startActivity(intent)
            },
            { error ->
                val response = error.networkResponse
                if (response != null) {
                    try {
                        val res = String(response.data, Charsets.UTF_8)
                        val arr = JSONArray(res).toList()
                        for(x in arr)
                            Toast.makeText(context, x.toString(), Toast.LENGTH_SHORT).show()
                    } catch (e1 : UnsupportedEncodingException) {
                        e1.printStackTrace()
                    } catch (e2 : JSONException) {
                        e2.printStackTrace()
                    }
                }
            }, requestBody)
        Program.getRequestQueue()!!.add(stringRequest)
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