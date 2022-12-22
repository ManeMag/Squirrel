package com.example.squirrel

import android.content.Context
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.example.squirrel.Program.Companion.preferences

open class BaseActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        preferences = getSharedPreferences("Squirrel", Context.MODE_PRIVATE)
        super.onCreate(savedInstanceState)
    }
}