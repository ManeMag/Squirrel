package com.example.squirrel

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import com.example.squirrel.Program.Companion.client
import com.example.squirrel.Program.Companion.domain
import com.example.squirrel.Program.Companion.port
import com.example.squirrel.Program.Companion.protocol
import com.example.squirrel.databinding.ActivityAuthorizationBinding
import io.ktor.client.call.*
import io.ktor.client.request.*
import io.ktor.client.request.forms.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.launch

class AuthorizationActivity : AppCompatActivity()  {

    private lateinit var binding : ActivityAuthorizationBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        supportActionBar?.hide()
        super.onCreate(savedInstanceState)
        val intent = Intent(this, MainActivity::class.java)

        lifecycleScope.launch{
            val response: HttpResponse = client.get("$protocol://$domain:$port/api/account/email")
            if(response.status == HttpStatusCode.OK)
                startActivity(intent)
            else {
                binding = ActivityAuthorizationBinding.inflate(layoutInflater)
                setContentView(binding.root)
            }
        }
    }

    fun onClickGoRegister (view: View) = startActivity(Intent(this, RegisterActivity::class.java))

    fun onClickSignIn (view: View) {
        val intent = Intent(this, MainActivity::class.java)
        if (binding.loginPrompt.text.toString() == "" || binding.passwordPrompt.text.toString() == "") {
            Toast.makeText(this, "Fill in all the fields", Toast.LENGTH_LONG).show()
            return
        }
        val context = this
        lifecycleScope.launch{
            val response: HttpResponse = client.submitForm(
                url = "$protocol://$domain:$port/api/account/authenticate",
                formParameters = Parameters.build {
                    append("email", binding.loginPrompt.text.toString())
                    append("password", binding.passwordPrompt.text.toString())
                }
            )
            if(response.status == HttpStatusCode.OK) {
                Toast.makeText(context, "Welcome!", Toast.LENGTH_LONG).show()
                startActivity(intent)
            }
            else {
                val arr = response.body<List<String>>()
                for(x in arr)
                    Toast.makeText(context, x, Toast.LENGTH_SHORT).show()
            }
        }
    }
}