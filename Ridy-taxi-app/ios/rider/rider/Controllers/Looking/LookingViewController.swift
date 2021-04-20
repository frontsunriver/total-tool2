//
//  LookingViewController.swift
//  rider
//
//  Copyright © 2019 minimalistic apps. All rights reserved.
//

import UIKit

import Lottie

class LookingViewController: UIViewController {
    @IBOutlet weak var ViewLoading: UIView!
    var animationView: AnimationView!
    weak var delegate: LookingDelegate?
    @IBOutlet weak var textStatus: UILabel!
    @IBOutlet weak var blurView: UIView!
    @IBOutlet weak var buttonCancel: ColoredButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.onDriverAccepted), name: .newDriverAccepted, object: nil)
        NotificationCenter.default.addObserver(self, selector:#selector(self.refreshPage), name: .connectedAfterForeground, object: nil)
        let blurEffect = UIBlurEffect(style: .prominent)
        let blurEffectView = UIVisualEffectView(effect: blurEffect)
        blurEffectView.frame = self.view.bounds
        blurEffectView.autoresizingMask = [.flexibleWidth, .flexibleHeight]
        self.blurView.addSubview(blurEffectView)
        animationView = AnimationView(name: "car")
        animationView.contentMode = .scaleAspectFit
        animationView.translatesAutoresizingMaskIntoConstraints = false
        animationView.loopMode = .loop
        animationView.backgroundColor = UIColor.clear
        ViewLoading.addSubview(animationView)
        refreshUI()
    }
    
    override func updateViewConstraints() {
        super.updateViewConstraints()
        let horizontalConstraint = NSLayoutConstraint(item: animationView!, attribute: NSLayoutConstraint.Attribute.centerX, relatedBy: NSLayoutConstraint.Relation.equal, toItem: ViewLoading, attribute: NSLayoutConstraint.Attribute.centerX, multiplier: 1, constant: 0)
        let verticalConstraint = NSLayoutConstraint(item: animationView!, attribute: NSLayoutConstraint.Attribute.centerY, relatedBy: NSLayoutConstraint.Relation.equal, toItem: ViewLoading, attribute: NSLayoutConstraint.Attribute.centerY, multiplier: 1, constant: 0)
        let widthConstraint = NSLayoutConstraint(item: animationView!, attribute: NSLayoutConstraint.Attribute.width, relatedBy: NSLayoutConstraint.Relation.equal, toItem: ViewLoading, attribute: NSLayoutConstraint.Attribute.width, multiplier: 1, constant: 0)
        let heightConstraint = NSLayoutConstraint(item: animationView!, attribute: NSLayoutConstraint.Attribute.height, relatedBy: NSLayoutConstraint.Relation.equal, toItem: ViewLoading, attribute: NSLayoutConstraint.Attribute.height, multiplier: 1, constant: 0)
        ViewLoading.addConstraints([horizontalConstraint,verticalConstraint,widthConstraint,heightConstraint])
    }
    
    @objc func refreshPage() {
        LoadingOverlay.shared.showOverlay(view: self.view)
        GetCurrentRequestInfo().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(let response):
                Request.shared = response.request
                self.refreshUI()
                
            case .failure(_):
                self.dismiss(animated: true, completion: nil)
                self.delegate?.cancel()
            }
        }
    }
    
    func refreshUI() {
        let travel = Request.shared
        switch(travel.status!) {
        case .DriverAccepted, .Finished, .WaitingForPostPay, .WaitingForReview, .Started, .WaitingForPrePay, .Arrived:
            self.dismiss(animated: true, completion: nil)
            self.delegate?.found()
            break
            
        case .DriverCanceled, .RiderCanceled:
            self.dismiss(animated: true, completion: nil)
            self.delegate?.cancel()
            break
            
        case .Booked:
            self.buttonCancel.isEnabled = true
            self.animationView.animation = Animation.named("check")
            self.animationView.loopMode = .playOnce
            self.animationView.play()
            self.textStatus.text = NSLocalizedString("Trip is booked & will be dispatched 30 minutes before pickup time. You can send app to background now.", comment: "")
            let dateFormatter = DateFormatter()
            dateFormatter.dateStyle = .none
            dateFormatter.timeStyle = .short
            
        case .Expired:
            let alert = UIAlertController(title: NSLocalizedString("Message", comment: ""), message: NSLocalizedString("Sadly your request wasn't accepted in appropriate time and it is expired now.", comment: ""), preferredStyle: .alert)
            alert.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: ""), style: .default) { action in
                self.dismiss(animated: true, completion: nil)
                self.delegate?.cancel()
            })
            self.present(alert, animated: true)
            
        case .Found, .NotFound, .NoCloseFound, .Requested:
            self.buttonCancel.isEnabled = true
            self.animationView.animation = Animation.named("car")
            self.animationView.loopMode = .loop
            self.animationView.play()
            self.textStatus.text = NSLocalizedString("Looking for drivers...", comment: "")
        }
    }
    
    override func viewDidAppear(_ animated: Bool) {
        self.animationView.play()
        refreshPage()
    }
    
    @IBAction func onCancelRequestClicked(_ sender: UIButton) {
        LoadingOverlay.shared.showOverlay(view: self.view)
        CancelRequest().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(_):
                self.dismiss(animated: true, completion: nil)
                
            case .failure(let error):
                error.showAlert()
            }
        }
        
    }
    
    @objc func onDriverAccepted(_ notification: Notification) {
        if let request = notification.object as? Request {
            Request.shared = request
            dismiss(animated: true, completion: nil)
            self.delegate?.found()
        }
    }
    

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destination.
        // Pass the selected object to the new view controller.
    }
    */
    
}

protocol LookingDelegate: class {
    func found()
    func cancel()
}
