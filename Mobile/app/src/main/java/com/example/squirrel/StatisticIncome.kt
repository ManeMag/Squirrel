package com.example.squirrel

import android.os.Bundle
import android.view.View
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.data.PieData
import com.github.mikephil.charting.data.PieDataSet
import com.github.mikephil.charting.data.PieEntry

import kotlin.collections.ArrayList

class StatisticIncome: Fragment(R.layout.fragment_statistic_income) {
    private lateinit var layout: View
    private lateinit var IncomeChart: PieChart

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view


        IncomeChart = layout.findViewById<PieChart>(R.id.income_chart)
        chartStyle(IncomeChart)
        setData(1,100,IncomeChart)
    }


    //var stringParamsOfChart: Array<String> = arrayOf("Spendings","Income")
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