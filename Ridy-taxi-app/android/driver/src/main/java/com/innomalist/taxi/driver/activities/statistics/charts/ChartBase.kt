package com.innomalist.taxi.driver.activities.statistics.charts

import android.os.Handler

open class ChartBase internal constructor() {
    var mLabels = arrayOfNulls<String>(0)
    var mValues = FloatArray(0)
    private val unlockAction = Runnable { Handler().postDelayed({ unlock() }, 500) }
    private val showAction = Runnable { Handler().postDelayed({ show(unlockAction) }, 500) }
    fun init(labels: Array<String?>, values: FloatArray) {
        mLabels = labels
        mValues = values
        show(unlockAction)
    }

    protected open fun show(action: Runnable?) {
        lock()
    }

    protected open fun update(labels: Array<String?>, values: FloatArray) {
        mLabels = labels
        mValues = values
        lock()
    }

    protected open fun dismiss(action: Runnable?) {
        lock()
    }

    private fun lock() {}
    private fun unlock() {}
}