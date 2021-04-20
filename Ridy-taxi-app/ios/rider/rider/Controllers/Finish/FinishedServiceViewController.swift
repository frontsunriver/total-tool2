//
//  FinishedServiceViewController.swift
//  rider
//
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit
import SPAlert
import Eureka

class FinishedServiceViewController: FormViewController {
    public var travel: Request = Request.shared
    private var score: Int?
    private var review: String?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        form +++ Section(NSLocalizedString("Summary", comment: ""))
            <<< LabelRow() { row in
                row.title = NSLocalizedString("Final fee", comment: "")
                row.value = MyLocale.formattedCurrency(amount: travel.costAfterCoupon!, currency: travel.currency!)
            }
            /*<<< PushRow<String>() {
            $0.title = NSLocalizedString("Payment Method", comment: "Payment Method")
            $0.selectorTitle = NSLocalizedString("Select Your preffered payment Method", comment: "Title of paymnet method selection in finish travel.")
            $0.options = ["Braintree","Cash","Stripe"]
            }//.onChange {  }
            <<< ButtonRow() {
                $0.title = "Pay"
                $0.onCellSelection() { cell, row in
                    
                }
            }*/
            +++ Section(NSLocalizedString("Review", comment: "")) { $0.tag = "review" }
            <<< SegmentedRow<String>("score") {
                $0.options = ["1", "2", "3", "4", "5"]
                $0.title = NSLocalizedString("Score", comment: "")
                }.onChange { row in
                    self.score = Int(row.value!)!
                }
            <<< TextRow() { row in
                row.title = NSLocalizedString("Your review", comment: "")
                row.placeholder = NSLocalizedString("Enter your review here...", comment: "")
                row.onChange() { row in
                    self.review = row.value
                }
        }
            <<< ButtonRow() { row in
                row.title = NSLocalizedString("Save", comment: "")
                row.onCellSelection() { cell, row in
                    guard let _score = self.score else {
                        SPAlert.present(title: NSLocalizedString(NSLocalizedString("you haven't selected any score point.", comment: ""), comment: ""), preset: .exclamation)
                        return;
                    }
                    LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
                    ReviewDriver(review: Review(score: _score * 20, review: self.review ?? "")).execute() { result in
                        LoadingOverlay.shared.hideOverlayView()
                        switch result {
                        case .success(_):
                            SPAlert.present(title: NSLocalizedString(NSLocalizedString("Thanks for review!", comment: ""), comment: ""), preset: .star)
                            Request.shared.status = .Finished
                            self.navigationController?.popViewController(animated: true)
                            
                        case .failure(let error):
                            error.showAlert()
                        }
                    }
                }
        }
        
    }
}
