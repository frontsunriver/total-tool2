//
//  EarningViewController.swift
//  driver
//
//  Created by Manly Man on 1/19/20.
//  Copyright © 2020 minimal. All rights reserved.
//

import UIKit
import Charts
import MapKit

class EarningViewController: UIViewController {
    @IBOutlet weak var stackParent: UIStackView!
    @IBOutlet weak var scrollView: UIScrollView!
    @IBOutlet weak var earningChart: BarChartView!
    @IBOutlet weak var distanceChart: BarChartView!
    @IBOutlet weak var timeChart: BarChartView!
    @IBOutlet weak var textEarning: UILabel!
    @IBOutlet weak var textDistance: UILabel!
    @IBOutlet weak var textTime: UILabel!
    @IBOutlet weak var countChart: BarChartView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        refresh(queryType: .Daily)
    }
    
    @IBAction func onQueryTypeChanged(_ sender: UISegmentedControl) {
        switch sender.selectedSegmentIndex {
        case 0:
            refresh(queryType: .Daily)
        case 1:
            refresh(queryType: .Weekly)
        case 2:
            refresh(queryType: .Monthly)
        default:
            break
        }
    }
    
    func refresh(queryType: QueryType) {
        GetStats(query: queryType).execute() { result in
            switch result {
            case .success(let response):
                self.setEarningChart(dataPoints: response.dataset)
                self.setDistanceChart(dataPoints: response.dataset)
                self.setTimeChart(dataPoints: response.dataset)
                self.setCountChart(dataPoints: response.dataset)
                if response.dataset.count == 0 || !response.dataset.contains(where: { return $0.name == $0.current }) {
                    self.textEarning.text = "-"
                    self.textTime.text = "-"
                    self.textDistance.text = "-"
                    return
                }
                let current = response.dataset.first() { return $0.name == $0.current }!
                let formatter = DateComponentsFormatter()
                formatter.allowedUnits = [.minute]
                formatter.unitsStyle = .short
                self.textTime.text = formatter.string(from: Double(current.time)!)
                self.textEarning.text = MyLocale.formattedCurrency(amount: current.earning, currency: response.currency)
                let distanceFormatter = MKDistanceFormatter()
                distanceFormatter.unitStyle = .abbreviated
                self.textDistance.text = distanceFormatter.string(fromDistance: Double(current.distance)!)
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    func setEarningChart(dataPoints: [DataPoint]) {
        styleChart(chart: earningChart)
        var dataEntries: [BarChartDataEntry] = []
        for i in 0..<dataPoints.count {
            let dataEntry = BarChartDataEntry(x: Double(i), yValues: [dataPoints[i].earning.rounded()])
            dataEntries.append(dataEntry)
        }
        earningChart.xAxis.valueFormatter = getFormatter(dataPoints: dataPoints)
        let chartDataSet = BarChartDataSet(entries: dataEntries, label: NSLocalizedString("Earnings", comment: "Earning screen's earning title"))
        chartDataSet.colors = ChartColorTemplates.pastel()
        earningChart.data = BarChartData(dataSet: chartDataSet)
    }
    
    func setDistanceChart(dataPoints: [DataPoint]) {
        styleChart(chart: distanceChart)
        var dataEntries: [BarChartDataEntry] = []
        for i in 0..<dataPoints.count {
            let dataEntry = BarChartDataEntry(x: Double(i), yValues: [Double(dataPoints[i].distance)!])
            dataEntries.append(dataEntry)
        }
        distanceChart.xAxis.valueFormatter = getFormatter(dataPoints: dataPoints)
        let chartDataSet = BarChartDataSet(entries: dataEntries, label: NSLocalizedString("Distance", comment: "Earning screen's distance driven title"))
        chartDataSet.colors = ChartColorTemplates.pastel()
        distanceChart.data = BarChartData(dataSet: chartDataSet)
    }
    
    func setTimeChart(dataPoints: [DataPoint]) {
        styleChart(chart: timeChart)
        var dataEntries: [BarChartDataEntry] = []
        for i in 0..<dataPoints.count {
            let dataEntry = BarChartDataEntry(x: Double(i), yValues: [Double(dataPoints[i].time)!])
            dataEntries.append(dataEntry)
        }
        timeChart.xAxis.valueFormatter = getFormatter(dataPoints: dataPoints)
        let chartDataSet = BarChartDataSet(entries: dataEntries, label: NSLocalizedString("Time", comment: "Earning screen's time driven title"))
        chartDataSet.colors = ChartColorTemplates.pastel()
        timeChart.data = BarChartData(dataSet: chartDataSet)
    }
    
    func setCountChart(dataPoints: [DataPoint]) {
        styleChart(chart: countChart)
        var dataEntries: [BarChartDataEntry] = []
        for i in 0..<dataPoints.count {
            let dataEntry = BarChartDataEntry(x: Double(i), yValues: [Double(dataPoints[i].count)!])
            dataEntries.append(dataEntry)
        }
        countChart.xAxis.valueFormatter = getFormatter(dataPoints: dataPoints)
        let chartDataSet = BarChartDataSet(entries: dataEntries, label: NSLocalizedString("Services", comment: "Earning screen's service counts title"))
        chartDataSet.colors = ChartColorTemplates.pastel()
        countChart.data = BarChartData(dataSet: chartDataSet)
    }
    
    func styleChart(chart: BarChartView) {
        chart.noDataText = NSLocalizedString("No Data to show.", comment: "Earning charts empty state.")
        chart.animate(xAxisDuration: 0.5, yAxisDuration: 0.5)
        chart.xAxis.labelPosition = .bottom
        chart.drawValueAboveBarEnabled = false
        chart.rightAxis.enabled = false
        chart.leftAxis.enabled = false
        chart.legend.enabled = false
        if #available(iOS 13.0, *) {
            chart.noDataTextColor = UIColor.label
            chart.xAxis.gridColor = UIColor.clear
            chart.xAxis.axisLineColor = UIColor.label
            chart.xAxis.labelTextColor = UIColor.label
        }
    }
    
    func getFormatter(dataPoints: [DataPoint]) -> IndexAxisValueFormatter {
        return IndexAxisValueFormatter(values:dataPoints.map() { $0.name })
    }

}
