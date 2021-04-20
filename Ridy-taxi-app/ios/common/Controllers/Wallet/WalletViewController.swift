//
//  DriverWalletViewController.swift
//  Rider
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import Eureka
import Stripe
import SPAlert
import Braintree
import BraintreeDropIn

class WalletViewController: FormViewController {
    
    
    var paymentField: STPPaymentCardTextField?
    var amount: Double?
    var currency: String?
    let amounts: [Double] = [5, 20, 50]
    var methodsRow: SegmentedRow<PaymentGateway> {
        get {
            return (self.form.rowBy(tag: "methods") as? SegmentedRow<PaymentGateway>)!
        }
    }
    
    var cardRow: StripeRow {
        get {
            return (self.form.rowBy(tag: "card") as? StripeRow)!
        }
    }
    
    var amountRow: StepperRow {
        get {
            return (self.form.rowBy(tag: "amount") as? StepperRow)!
        }
    }
    
    var creditsRow: PushRow<Wallet> {
        get {
            return (self.form.rowBy(tag: "credits") as? PushRow<Wallet>)!
        }
    }
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        LoadingOverlay.shared.showOverlay(view: self.view)
        WalletInfo().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(let methods):
                if methods.wallet.count < 1 && self.currency == nil {
                    SPAlert.present(title: NSLocalizedString("You need to do at least one trip so wallet would be enabled for you.", comment: ""), preset: .exclamation)
                    self.navigationController?.popViewController(animated: true)
                    return
                }
                if methods.gateways.count < 1 {
                    SPAlert.present(title: NSLocalizedString("You need to do at least one trip so wallet would be enabled for you.", comment: ""), preset: .error)
                    self.navigationController?.popViewController(animated: true)
                    return
                }
                self.creditsRow.options = methods.wallet
                self.creditsRow.updateCell()
                self.methodsRow.options = methods.gateways
                self.methodsRow.value = self.methodsRow.options![0]
                self.methodsRow.updateCell()
                self.cardRow.hidden = Condition(booleanLiteral: methods.gateways[0].type != .Stripe || methods.gateways[0].type != .Flutterwave || methods.gateways[0].type != .PayGate)
                self.cardRow.evaluateHidden()
                if methods.gateways.count > 1 {
                    self.methodsRow.hidden = false
                    self.methodsRow.evaluateHidden()
                }
                if self.amount != nil {
                    self.creditsRow.value = self.creditsRow.options!.first() { $0.currency == self.currency }
                    self.creditsRow.updateCell()
                    self.amountRow.value = self.amount! - (self.creditsRow.value?.amount ?? 0)
                    if self.creditsRow.value == nil {
                        self.creditsRow.hidden = true
                        self.creditsRow.evaluateHidden()
                    }
                    if self.amountRow.value! <= 0.0 {
                        self.navigationController?.popViewController(animated: true)
                    }
                    self.amountRow.cell.stepper.minimumValue = self.amount!
                    self.amountRow.reload()
                } else {
                    self.creditsRow.value = self.creditsRow.options!.first
                    self.creditsRow.updateCell()
                }
                
