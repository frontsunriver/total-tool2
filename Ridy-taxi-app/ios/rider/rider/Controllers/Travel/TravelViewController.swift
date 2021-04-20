//
//  TravelViewController.swift
//  Rider
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import MapKit
import SPAlert
import Kingfisher

class TravelViewController: UIViewController, CouponsViewDelegate, MKMapViewDelegate {
    @IBOutlet weak var map: MKMapView!
    @IBOutlet weak var labelCost: UILabel!
    @IBOutlet weak var labelTime: UILabel!
    @IBOutlet weak var buttonCall: ColoredButton!
    @IBOutlet weak var buttonMessage: ColoredButton!
    @IBOutlet weak var buttonCancel: ColoredButton!
    @IBOutlet weak var buttonPay: ColoredButton!
    
    var pickupMarker = MKPointAnnotation()
    var destinationMarkers: [MKPointAnnotation] = []
    var driverMarker = MKPointAnnotation()
    var timer: Timer!
    @IBOutlet weak var confirmationBarButton: UIBarButtonItem!
    @IBOutlet weak var tabBar: UISegmentedControl!
    @IBOutlet weak var backgroundView: UIView!
    @IBOutlet weak var viewStatistics: UIView!
    @IBOutlet weak var viewDriver: UIStackView!
    @IBOutlet weak var textDriverName: UILabel!
    @IBOutlet weak var textPlateNumber: UILabel!
    @IBOutlet weak var imgDriver: UIImageView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.onArrived), name: .arrived, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.onServiceStarted), name: .serviceStarted, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.onServiceCanceled), name: .serviceCanceled, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.onServiceFinished), name: .serviceFinished, object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.onTravelInfoReceived), name: .travelInfoReceived, object: nil)
        NotificationCenter.default.addObserver(self, selector:#selector(self.requestRefresh), name: .connectedAfterForeground, object: nil)
        textDriverName.text = "\(Request.shared.driver?.firstName ?? "- ") \(Request.shared.driver?.lastName ?? "")"
        var details = [String]()
        if let carName = Request.shared.driver?.car?.title {
            details.append(carName)
        }
        if let carColor = Request.shared.driver?.carColor {
            details.append(carColor)
        }
        if let carPlate = Request.shared.driver?.carPlate {
            details.append(carPlate)
        }
        textPlateNumber.text = details.joined(separator: ", ")
        tabBar.addTarget(self, action: #selector(selectedTabItem), for: .valueChanged)
        map.layoutMargins = UIEdgeInsets(top: 50, left: 0, bottom: 290, right: 0)
        let blurEffect = UIBlurEffect(style: .prominent)
        let blurEffectView = UIVisualEffectView(effect: blurEffect)
        blurEffectView.frame = backgroundView.bounds
        blurEffectView.autoresizingMask = [.flexibleWidth, .flexibleHeight]
        backgroundView.addSubview(blurEffectView)
        let maskLayer = CAShapeLayer()
        maskLayer.path = UIBezierPath(roundedRect: view.bounds, byRoundingCorners: [.topLeft, .topRight], cornerRadii: CGSize(width: 10, height: 10)).cgPath
        self.backgroundView.layer.mask = maskLayer
        driverMarker = MKPointAnnotation()
        map.delegate = self
        self.navigationItem.hidesBackButton = true
        if Request.shared.service?.canEnableVerificationCode == false {
            self.navigationItem.rightBarButtonItem = nil
        }
        if let cost = Request.shared.costAfterCoupon {
            self.labelCost.text = MyLocale.formattedCurrency(amount: cost, currency: Request.shared.currency!)
        }
        timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(self.onEachSecond), userInfo: nil, repeats: true)
    }
    
    @objc func selectedTabItem(sender: UISegmentedControl) {
        if sender.selectedSegmentIndex == 1 {
            viewDriver.isHidden = true
            viewStatistics.isHidden = false
        } else {
            viewDriver.isHidden = false
            viewStatistics.isHidden = true
        }
    }
    
    @IBAction func onMessageTapped(_ sender: UIButton) {
        let vc = ChatViewController()
        vc.sender = Request.shared.driver!
        self.navigationController!.pushViewController(vc, animated: true)
    }
    
    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        self.requestRefresh()
    }
    
    @objc func onEachSecond() {
        let now = Date()
        let etaInterval = Request.shared.startTimestamp != nil ? (Request.shared.startTimestamp! / 1000) + Double(Request.shared.durationBest!) : Request.shared.etaPickup! / 1000
        let etaTime = Date(timeIntervalSince1970: etaInterval)
        if etaTime <= now {
            if Request.shared.status == .Arrived {
                labelTime.text = NSLocalizedString("Arrived!", comment: "Driver Arrived text instead of time.")
            } else {
                labelTime.text = NSLocalizedString("Soon!", comment: "When driver is coming later than expected.")
            }
            
        } else {
            let formatter = DateComponentsFormatter()
            formatter.allowedUnits = [.minute, .second]
            formatter.unitsStyle = .short
            labelTime.text = formatter.string(from: now, to: etaTime)
        }
    }
    
    @objc private func requestRefresh() {
        GetCurrentRequestInfo().execute() { result in
            switch result {
            case .success(let response):
                Request.shared = response.request
                self.refreshScreen(driverLocation: response.driverLocation)
                
            case .failure(_):
                self.navigationController?.popViewController(animated: false)
            }
        }
    }
    
    private func refreshScreen(travel: Request = Request.shared, driverLocation: CLLocationCoordinate2D?) {
        if let cost = Request.shared.costAfterCoupon {
            self.labelCost.text = MyLocale.formattedCurrency(amount: cost, currency: Request.shared.currency!)
        }
        if let driverImage = travel.driver?.media?.address {
            let processor = DownsamplingImageProcessor(size: imgDriver.intrinsicContentSize) |> RoundCornerImageProcessor(cornerRadius: imgDriver.intrinsicContentSize.width / 2)
            let url = URL(string: Config.Backend + driverImage.replacingOccurrences(of: " ", with: "%20"))
            imgDriver.kf.setImage(with: url, placeholder: UIImage(named: "Nobody"), options: [
                .processor(processor),
                .scaleFactor(UIScreen.main.scale),
                .transition(.fade(0.5)),
                .cacheOriginalImage
            ], completionHandler: { result in
                switch result {
                case .success(let value):
                    print("Task done for: \(value.source.url?.absoluteString ?? "")")
                case .failure(let error):
                    print("Job failed: \(error.localizedDescription)")
                }
            })
        }
        switch travel.status! {
        case .RiderCanceled, .DriverCanceled:
            let alert = UIAlertController(title: NSLocalizedString("Message", comment: ""), message: NSLocalizedString("Service Has Been Canceled.", comment: ""), preferredStyle: .alert)
            alert.addAction(UIAlertAction(title: NSLocalizedString("Alright!", comment: ""), style: .default) { action in
                _ = self.navigationController?.popViewController(animated: true)
            })
            present(alert, animated: true)
            break
            
        case .DriverAccepted:
            pickupMarker.coordinate = travel.points[0]
            map.addAnnotation(pickupMarker)
            if let _location = driverLocation {
                driverMarker.coordinate = _location
                map.addAnnotation(driverMarker)
                map.showAnnotations([pickupMarker, driverMarker], animated: true)
            } else {
                let region = MKCoordinateRegion(center: travel.points[0], latitudinalMeters: 1000, longitudinalMeters: 1000)
                map.setRegion(region, animated: true)
            }
            break
            
        case .Arrived:
            SPAlert.present(title: "Driver has arrived", preset: .flag)
            break
            
        case .Started:
            buttonCall.isHidden = true
            buttonMessage.isHidden = true
            buttonCancel.isHidden = true
            map.removeAnnotation(pickupMarker)
            for (index, point) in travel.points.enumerated() {
                if index == 0 {
                    continue;
                }
                let p = MKPointAnnotation()
                p.coordinate = point
                destinationMarkers.append(p)
                map.addAnnotation(p)
            }
            if driverLocation != nil || destinationMarkers.count > 1 {
                if(driverLocation != nil) {
                    driverMarker.coordinate = driverLocation!
                    map.addAnnotation(driverMarker)
                    destinationMarkers.append(driverMarker)
                    map.showAnnotations(destinationMarkers, animated: true)
                    destinationMarkers.removeLast()
                } else {
                    map.showAnnotations(destinationMarkers, animated: true)
                }
            } else {
                let region = MKCoordinateRegion(center: travel.points[1], latitudinalMeters: 1000, longitudinalMeters: 1000)
                map.setRegion(region, animated: true)
            }
            break
            
        case .WaitingForPostPay:
            if travel.service?.paymentMethod == .OnlyCash {
                let alert = UIAlertController(title: NSLocalizedString("Payment", comment: ""), message: NSLocalizedString("Service Has Been finished and payment is waiting to be settled.", comment: ""), preferredStyle: .alert)
                alert.addAction(UIAlertAction(title: NSLocalizedString("Alright!", comment: ""), style: .default) { action in
                    self.requestRefresh()
                })
            } else {
                if let vc = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "Wallet") as? WalletViewController {
                    vc.amount = Request.shared.costAfterCoupon
                    vc.currency = Request.shared.currency
                    self.navigationController?.pushViewController(vc, animated: true)
                }
            }
            
            break
            
        case .WaitingForReview:
            let storyboard = UIStoryboard(name: "Main", bundle: nil)
            let controller = storyboard.instantiateViewController(withIdentifier: "finishedViewController")
            self.navigationController?.pushViewController(controller, animated: false)
            break
            
        case .Finished:
            SPAlert.present(title: NSLocalizedString("Done!", comment: ""), preset: .done)
            self.navigationController?.popViewController(animated: true)
            
        default:
            let alert = UIAlertController(title: "Error", message: "Unknown status: \(travel.status!.rawValue)", preferredStyle: .alert)
            alert.addAction(UIAlertAction(title: "Allright!", style: .default) { action in
                _ = self.navigationController?.popViewController(animated: true)
            })
            self.present(alert, animated: true)
        }
    }
    
    @IBAction func onCancelTapped(_ sender: UIButton) {
        Cancel().execute() { result in
            switch result {
            case .success(_):
                Request.shared.status = .RiderCanceled
                self.refreshScreen(driverLocation: nil)
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    @IBAction func onWalletTapped(_ sender: UIButton) {
        buttonPay.isHidden = true
        if let vc = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "Wallet") as? WalletViewController {
            vc.amount = Request.shared.costAfterCoupon
            vc.currency = Request.shared.currency
            self.navigationController!.pushViewController(vc, animated: true)
        }
    }
    
    @objc func onServiceStarted(_ notification: Notification) {
        Request.shared = notification.object as! Request
        let location = driverMarker.coordinate.latitude != 0 ? driverMarker.coordinate : nil
        refreshScreen(driverLocation: location)
    }
    
    @objc func onArrived(_ notification: Notification) {
        Request.shared = notification.object as! Request
        refreshScreen(driverLocation: nil)
    }
    
    @objc func onServiceCanceled(_ notification: Notification) {
        Request.shared.status = .DriverCanceled
        refreshScreen(driverLocation: nil)
    }
    
    @objc func onServiceFinished(_ notification: Notification) {
        let obj = notification.object as! [Any]
        Request.shared.status = (obj[0] as! Bool) == true ? Request.Status.WaitingForReview : Request.Status.WaitingForPostPay
        refreshScreen(driverLocation: nil)
    }
    
    @objc func onTravelInfoReceived(_ notification: Notification) {
        refreshScreen(driverLocation: (notification.object as! CLLocationCoordinate2D))
    }
    
    @IBAction func onSelectCouponClicked(_ sender: UIButton) {
        if let vc = UIStoryboard(name: "Main", bundle: nil).instantiateViewController(withIdentifier: "CouponsCollectionViewController") as? CouponsCollectionViewController
        {
            vc.selectMode = true
            vc.delegate = self
            self.navigationController!.pushViewController(vc, animated: true)
        }
    }
    
    func didSelectedCoupon(_ coupon: Coupon) {
        ApplyCoupon(code: coupon.code!).execute() { result in
            switch result {
            case .success(_):
                SPAlert.present(title: "Coupon Applied", preset: .done)
                self.requestRefresh()
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    enum MarkerType: String {
        case pickup = "pickup"
        case dropoff = "dropOff"
        case driver = "driver"
    }
    
    func mapView(_ mapView: MKMapView, viewFor annotation: MKAnnotation) -> MKAnnotationView? {
        guard let annotation = annotation as? MKPointAnnotation else { return nil }
        let identifier = annotation == pickupMarker ? MarkerType.pickup : (annotation == driverMarker ? MarkerType.driver : MarkerType.dropoff)
        var view: MKMarkerAnnotationView
        if let dequeuedView = mapView.dequeueReusableAnnotationView(withIdentifier: identifier.rawValue) as? MKMarkerAnnotationView {
            dequeuedView.annotation = annotation
            view = dequeuedView
        } else {
            view = MKMarkerAnnotationView(annotation: annotation, reuseIdentifier: identifier.rawValue)
            switch(identifier) {
            case .pickup:
                view.glyphImage = UIImage(named: "annotation_glyph_home")
                view.markerTintColor = UIColor(hex: 0x009688)
                break;
                
            case .dropoff:
                view.markerTintColor = UIColor(hex: 0xFFA000)
                break;
                
            default:
                view.glyphImage = UIImage(named: "annotation_glyph_car")
            }
        }
        return view
    }
    
    @IBAction func onCallTouched(_ sender: UIButton) {
        if let call = Request.shared.driver?.mobileNumber, let url = URL(string: "tel://\(call)"), UIApplication.shared.canOpenURL(url) {
            UIApplication.shared.open(url)
        }
    }
    
    @IBAction func enableConfirmationClicked(_ sender: UIBarButtonItem) {
        EnableVerification().execute() { result in
            switch result {
            case .success(let response):
                let message = UIAlertController(title: NSLocalizedString("Success", comment: ""), message: "Confirmation code service is enabled. Driver will need to enter \(response) to finish service.", preferredStyle: .alert)
                message.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: ""), style: .default))
                self.present(message, animated: true)
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
}
