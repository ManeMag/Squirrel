package com.example.squirrel.entities

import java.time.Period
import java.util.Date

data class Subscription(var id: Int,var name: String, var day: Date,var price: Double,var period: Period,var userId: String)
