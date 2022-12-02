package com.example.squirrel

import android.os.Bundle
import android.view.View
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController

class TransactionSpendings:Fragment(R.layout.fragment_transaction_spendings) {
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        view.findViewById<TextView>(R.id.buttonSpendings).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transaction_to_transactionSpendings)
        }
        view.findViewById<TextView>(R.id.buttonIncome).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transactionSpendings_to_nav_fragment_transaction)
        }

    }
}