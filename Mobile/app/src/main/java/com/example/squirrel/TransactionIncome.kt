package com.example.squirrel

import android.app.DatePickerDialog
import android.os.Bundle
import android.renderscript.ScriptGroup.Binding
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.DatePicker
import android.widget.EditText
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.findViewTreeViewModelStoreOwner
import androidx.navigation.fragment.findNavController
import com.example.squirrel.databinding.ActivityMainBinding
import java.text.SimpleDateFormat
import java.util.*

class TransactionIncome:Fragment(R.layout.fragment_transaction), DatePickerDialog.OnDateSetListener {

    private var _binding:ActivityMainBinding? = null;
    private val binding get() = _binding!!
    private val calendar = Calendar.getInstance()
    private val formatter = SimpleDateFormat("MMM. dd, yyyy ", Locale.US)


    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = ActivityMainBinding.inflate(inflater, container, false)
        val view = binding.root
        return view
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        binding.activityMainLayout.findViewById<TextView>(R.id.buttonSpendings).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transaction_to_transactionSpendings)
        }
        binding.root.findViewById<TextView>(R.id.buttonIncome).setOnClickListener {
            findNavController().navigate(R.id.action_nav_fragment_transactionSpendings_to_nav_fragment_transaction)
        }

        binding.root.findViewById<EditText>(R.id.datePrompt).setText("sdasdasdasdasdas")
        binding.root.findViewById<TextView>(R.id.datePickButton).setOnClickListener() {
            DatePickerDialog(
                requireContext(),
                this,
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)
            ).show()
        }

    }

    override fun onDateSet(view: DatePicker?, year: Int, month: Int, dayOfMonth: Int) {
        Log.e("Calendar","$year -- $month -- $dayOfMonth")
        calendar.set(year, month, dayOfMonth)
        displayFormattedDate(calendar.timeInMillis)
    }

    private fun displayFormattedDate(timestamp: Long){
        binding.root.findViewById<EditText>(R.id.datePrompt).setText(formatter.format(timestamp))
        Log.i("Formatting", timestamp.toString())

    }
}