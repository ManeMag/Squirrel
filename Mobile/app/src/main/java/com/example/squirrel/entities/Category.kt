package com.example.squirrel.entities
import kotlinx.serialization.Serializable
@Serializable
data class Category(var id: Int,var name: String, var color: String, var transactions: List<Transaction>,var isBaseCategory: Boolean)
