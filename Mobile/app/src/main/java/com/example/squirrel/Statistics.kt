package com.example.squirrel


import android.app.DatePickerDialog
import android.graphics.Color
import android.os.Bundle
import android.view.View
import android.widget.DatePicker
import android.widget.RadioGroup
import android.widget.TextView
import androidx.core.content.ContextCompat.getColor
import androidx.fragment.app.Fragment
import com.example.squirrel.overrides.MyValueFormatter
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.data.PieData
import com.github.mikephil.charting.data.PieDataSet
import com.github.mikephil.charting.data.PieEntry
import java.text.SimpleDateFormat
import java.util.*


class Statistics: Fragment(R.layout.fragment_statistics), DatePickerDialog.OnDateSetListener {
    private lateinit var layout: View
    private lateinit var spendingsIncomeRatioChart: PieChart
    private val calendar = Calendar.getInstance()
    private val formatter = SimpleDateFormat("MMMM", Locale.US)

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view



        layout.findViewById<TextView>(R.id.calendar_button).setOnClickListener() {
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

        parentFragmentManager.beginTransaction().replace(R.id.nastedFragmetsLayout, StatisticIncome()).commit()
        layout.findViewById<RadioGroup>(R.id.radioGroup).setOnCheckedChangeListener { _, checkedId ->
            when (checkedId) {
                R.id.buttonSpendings -> parentFragmentManager.beginTransaction().replace(R.id.nastedFragmetsLayout, StatisticSpendings()).commit()
                R.id.buttonIncome -> parentFragmentManager.beginTransaction().replace(R.id.nastedFragmetsLayout, StatisticIncome()).commit()
            }
        }

        spendingsIncomeRatioChart = layout.findViewById<PieChart>(R.id.income_spending_chart)
        chartStyle(spendingsIncomeRatioChart)
        setData(1,100,spendingsIncomeRatioChart)

    }


    //var stringParamsOfChart: Array<String> = arrayOf("Spendings","Income")
    fun setData(count: Int,range: Int,mChart: PieChart){
        val values: ArrayList<PieEntry> = ArrayList()

        values.add(PieEntry(9600f,"Spendings"))
        values.add(PieEntry(3200f,"Income"))

        var dataSet: PieDataSet = PieDataSet(values,"")
        val colorsOfChartRGB: ArrayList<Int> = arrayListOf(255 shl 24 or "E53F3F".toInt(16), 255 shl 24 or "18C715".toInt(16))
        dataSet.setColors(colorsOfChartRGB)
        dataSet.setDrawValues(true)
        dataSet.selectionShift = 0f
        var data: PieData = PieData(dataSet)
        data.setValueFormatter(MyValueFormatter())
        data.setValueTextSize(12.5f)
        data.setValueTextColor(Color.WHITE)
        mChart.data = data
        mChart.invalidate()
    }

    fun chartStyle(mChart: PieChart){
        mChart.setDrawSliceText(false);
        mChart.setBackgroundColor(getColor(requireContext(), R.color.main_background))
        mChart.setHoleColor(Color.TRANSPARENT)
        mChart.setCenterTextOffset(0f,25f)
        mChart.setCenterTextSize(20f)
        mChart.centerText = formatter.format(Date())
        mChart.isRotationEnabled = false
        mChart.description.isEnabled = false
        mChart.setUsePercentValues(false)
        mChart.getLegend().setEnabled(false)
        mChart.maxAngle = 180.toFloat()
        mChart.rotationAngle = 360.toFloat()
    }

    override fun onDateSet(view: DatePicker?, year: Int, month: Int, dayOfMonth: Int) {
        calendar.set(year, month, dayOfMonth)
        displayFormattedDate(calendar.timeInMillis)
    }


    private fun displayFormattedDate(timestamp: Long){
        spendingsIncomeRatioChart.centerText = formatter.format(timestamp).toString()
        spendingsIncomeRatioChart.invalidate()
    }
}