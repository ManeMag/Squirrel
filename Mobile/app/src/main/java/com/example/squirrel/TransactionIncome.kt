package com.example.squirrel

import android.app.DatePickerDialog
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.View
import android.widget.DatePicker
import android.widget.EditText
import android.widget.PopupMenu
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import io.ktor.client.request.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.launch
import java.text.SimpleDateFormat
import java.util.*
import com.example.squirrel.entities.Category
import io.ktor.client.call.*

class TransactionIncome:Fragment(R.layout.fragment_transaction), DatePickerDialog.OnDateSetListener {


    private lateinit var layout: View
    private val calendar = Calendar.getInstance()
    private val formatter = SimpleDateFormat("MM.dd.yyyy ", Locale.US)
    var listCategory = emptyList<Category>()


    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view
        var menuCategory = PopupMenu(layout.context, layout.findViewById(R.id.categoryPickButton))

        layout.findViewById<TextView>(R.id.buttonSpendings).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transaction_to_transactionSpendings)
        }
        layout.findViewById<TextView>(R.id.buttonIncome).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transactionSpendings_to_nav_fragment_transaction)
        }

        layout.findViewById<TextView>(R.id.datePickButton).setOnClickListener() {
            DatePickerDialog(
                requireContext(),
                this,
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)
            ).show()
        }

        layout.findViewById<EditText>(R.id.categoryText).setEnabled(false)
        layout.findViewById<EditText>(R.id.datePrompt).setEnabled(false)


        lifecycleScope.launch{
            val response: HttpResponse = Program.client.get("${Program.protocol}://${Program.domain}:${Program.port}/api/category")
            if(response.status == HttpStatusCode.OK)
            {
                Log.e("error",response.body<String>().toString())
                listCategory = response.body()
                for(category in listCategory){
                    Log.i("tag",category.name)
                }
            }
            else {
                Toast.makeText(context, "Something went wrong", Toast.LENGTH_LONG).show()
            }

            var count = 0
            for (category in listCategory)
            {
                menuCategory.menu.add(Menu.NONE,count,count,category.name)
                count++
            }
            menuCategory.setOnMenuItemClickListener {menuItem ->
                val id = menuItem.itemId
                layout.findViewById<EditText>(R.id.categoryText).setText(menuItem.title)
                false
            }
            layout.findViewById<TextView>(R.id.categoryPickButton).setOnClickListener{
                menuCategory.show()
            }
        }


    }


    override fun onDateSet(view: DatePicker?, year: Int, month: Int, dayOfMonth: Int) {
        calendar.set(year, month, dayOfMonth)
        displayFormattedDate(calendar.timeInMillis)
    }

    private fun categoryMenuCreate()
    {

    }

    private fun displayFormattedDate(timestamp: Long){
        layout.findViewById<EditText>(R.id.datePrompt).setText(formatter.format(timestamp))
    }
}