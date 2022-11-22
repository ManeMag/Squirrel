package com.example.squirrel

import android.os.Bundle
import android.view.View
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.lifecycleScope
import com.example.squirrel.databinding.ActivityRegisterBinding
import io.ktor.client.call.*
import io.ktor.client.request.forms.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.launch


class RegisterActivity : AppCompatActivity() {

    private lateinit var binding: ActivityRegisterBinding

    override fun onCreate(savedInstanceState: Bundle?) {

        supportActionBar?.hide()

        super.onCreate(savedInstanceState)
        binding = ActivityRegisterBinding.inflate(layoutInflater)
        setContentView(binding.root)
        binding.enterButton.setOnTouchListener { _, _ ->
            println("ok")
            false}

    }

    fun onClickSignOn (view: View) {
        if (binding.loginPrompt.text.toString() == "" || binding.passwordPrompt.text.toString() == "" || binding.confirmPasswordPrompt.text.toString() == "") {
            Toast.makeText(this, "Fill in all the fields", Toast.LENGTH_LONG).show()
            return
        }
        val context = this
        lifecycleScope.launch{
            val response: HttpResponse = Program.client.submitForm(

                url = "${Program.protocol}://${Program.domain}:${Program.port}/api/account/login",
                formParameters = Parameters.build {
                    append("email", binding.loginPrompt.text.toString())
                    append("password", binding.passwordPrompt.text.toString())
                    append("confirmpassword", binding.confirmPasswordPrompt.text.toString())
                }
            )
            if(response.status == HttpStatusCode.OK) {
                Toast.makeText(context,"OK",Toast.LENGTH_LONG).show()
            }
            else {
                val arr = response.body<List<String>>()
                for(x in arr)
                    Toast.makeText(context, x, Toast.LENGTH_SHORT).show()
            }
        }
    }
}