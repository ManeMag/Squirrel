package com.example.squirrel


import android.app.DatePickerDialog
import android.graphics.Color
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.*
import androidx.core.content.ContextCompat.getColor
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.squirrel.Program.Companion.client
import com.example.squirrel.Program.Companion.domain
import com.example.squirrel.Program.Companion.port
import com.example.squirrel.Program.Companion.protocol
import com.example.squirrel.overrides.MyValueFormatter
import com.github.mikephil.charting.charts.PieChart
import com.github.mikephil.charting.data.PieData
import com.github.mikephil.charting.data.PieDataSet
import com.github.mikephil.charting.data.PieEntry
import io.ktor.client.call.*
import io.ktor.client.request.*
import io.ktor.client.statement.*
import io.ktor.http.*
import kotlinx.coroutines.launch
import kotlinx.datetime.LocalDate
import java.text.SimpleDateFormat
import java.util.*
import kotlin.collections.ArrayList


class Statistics : Fragment(R.layout.fragment_statistics), DatePickerDialog.OnDateSetListener {
    private lateinit var layout: View
    private lateinit var spendingsIncomeRatioChart: PieChart
    private val calendar = Calendar.getInstance()
    private val formatter = SimpleDateFormat("MMMM", Locale.US)

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        this.layout = view

        layout.findViewById<TextView>(R.id.calendar_button).setOnClickListener() {
            DatePickerDialog(
                requireContext(),
                this,
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)
            ).show()
        }

        parentFragmentManager.beginTransaction()
            .replace(R.id.nastedFragmetsLayout, StatisticIncome()).commit()
        layout.findViewById<RadioGroup>(R.id.radioGroup)
            .setOnCheckedChangeListener { _, checkedId ->
                when (checkedId) {
                    R.id.buttonSpendings -> parentFragmentManager.beginTransaction()
                        .replace(R.id.nastedFragmetsLayout, StatisticSpendings()).commit()
                    R.id.buttonIncome -> parentFragmentManager.beginTransaction()
                        .replace(R.id.nastedFragmetsLayout, StatisticIncome()).commit()
                }
            }
        val start = LocalDate(
            calendar.get(Calendar.YEAR),
            calendar.get(Calendar.MONTH) + 1,
            1
        )
        val end = LocalDate(
            calendar.get(Calendar.YEAR),
            calendar.get(Calendar.MONTH) + 1,
            calendar.getActualMaximum(Calendar.DAY_OF_MONTH)
        )
        spendingsIncomeRatioChart = layout.findViewById(R.id.income_spending_chart)
        lifecycleScope.launch {
            val response = client.get("$protocol://$domain:$port/api/Statistics") {
                url {
                    parameters.append("startDate", start.toString())
                    parameters.append("endDate", end.toString())
                }
            }
            Log.e("Status", response.status.toString())
            Log.e("Body", response.body())
            if (response.status == HttpStatusCode.OK) {
                val statistics: com.example.squirrel.models.Statistics = response.body()
                val values: ArrayList<PieEntry> = ArrayList()
                values.add(PieEntry(statistics.outcome.toFloat(), "Spendings"))
                values.add(PieEntry(statistics.income.toFloat(), "Income"))
                chartStyle(spendingsIncomeRatioChart)
                setData(spendingsIncomeRatioChart, values)
            }
        }
    }

    private fun setData(mChart: PieChart, values: ArrayList<PieEntry>) {
        val colorsOfChartRGB: ArrayList<Int> =
            arrayListOf(255 shl 24 or "E53F3F".toInt(16), 255 shl 24 or "18C715".toInt(16))
        with(PieDataSet(values, "")) {
            setColors(colorsOfChartRGB)
            setDrawValues(true)
            selectionShift = 0f
            with(PieData(this)) {
                setValueFormatter(MyValueFormatter())
                setValueTextSize(12.5f)
                setValueTextColor(Color.WHITE)
                mChart.data = this
            }
        }
        mChart.invalidate()
    }

    fun chartStyle(mChart: PieChart) {
        mChart.setDrawSliceText(false);
        mChart.setBackgroundColor(getColor(requireContext(), R.color.main_background))
        mChart.setHoleColor(Color.TRANSPARENT)
        mChart.setCenterTextOffset(0f, 25f)
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


    private fun displayFormattedDate(timestamp: Long) {
        spendingsIncomeRatioChart.centerText = formatter.format(timestamp).toString()
        spendingsIncomeRatioChart.invalidate()
    }
}