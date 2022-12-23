package com.example.squirrel.models

import com.example.squirrel.entities.Transaction
import kotlinx.datetime.LocalDateTime
import kotlinx.serialization.Serializable

@Serializable
class Impact(
    var categoryId: Int,
    var positivePercentage: Double,
    var income: Double,
    var negativePercentage: Double,
    var outcome: Double,
    var transactionsCount: Int
)
@Serializable
class Statistics (
    var impact : List<Impact>,
    var transactions: List<Transaction>,
    var startDate : LocalDateTime,
    var endDate : LocalDateTime,
    var receivedAt: String,
    var income : Double,
    var outcome : Double
    )