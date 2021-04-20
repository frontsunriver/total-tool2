//
//  RequestCard.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit


class RequestCard: UIView {
    @IBOutlet weak var labelPickupLocation: UILabel!
    @IBOutlet weak var labelFromYou: UILabel!
    @IBOutlet weak var labelDistance: UILabel!
    @IBOutlet weak var buttonAccept: UIButton!
    @IBOutlet weak var labelCost: UILabel!
    @IBOutlet weak var constraintUser: NSLayoutConstraint!
    
    var request: Request?
    var delegate: DriverRequestCardDelegate?
    @IBAction func onAcceptTouched(_ sender: UIButton) {
        delegate?.accept(request: request!)
    }
    
    @IBAction func onRejectTouched(_ sender: Any) {
        delegate?.reject(request: request!)
    }
}
protocol DriverRequestCardDelegate: AnyObject {
    func accept(request:Request)
    func reject(request:Request)
}
