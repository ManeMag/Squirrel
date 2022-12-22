package com.example.squirrel.models

import kotlinx.datetime.LocalDateTime
import kotlinx.serialization.Serializable

@Serializable
class Impact(
    var categoryId: Int,
    var totalPercentage: Double,
    var totalMoney: Double,
    var transactionsCount: Int
)
@Serializable
class Statistics (
    var impact : List<Impact>,
    var startDate : LocalDateTime,
    var endDate : LocalDateTime,
    var receivedAt: String,
    var income : Double,
    var outcome : Double
    )