package com.example.squirrel.entities

import java.util.Date

data class Transaction(var Id: Int, var Time: Date, var Amount: Double, var Description: String,var CategoryId: Int,var Category: Category)
