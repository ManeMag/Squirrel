package com.example.squirrel.entities

import java.time.Period
import java.util.Date

data class Subscription(var Id: Int,var Name: String, var Day: Date,var Price: Double,var Period: Period,var UserId: String)
