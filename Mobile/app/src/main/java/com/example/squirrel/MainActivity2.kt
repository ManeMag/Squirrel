package com.example.squirrel

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import androidx.appcompat.widget.Toolbar
import androidx.navigation.NavController
import androidx.navigation.findNavController
import androidx.navigation.fragment.NavHostFragment
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.setupWithNavController
import com.google.android.material.navigation.NavigationView

class MainActivity2 : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main2)
        //val appBarConfiguration = AppBarConfiguration(navController.graph)
        //val toolbar = findViewById<Toolbar>(R.id.toolbar)
        //toolbar.setupWithNavController(navController,appBarConfiguration)
        //val bottomBar = findViewById<NavigationView>(R.id.bottom_nav_view)
        //bottomBar?.setupWithNavController(navController)
    }

}