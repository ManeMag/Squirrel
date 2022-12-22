package com.example.squirrel

import android.app.DatePickerDialog
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.EditText
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.Toolbar
import androidx.navigation.findNavController
import androidx.navigation.fragment.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.setupWithNavController
import com.example.squirrel.databinding.ActivityMainBinding
import com.google.android.material.bottomnavigation.BottomNavigationView
import io.ktor.client.request.*
import io.ktor.client.request.forms.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding


    override fun onCreate(savedInstanceState: Bundle?) {

        supportActionBar?.hide()

        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        val navView: BottomNavigationView = binding.navView

        val navController = findNavController(R.id.nav_host_fragment_activity_main)
        //val appBarConfiguration = AppBarConfiguration(navController.graph)
        val appBarConfiguration = AppBarConfiguration(setOf(
            R.id.nav_fragment_transaction,
            R.id.nav_fragment_statistics,
            R.id.nav_fragment_piggy_bank,
            R.id.nav_fragment_transactionSpendings,
            R.id.nav_statistic_income,
            R.id.nav_statistic_spendings
            )
        )
        val toolBar = findViewById<Toolbar>(R.id.toolbar)
        toolBar.setupWithNavController(navController,appBarConfiguration)
        navView.setupWithNavController(navController)

        //binding.toolbarInclude.toolbarCalendarButton.visibility = View.VISIBLE
    }

    private suspend fun greeting(): String {
        val response: HttpResponse = Program.client.submitForm(
            url = "http://geneirodan.zapto.org:23450/api/account/login",
            formParameters = Parameters.build {
                append("email", "admin@nextgenmail.com")
                append("password", "_Aa123456")
            }
        )
        return response.status.toString()
    }
}