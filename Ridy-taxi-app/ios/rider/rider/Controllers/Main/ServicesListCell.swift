//
//  ServicesListCell.swift
//
//  Copyright © 2018 Minimalistic Apps. All rights reserved.
//

import UIKit
import Kingfisher

class ServicesListCell: UICollectionViewCell {
    @IBOutlet weak var imageIcon: UIImageView!
    @IBOutlet weak var textTitle: UILabel!
    @IBOutlet weak var textCost: UILabel!
    @IBOutlet weak var buttonMinus: UIButton!
    @IBOutlet weak var buttonPlus: UIButton!
    private var service: Service!
    private var quantity: Int = 0
    private var distance: Double = 0
    private var duration: Double = 0
    private var currency: String = ""
    
    override var isSelected: Bool {
        didSet {
            self.contentView.alpha = isSelected ? 1 : 0.5
        }
    }
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.contentView.alpha = 0.5
    }
    
    func initialize(service:Service, distance: Int, duration: Int, currency: String) {
        self.service = service
        self.distance = Double(distance)
        self.duration = Double(duration)
        self.currency = currency
        self.updatePrice()
        textTitle.text = service.title
        if let media = service.media, let address = media.address {
            let url = URL(string: Config.Backend + address)
            imageIcon.kf.setImage(with: url)
        }
        buttonPlus.isHidden = true
        buttonMinus.isHidden = true
    }
    
    func updatePrice() {
        let formatter = NumberFormatter()
        formatter.locale = Locale.current
        formatter.numberStyle = .currency
        formatter.currencyCode = currency
        let cost: Double = service.cost!
        switch service.feeEstimationMode {
        case .Disabled:
            textCost.text = "-"
        
        case .Static:
            textCost.text = formatter.string(from: NSNumber(value: cost))
            
        case .Dynamic:
            textCost.text = "~\(formatter.string(from: NSNumber(value: cost)) ?? "Unkown Curr")"
            
        case .Ranged:
            let cMinus = cost - (cost * Double(service.rangeMinusPercent / 100))
            let cPlus = cost + (cost * Double(service.rangePlusPercent / 100))
            textCost.text = "\(formatter.string(from: NSNumber(value: cMinus)) ?? "Unkown Curr")~\(formatter.string(from: NSNumber(value: cPlus)) ?? "Unkown Curr")"
            
        case .RangedStrict:
            let cMinus = cost - (cost * Double(service.rangeMinusPercent / 100))
            let cPlus = cost + (cost * Double(service.rangePlusPercent / 100))
            textCost.text = "\(formatter.string(from: NSNumber(value: cMinus)) ?? "Unkown Curr")-\(formatter.string(from: NSNumber(value: cPlus)) ?? "Unkown Curr")"
        }
        if service.quantityMode == .Multiple && quantity > 0 {
            textCost.text = "\(textCost.text!) (\(quantity)x)"
        }
    }
    
    @IBAction func onButtonMinusTouched(_ sender: UIButton) {
        if quantity > 0 {
            quantity -= 1
        }
        updatePrice()
    }
    
    @IBAction func onButtonPlusTouched(_ sender: UIButton) {
        if quantity < service.maxQuantity {
            quantity += 1
            updatePrice()
        }
    }
}
