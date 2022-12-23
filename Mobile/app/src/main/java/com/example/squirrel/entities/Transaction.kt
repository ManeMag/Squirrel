package com.example.squirrel.entities

import kotlinx.datetime.DateTimeUnit
import kotlinx.serialization.Serializable
@Serializable
data class Transaction(
    var id: Int,
    var time: kotlinx.datetime.LocalDateTime,
    var amount: Double,
    var description: String,
    var categoryId: Int
)

