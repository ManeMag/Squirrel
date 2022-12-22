package com.example.squirrel.overrides

import com.github.mikephil.charting.data.Entry
import com.github.mikephil.charting.formatter.IValueFormatter
import com.github.mikephil.charting.utils.ViewPortHandler
import java.text.DecimalFormat


public class MyValueFormatter : IValueFormatter {
    private val mFormat: DecimalFormat

    init {
        mFormat = DecimalFormat("########0") // use one decimal
    }

    override fun getFormattedValue(
        value: Float,
        entry: Entry?,
        dataSetIndex: Int,
        viewPortHandler: ViewPortHandler?
    ): String {
        return mFormat.format(value) + " $" // e.g. append a dollar-sign
    }
}