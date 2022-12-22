package com.example.squirrel

import android.graphics.drawable.GradientDrawable
import android.os.Bundle
import android.view.View
import android.widget.*
import androidx.core.content.ContextCompat
import androidx.core.content.ContextCompat.getDrawable
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.squirrel.entities.Category
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.data.PieData
import com.github.mikephil.charting.data.PieDataSet
import com.github.mikephil.charting.data.PieEntry
import io.ktor.client.call.*
import io.ktor.client.request.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.launch


class StatisticIncome: Fragment(R.layout.fragment_statistic_income) {
    private lateinit var layout: View
    private lateinit var IncomeChart: PieChart
    var listCategory = emptyList<Category>()

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view


        IncomeChart = layout.findViewById<PieChart>(R.id.income_chart)
        chartStyle(IncomeChart)
        setData(1,100,IncomeChart)


        lifecycleScope.launch {
            val response: HttpResponse =
                Program.client.get("${Program.protocol}://${Program.domain}:${Program.port}/api/category/1")
            if (response.status == HttpStatusCode.OK) {
                listCategory = response.body()
            } else {
                Toast.makeText(context, "Something went wrong", Toast.LENGTH_LONG).show()
            }

            val rows = listCategory.size
            val columns = 2
            val tableLayout = layout.findViewById<TableLayout>(R.id.category_layout)
            for (i in 0 until rows) {
                val tableRow = TableRow(layout.context)
                tableRow.setLayoutParams(
                    TableRow.LayoutParams(
                        TableRow.LayoutParams.MATCH_PARENT,
                        TableRow.LayoutParams.WRAP_CONTENT
                    )
                )

                val imageView = ImageView(layout.context)
                var drawable = getDrawable(requireContext(), R.drawable.category_color_icon)
                var gradientDrawable = drawable as GradientDrawable
                gradientDrawable.setColor(255 shl 24 or  listCategory[i].color.substring(1).toInt(16))
                imageView.setImageDrawable(gradientDrawable)
                imageView.setPadding(70,15,20,0)

                var categoryName = TextView(layout.context)
                categoryName.textSize = 20f
                categoryName.setText(listCategory[i].name)

                tableRow.addView(imageView, 0)
                tableRow.addView(categoryName, 1)
                tableLayout.addView(tableRow, i)
            }
        }
    }
    fun setData(count: Int,range: Int,mChart: PieChart){
        val values: ArrayList<PieEntry> = ArrayList()

        values.add(PieEntry(9600f,"Spendings"))
        values.add(PieEntry(3200f,"Income"))

        var dataSet: PieDataSet = PieDataSet(values,"")
        val colorsOfChartRGB: ArrayList<Int> = arrayListOf(255 shl 24 or "E53F3F".toInt(16), 255 shl 24 or "18C715".toInt(16))
        dataSet.setColors(colorsOfChartRGB)
        dataSet.setDrawValues(false)
        dataSet.sliceSpace = 2f
        dataSet.selectionShift = 0f
        dataSet.selectionShift = 4f
        var data: PieData = PieData(dataSet)
        mChart.data = data
        mChart.invalidate()
    }

    fun chartStyle(mChart: PieChart){
        mChart.setDrawSliceText(false);
        mChart.setBackgroundColor(ContextCompat.getColor(requireContext(), R.color.main_background))
        mChart.isDrawHoleEnabled = false
        mChart.isRotationEnabled = false
        mChart.description.isEnabled = false
        mChart.setUsePercentValues(false)
        mChart.getLegend().setEnabled(false)
    }
}