            case .failure(let error):
                error.showAlert()
            }
        }
        form +++ Section(header: "", footer: self.amount == nil ? "" : NSLocalizedString("You can charge your wallet for an amount more than your \"Service Fee\" and your future Service Fees will be deducted automatically from your in-app wallet.", comment: "")) {
            $0.tag = "sec_currrent_credit"
            }
            <<< LabelRow("fee") {
                $0.title = "Service Fee"
                if self.amount != nil {
                    let formatter = NumberFormatter()
                    formatter.locale = Locale.current
                    formatter.numberStyle = .currency
                    formatter.currencyCode = self.currency
                    $0.value = formatter.string(from: NSNumber(value: self.amount!))!
                }
                $0.hidden = Condition(booleanLiteral: (self.amount == nil))
            }
            <<< PushRow<Wallet>("credits") {
                $0.title = "Credit"
                $0.selectorTitle = NSLocalizedString("Current Credit", comment: "")
                $0.disabled = Condition(booleanLiteral: (self.currency != nil))
                $0.displayValueFor = {
                    if $0 == nil { return nil }
                    let formatter = NumberFormatter()
                    formatter.locale = Locale.current
                    formatter.numberStyle = .currency
                    formatter.currencyCode = $0!.currency
                    return formatter.string(from: NSNumber(value: $0!.amount!))!
                }
            }
        form +++ Section(NSLocalizedString("Add credit", comment: "Wallet section title")) {
            $0.tag = "sec_add_credit"
            }
            <<< SegmentedRow<PaymentGateway>("methods") {
                $0.options = []
                $0.hidden = true
                $0.displayValueFor = { gateway in
                    return gateway?.title
                }
            }.onChange { row in
                if row.cell.segmentedControl.selectedSegmentIndex < 0 {
                    return
                }
                self.cardRow.hidden = Condition(booleanLiteral: row.value!.type != .Stripe && row.value!.type != .Flutterwave && row.value!.type != .PayGate)
                self.cardRow.evaluateHidden()
            }
            <<< StripeRow("card") { row in
                row.hidden = true
                row.cellUpdate { cell, _row in
                    self.paymentField = cell.paymentField
                }
            }
            <<< StepperRow("amount") {
                $0.value = self.amount ?? 0
                $0.title = NSLocalizedString("Amount", comment: "Wallet's amount field description")
            }
            <<< SegmentedRow<String>("amounts") {
                $0.cell.segmentedControl.isMomentary = true
                $0.cell.segmentedControl.addTarget(self, action: #selector(self.selectedPresetValue), for: .valueChanged)
                $0.options = self.amounts.map({return String("+\($0)")})
        }
            +++ Section("") {_ in
        }
            <<< ButtonRow("button_pay") {
                $0.title = NSLocalizedString("Pay!", comment: "Wallet Pay Button")
            }.onCellSelection() {cell, row in
                guard let amountRowValue = self.amountRow.value, Int(amountRowValue) > 0 else {
                    SPAlert.present(title: NSLocalizedString("Amount required", comment: ""), preset: .exclamation)
                    return
                }
                self.amount = Double(amountRowValue)
                switch self.methodsRow.value!.type {
                case .Stripe:
                    self.startStripePayment()
                    
                case .BrainTree:
                    self.startBraintreePayment()
                    
                case .Flutterwave, .PayGate:
                    guard let _paymentField = self.paymentField, let cNumber = _paymentField.cardParams.number else {
                        SPAlert.present(title: NSLocalizedString("Card input required", comment: ""), preset: .card)
                        return
                    }
                    let token = "{\"cardNumber\":\(cNumber),\"cvv\":\(self.paymentField!.cvc!),\"expiryMonth\":\(self.paymentField!.expirationMonth),\"expiryYear\":\(self.paymentField!.expirationYear)}"
                    self.doPayment(token: token, amount: self.amount!)
                    
                case .PayPal, .Paytm, .Razorpay, .Paystack, .PayU, .Instamojo, .MIPS, .CustomLink:
                    LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
                    GetPaymentLink(gatewayId: self.methodsRow.value!.id, amount: self.amount!, currency: self.creditsRow.value?.currency ?? self.currency!).execute() { result in
                        LoadingOverlay.shared.hideOverlayView()
                        switch result {
                        case .success(let res):
                            let viewController = PaymentWebPageViewController()
                            viewController.url = res.url
                            viewController.delegate = self
                            self.present(viewController, animated: true, completion: nil)
                            break
                            
                        case .failure(let error):
                            error.showAlert()
                        }
                    }
                    
                }
        }
    }
    
    @objc func selectedPresetValue(r: UISegmentedControl) {
        self.amountRow.value = (self.amountRow.value ?? 0) + Double(self.amounts[r.selectedSegmentIndex])
        self.amountRow.updateCell()
    }
    
    func startBraintreePayment() {
        self.showDropIn(clientTokenOrTokenizationKey: self.methodsRow.value!.publicKey!)
    }
    
    func showDropIn(clientTokenOrTokenizationKey: String) {
        LoadingOverlay.shared.showOverlay(view: self.view)
        let request =  BTDropInRequest()
        let dropIn = BTDropInController(authorization: clientTokenOrTokenizationKey, request: request) { (controller, result, error) in
            if (error != nil) {
                LoadingOverlay.shared.hideOverlayView()
                SPAlert.present(title: NSLocalizedString(error!.localizedDescription, comment: ""), preset: .error)
            } else if (result?.isCancelled == true) {
                LoadingOverlay.shared.hideOverlayView()
            } else if let result = result {
                self.doPayment(token: (result.paymentMethod?.nonce)!, amount: self.amount!)
            }
            controller.dismiss(animated: true, completion: nil)
        }
        self.present(dropIn!, animated: true, completion: nil)
    }
    
    func startStripePayment() {
        StripeAPI.defaultPublishableKey = self.methodsRow.value!.publicKey!
        guard let _paymentField = paymentField, _paymentField.isValid else {
            SPAlert.present(title: NSLocalizedString("Card input required", comment: ""), preset: .card)
            return
        }
        let amountDouble = self.amount!
        var amountInt = 0
        let currency = self.creditsRow.value?.currency ?? self.currency!
        if currency == "USD" {
            amountInt = Int(amountDouble * 100)
        } else {
            amountInt = Int(amountDouble)
        }
        GetStripeClientSecret(gatewayId: self.methodsRow.value!.id, amount: amountInt, currency: currency).execute() { result in
            switch result {
            case .success(let res):
                let cardParams = _paymentField.cardParams
                let paymentMethodParams = STPPaymentMethodParams(card: cardParams, billingDetails: nil, metadata: nil)
                let paymentIntentParams = STPPaymentIntentParams(clientSecret: res.clientSecret)
                paymentIntentParams.paymentMethodParams = paymentMethodParams

                STPPaymentHandler.shared().confirmPayment(paymentIntentParams, with: self) { (status, paymentIntent, error) in
                    switch (status) {
                    case .failed:
                        SPAlert.present(title: "Error", message: error?.localizedDescription ?? "", preset: .error)
                        break
                        
                    case .canceled:
                        break
                        
                    case .succeeded:
                        self.doPayment(token: paymentIntent!.stripeId, amount: amountDouble)
                        break
                        
                    @unknown default:
                        fatalError()
                        break
                    }
                }
                break
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    func doPayment(token: String, amount: Double, pin: Int? = nil, otp: Int? = nil, transactionId: String? = nil) {
        let dto = WalletTopUpDTO(gatewayId: self.methodsRow.value!.id, amount: amount, currency: self.creditsRow.value?.currency ?? self.currency!, token: token, pin: pin, otp: otp, transactionId: transactionId)
        WalletTopUp(dto: dto).execute { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(_):
                _ = self.navigationController?.popViewController(animated: true)
                SPAlert.present(title: NSLocalizedString("Payment Successful", comment: ""), preset: .done)
                
            case .failure(let error):
                if error.status == .PINCodeRequired || error.status == .OTPCodeRequired {
                    self.showVerifyDialog(dto: dto, error: error)
                    return
                }
                error.showAlert()
            }
        }
    }
    
    func showVerifyDialog(dto: WalletTopUpDTO, error: ServerError) {
        let question = UIAlertController(title: NSLocalizedString("Verification", comment: "Verification Title"), message: error.status == .OTPCodeRequired ? NSLocalizedString("Please Enter OTP code sent to your mobile number", comment: "OTP Message") : NSLocalizedString("Please Enter PIN code", comment: "PIN Code message"), preferredStyle: .alert)
        question.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "OK Button"), style: .default) { action in
            let code = question.textFields![0].text!
            if error.status == .OTPCodeRequired {
                self.doPayment(token: dto.token, amount: dto.amount, otp: Int(code), transactionId: error.message)
            } else {
                self.doPayment(token: dto.token, amount: dto.amount, pin: Int(code))
            }
        })
        question.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel Button"), style: .cancel, handler: nil))
        question.addTextField() { textField in
            textField.placeholder = NSLocalizedString("Code", comment: "")
        }
        self.present(question, animated: true)
    }
}

extension WalletViewController: STPAuthenticationContext {
    func authenticationPresentingViewController() -> UIViewController {
        return self
    }
}

extension WalletViewController: WebPaymentResultDelegate {
    func paid() {
        _ = self.navigationController?.popViewController(animated: true)
        SPAlert.present(title: NSLocalizedString("Payment Successful", comment: ""), preset: .done)
    }
    
    func canceled() {
        
    }
}
