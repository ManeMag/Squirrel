package com.example.squirrel

import android.annotation.SuppressLint
import android.graphics.Color
import android.graphics.drawable.GradientDrawable
import android.os.Bundle
import android.util.Log
import android.view.Gravity
import android.view.View
import android.widget.*
import androidx.core.content.ContextCompat
import androidx.core.view.setPadding
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.squirrel.Program.Companion.client
import com.example.squirrel.Program.Companion.domain
import com.example.squirrel.Program.Companion.port
import com.example.squirrel.Program.Companion.protocol
import com.example.squirrel.entities.Category
import com.example.squirrel.entities.Transaction
import com.example.squirrel.models.Impact
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.data.PieData
import com.github.mikephil.charting.data.PieDataSet
import com.github.mikephil.charting.data.PieEntry
import io.ktor.client.call.*
import io.ktor.client.request.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.launch

class StatisticSpendings(
    private val transactions: List<Transaction>,
    private val impact: List<Impact>
) : Fragment(R.layout.fragment_statistic_spendings) {
    private lateinit var layout: View
    private lateinit var IncomeChart: PieChart
    var listCategory = emptyList<Category>()
    var viewList = mutableListOf<Transaction>()
    var colors = mutableMapOf<Int, Int>()

    @SuppressLint("SetTextI18n")
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view


        lifecycleScope.launch {
            val response: HttpResponse =
                client.get("$protocol://$domain:$port/api/category/2")
            if (response.status == HttpStatusCode.OK) {
                listCategory = response.body()
            } else {
                Toast.makeText(context, "Something went wrong", Toast.LENGTH_LONG).show()
            }

            val tableLayoutCategory = layout.findViewById<RadioGroup>(R.id.category_layout)
            listCategory.indices.forEach {
                colors.put(
                    listCategory[it].id,
                    255 shl 24 or listCategory[it].color.substring(1).toInt(16)
                )
                with(RadioButton(layout.context)) {
                    layoutParams = TableRow.LayoutParams(
                        TableRow.LayoutParams.MATCH_PARENT,
                        TableRow.LayoutParams.WRAP_CONTENT
                    )
                    val gradientDrawable = ContextCompat.getDrawable(
                        requireContext(),
                        R.drawable.category_color_icon
                    ) as GradientDrawable
                    gradientDrawable.setColor(
                        255 shl 24 or listCategory[it].color.substring(1).toInt(16)
                    )
                    setPadding(15)
                    background =
                        ContextCompat.getDrawable(context, R.drawable.radio_button_category)
                    buttonDrawable = gradientDrawable
                    gravity = Gravity.CENTER
                    textSize = 20f
                    id = listCategory[it].id
                    text = listCategory[it].name
                    tableLayoutCategory.addView(this, it)
                }
            }
            tableLayoutCategory.setOnCheckedChangeListener { buttonView, id ->
                with(viewList) {
                    clear()
                    transactions.forEach {
                        Log.e("ID", id.toString())
                        Log.e("itID", it.id.toString())
                        Log.e("amount", it.amount.toString())
                        if (it.categoryId == id) {
                            add(it)
                        }
                    }
                }
                with(layout.findViewById<TableLayout>(R.id.transaction_layout)) {
                    removeAllViews()
                    viewList.forEach {
                        TableRow(layout.context).apply {
                            layoutParams = TableRow.LayoutParams(
                                TableRow.LayoutParams.MATCH_PARENT,
                                TableRow.LayoutParams.WRAP_CONTENT
                            )
                            with(TextView(layout.context)) {
                                textSize = 20f
                                text = "${it.amount}$"
                                setTextColor(Color.RED)
                                this@apply.addView(this, 0)
                            }
                            with(TextView(layout.context)) {
                                textSize = 20f
                                text = it.description
                                setPadding(25, 15, 0, 0)
                                this@apply.addView(this, 1)
                            }
                            this@with.addView(this@apply)
                        }
                    }
                }
            }


            IncomeChart = layout.findViewById(R.id.income_chart)
            chartStyle(IncomeChart)
            setData(IncomeChart)
        }
    }

    private fun setData(mChart: PieChart) {
        val values: ArrayList<PieEntry> = ArrayList()

        val colorsOfChartRGB = mutableListOf<Int>()
        with(values) {
            impact.forEach {
                Log.e("id", it.negativePercentage.toString())
                if(listCategory.any{category -> category.id == it.categoryId } && it.negativePercentage > 0){
                    add(PieEntry(it.negativePercentage.toFloat()))
                    colorsOfChartRGB.add(colors.get(it.categoryId)!!)
                }
            }
        }
        with(PieDataSet(values, "")) {
            colors = colorsOfChartRGB
            this.setDrawValues(false)
            sliceSpace = 2f
            selectionShift = 0f
            selectionShift = 4f
            mChart.data = PieData(this)
            mChart.invalidate()
        }
    }

    private fun chartStyle(mChart: PieChart) {
        mChart.setDrawSliceText(false);
        mChart.setBackgroundColor(
            ContextCompat.getColor(
                requireContext(),
                R.color.main_background
            )
        )
        mChart.isDrawHoleEnabled = false
        mChart.isRotationEnabled = false
        mChart.description.isEnabled = false
        mChart.setUsePercentValues(true)
        mChart.legend.isEnabled = false
    }
}