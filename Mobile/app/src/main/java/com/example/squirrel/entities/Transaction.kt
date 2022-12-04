package com.example.squirrel.entities

import kotlinx.datetime.DateTimeUnit
import kotlinx.serialization.Serializable
@Serializable
data class Transaction(
    var id: Int,
    var time: DateTimeUnit,
    var amount: Double,
    var description: String,
    var categoryId: Int,
    var category: Category
)

