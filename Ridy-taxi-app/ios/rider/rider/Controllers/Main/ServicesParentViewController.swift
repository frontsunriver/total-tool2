//
//  ServicesViewController.swift
//
//  Copyright © 2018 Minimalistic Apps. All rights reserved.
//

import UIKit
import SPAlert

class ServicesParentViewController: UIViewController, UICollectionViewDataSource, UICollectionViewDelegate {
    public var calculateFareResult: CalculateFareResult!
    public var callback: ServiceRequested?
    private var selectedCategory: ServiceCategory?
    private var selectedService: Service?
    
    @IBOutlet weak var buttonRideNow: ColoredButton!
    @IBOutlet weak var buttonRideLater: ColoredButton!
    @IBOutlet weak var tabBar: UISegmentedControl!
    @IBOutlet weak var collectionServices: UICollectionView!
    @IBOutlet weak var viewBlur: UIView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tabBar.addTarget(self, action: #selector(tabChanged), for: .valueChanged)
        self.collectionServices.dataSource = self
        self.collectionServices.delegate = self
        let blurEffect = UIBlurEffect(style: .prominent)
        let blurEffectView = UIVisualEffectView(effect: blurEffect)
        blurEffectView.frame = view.bounds
        blurEffectView.autoresizingMask = [.flexibleWidth, .flexibleHeight]
        viewBlur.addSubview(blurEffectView)
        let maskLayer = CAShapeLayer()
        maskLayer.path = UIBezierPath(roundedRect: view.bounds, byRoundingCorners: [.topLeft, .topRight], cornerRadii: CGSize(width: 10, height: 10)).cgPath
        self.view.layer.mask = maskLayer
    }
    
    public func reload() {
        let segments = calculateFareResult.categories.map() { x in return x.title }
        self.tabBar.removeAllSegments()
        for (index, value) in segments.enumerated() {
            self.tabBar.insertSegment(withTitle: value, at: index, animated: false)
        }
        self.tabBar.selectedSegmentIndex = 0
        self.tabChanged(sender: self.tabBar)
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        guard let cat = self.selectedCategory else {
            return 0
        }
        return cat.services.count
    }
    
    @objc func tabChanged(sender: UISegmentedControl) {
        self.selectedCategory = self.calculateFareResult.categories[sender.selectedSegmentIndex]
        
        self.collectionServices.reloadData()
        self.collectionServices.performBatchUpdates({ [weak self] in
            self?.collectionServices.reloadSections(IndexSet(integer: 0))
            }) { completed -> Void in
            for indexPath in self.collectionServices.indexPathsForSelectedItems ?? [] {
                self.collectionServices.deselectItem(at: indexPath, animated: false)
            }
            if self.collectionServices.indexPathsForVisibleItems.count == 1 {
                self.collectionServices.selectItem(at: self.collectionServices.indexPath(for: self.collectionServices.visibleCells.first!), animated: false, scrollPosition: [])
                self.selectedService = self.selectedCategory!.services.first
                self.buttonRideNow.isEnabled = true
                self.buttonRideLater.isEnabled = true
                if self.selectedService!.bookingMode == .OnlyNow {
                    self.buttonRideLater.isHidden = true
                }
                self.buttonRideNow.setTitle("Request \(self.selectedService!.title ?? "") Now", for: .normal)
            }
        }
        
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "serviceCell", for: indexPath) as! ServicesListCell
        cell.initialize(service: (selectedCategory?.services[indexPath.row])!, distance: self.calculateFareResult.distance, duration: self.calculateFareResult.duration, currency: self.calculateFareResult.currency)
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let feedbackGenerator = UISelectionFeedbackGenerator()
        feedbackGenerator.selectionChanged()
        selectedService = (selectedCategory?.services[indexPath.row])!
        buttonRideNow.isEnabled = true
        buttonRideLater.isEnabled = true
        if self.selectedService!.bookingMode == .OnlyNow {
            self.buttonRideLater.isHidden = true
        }
        buttonRideNow.setTitle("Request \(selectedService!.title ?? "") Now", for: .normal)
    }
    
    @IBAction func onSelectServiceClicked(_ sender: UIButton) {
        if let d = callback {
            d.RideNowSelected(service: selectedService!)
        }
    }
    
    @IBAction func onBookLaterClicked(_ sender: ColoredButton) {
        DatePickerDialog().show(NSLocalizedString("Select Time", comment: "Select Time dialog title"), doneButtonTitle: "Done", cancelButtonTitle: "Cancel", datePickerMode: selectedService?.bookingMode == .Time ? .time : .dateAndTime) {
            (date) -> Void in
            if(self.selectedService?.bookingMode == .DateTimeAbosoluteHour && Calendar.current.component(.minute, from: date!) != 0) {
                SPAlert.present(title: NSLocalizedString("Only absolute hours are accepted for this service", comment: ""), preset: .exclamation)
                return
            }
            if let dt = date, let d = self.callback {
                
                let seconds = dt.timeIntervalSince(Date())
                if seconds < 30 {
                    let message = UIAlertController(title: "Problem", message: "Time selected seems to be for past. Select a future time or now!", preferredStyle: .alert)
                    message.addAction(UIAlertAction(title: "OK", style: .default, handler: nil))
                    self.parent!.present(message, animated: true, completion: nil)
                    return
                }
                d.RideLaterSelected(service: self.selectedService!, minutesFromNow: Int(seconds / 60))
                
            }
        }
    }
}

protocol ServiceRequested {
    func RideNowSelected(service: Service)
    func RideLaterSelected(service: Service, minutesFromNow: Int)
}
