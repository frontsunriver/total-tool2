package com.innomalist.taxi.driver.activities.statistics.charts

import android.animation.PropertyValuesHolder
import android.content.Context
import android.graphics.Color
import android.view.View
import android.view.animation.BounceInterpolator
import com.db.williamchart.view.LineChartView
import com.innomalist.taxi.driver.R

class IncomeLineChart(private val mChart: LineChartView, private val mContext: Context) : ChartBase() {
    //private var mTip: Tooltip? = null
    private var mBaseAction: Runnable? = null
    public override fun show(action: Runnable?) { // Tooltip
        /*mTip = Tooltip(mContext, R.layout.tooltip_income, R.id.value)
        mTip!!.setVerticalAlignment(Tooltip.Alignment.BOTTOM_TOP)
        mTip!!.setDimensions(Tools.fromDpToPx(58f).toInt(), Tools.fromDpToPx(25f).toInt())
        mTip!!.setEnterAnimation(PropertyValuesHolder.ofFloat(View.ALPHA, 1f),
                PropertyValuesHolder.ofFloat(View.SCALE_Y, 1f),
                PropertyValuesHolder.ofFloat(View.SCALE_X, 1f)).duration = 200
        mTip!!.setExitAnimation(PropertyValuesHolder.ofFloat(View.ALPHA, 0f),
                PropertyValuesHolder.ofFloat(View.SCALE_Y, 0f),
                PropertyValuesHolder.ofFloat(View.SCALE_X, 0f)).duration = 200
        mTip!!.pivotX = Tools.fromDpToPx(65f) / 2
        mTip!!.pivotY = Tools.fromDpToPx(25f)
        mChart.setTooltips(mTip)
        val dataSet = LineSet(mLabels, mValues)
        dataSet.setColor(Color.parseColor("#b3b5bb"))
                .setFill(Color.parseColor("#2d374c"))
                .setDotsColor(Color.parseColor("#ffc755")).thickness = 4f
        mChart.addData(dataSet)
        // Chart
        mChart.setBorderSpacing(Tools.fromDpToPx(15f).toInt()) //.setAxisBorderValues(0, 20)
                .setYLabels(AxisRenderer.LabelPosition.NONE)
                .setLabelsColor(Color.parseColor("#6a84c3"))
                .setXAxis(false)
                .setYAxis(false)
        mBaseAction = action
        val chartAction = Runnable {
            mBaseAction!!.run()
            //mTip.prepare(mChart.getEntriesArea(0).get(3), mValues[3]);
//mChart.showTooltip(mTip, true);
        }*/
        /*LineSet dataSet = new LineSet(mLabels, mValues[0]);
        dataSet.setColor(Color.parseColor("#53c1bd"))
                .setFill(Color.parseColor("#3d6c73"))
                .setGradientFill(new int[] {Color.parseColor("#364d5a"), Color.parseColor("#3f7178")},
                        null);
        mChart.addData(dataSet);

        mChart.setBorderSpacing(1)
                .setXLabels(AxisRenderer.LabelPosition.NONE)
                .setYLabels(AxisRenderer.LabelPosition.NONE)
                .setXAxis(false)
                .setYAxis(false)
                .setBorderSpacing(Tools.fromDpToPx(5));*/
        //val anim = Animation().setInterpolator(BounceInterpolator()).withEndAction(chartAction)
        //mChart.show(anim)
    }

    public override fun update(labels: Array<String?>, values: FloatArray) {
        super.update(labels, values)
        /*mChart.dismissAllTooltips()
        mChart.updateValues(0, mValues)
        //mChart.getChartAnimation().setEndAction(mBaseAction);
        mChart.notifyDataUpdate()*/
    }

    public override fun dismiss(action: Runnable?) {
        super.dismiss(action)
        /*mChart.dismissAllTooltips()
        mChart.dismiss(Animation().setInterpolator(BounceInterpolator()).withEndAction(action))*/
    }

}