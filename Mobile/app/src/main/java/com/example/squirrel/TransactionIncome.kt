package com.example.squirrel

import android.annotation.SuppressLint
import android.app.DatePickerDialog
import android.graphics.*
import android.graphics.drawable.GradientDrawable
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.View
import android.widget.DatePicker
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.widget.CustomPopupMenu
import androidx.core.content.ContextCompat.getDrawable
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import com.example.squirrel.entities.Category
import io.ktor.client.call.*
import io.ktor.client.request.*
import io.ktor.client.request.forms.*
import io.ktor.client.statement.*
import io.ktor.http.*
import io.ktor.util.date.*
import kotlinx.coroutines.launch
import java.text.SimpleDateFormat
import java.util.*


class TransactionIncome:Fragment(R.layout.fragment_transaction), DatePickerDialog.OnDateSetListener {
    private lateinit var layout: View
    private val calendar = Calendar.getInstance()
    private val formatter = SimpleDateFormat("MM.dd.yyyy", Locale.US)
    var listCategory = emptyList<Category>()
    var categoryId = 0


    @SuppressLint("UseCompatLoadingForDrawables")
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view
        var menuCategory = CustomPopupMenu(layout.context, layout.findViewById(R.id.categoryPickButton))

        layout.findViewById<TextView>(R.id.buttonSpendings).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transaction_to_transactionSpendings)
        }

        layout.findViewById<TextView>(R.id.datePickButton).setOnClickListener {
            val dialog = DatePickerDialog(
                requireContext(),
                this,
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)

            )
            dialog.datePicker.maxDate = Date().time
            dialog.show()
        }

        layout.findViewById<EditText>(R.id.categoryText).setEnabled(false)
        layout.findViewById<EditText>(R.id.datePrompt).setEnabled(false)

        // Filling category list
        lifecycleScope.launch{
            val response: HttpResponse = Program.client.get("${Program.protocol}://${Program.domain}:${Program.port}/api/category/1")
            if(response.status == HttpStatusCode.OK)
            {
                listCategory = response.body()
            }
            else {
                Toast.makeText(context, "Something went wrong", Toast.LENGTH_LONG).show()
            }


            // creating popup menu
            var count = 0
            categoryId = listCategory[0].id
            for (category in listCategory)
            {
                var drawable = getDrawable(requireContext(),R.drawable.oval)
                var gradientDrawable = drawable as GradientDrawable
                menuCategory.menu.add(Menu.NONE,category.id,count,category.name).apply {
                    gradientDrawable.setColor(255 shl 24 or  category.color.substring(1).toInt(16))
                    setIcon(gradientDrawable)
                }
                count++
            }

            menuCategory.setOnMenuItemClickListener {menuItem ->
                categoryId = menuItem.itemId
                layout.findViewById<EditText>(R.id.categoryText).setText(menuItem.title)
                false
            }
            layout.findViewById<TextView>(R.id.categoryPickButton).setOnClickListener{
                menuCategory.show()
            }
        }

        layout.findViewById<TextView>(R.id.createTransactionButton).setOnClickListener{
            lifecycleScope.launch{
                val response: HttpResponse = Program.client.post(
                    "${Program.protocol}://${Program.domain}:${Program.port}/api/transaction",
                )
                {
                    setBody(MultiPartFormDataContent(formData{
                        append("amount",layout.findViewById<EditText>(R.id.amountPrompt).text.toString())
                        append("description",layout.findViewById<EditText>(R.id.namePrompt).text.toString())
                        append("categoryId",categoryId)
                        append("time",layout.findViewById<EditText>(R.id.datePrompt).text.toString())
                    }))
                }
                if(response.status == HttpStatusCode.Created) {
                    Toast.makeText(context, "Transaction created!", Toast.LENGTH_LONG).show()
                }
                else {
                    Log.e("Error",response.body())
                    val arr = response.body<List<String>>()
                    for(x in arr)
                        Toast.makeText(context, x, Toast.LENGTH_SHORT).show()
                }
            }
        }

    }


    override fun onDateSet(view: DatePicker?, year: Int, month: Int, dayOfMonth: Int) {
        calendar.set(year, month, dayOfMonth)
        displayFormattedDate(calendar.timeInMillis)
    }


    private fun displayFormattedDate(timestamp: Long){
        layout.findViewById<EditText>(R.id.datePrompt).setText(formatter.format(timestamp))
    }
}