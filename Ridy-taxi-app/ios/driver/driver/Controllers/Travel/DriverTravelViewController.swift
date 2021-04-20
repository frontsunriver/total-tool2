//
//  DriverTravelViewController.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import SPAlert
import MapKit

class DriverTravelViewController: UIViewController, CLLocationManagerDelegate, MKMapViewDelegate {
    @IBOutlet weak var labelCost: UILabel!
    @IBOutlet weak var labelTime: UILabel!
    @IBOutlet weak var map: MKMapView!
    @IBOutlet weak var buttonStart: UIButton!
    @IBOutlet weak var buttonFinish: UIButton!
    @IBOutlet weak var buttonMessage: UIButton!
    @IBOutlet weak var buttonCall: UIButton!
    @IBOutlet weak var buttonCancel: UIButton!
    @IBOutlet weak var backgroundView: UIView!
    @IBOutlet weak var buttonArrived: ColoredButton!
    
    var timer = Timer()
    var locationManager = CLLocationManager()
    var pointAnnotations: [MKPointAnnotation] = []
    var destinationMarker = MKPointAnnotation()
    var driverMarker = MKPointAnnotation()
    var route = [CLLocationCoordinate2D]()
    var distance: Double = 0.0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.travelCanceled), name: .cancelTravel, object: nil)
        NotificationCenter.default.addObserver(self, selector:#selector(self.requestRefresh), name: .connectedAfterForeground, object: nil)
        if let cost = Request.shared.costBest {
            labelCost.text = MyLocale.formattedCurrency(amount: cost - (Request.shared.providerShare ?? 0), currency: Request.shared.currency!)
        }
        map.delegate = self
        map.layoutMargins = UIEdgeInsets(top: 50, left: 0, bottom: 300, right: 0)
        let blurEffect = UIBlurEffect(style: UIBlurEffect.Style.regular)
        let blurEffectView = UIVisualEffectView(effect: blurEffect)
        blurEffectView.frame = backgroundView.bounds
        blurEffectView.autoresizingMask = [.flexibleWidth, .flexibleHeight]
        backgroundView.addSubview(blurEffectView)
        let maskLayer = CAShapeLayer()
        maskLayer.path = UIBezierPath(roundedRect: view.bounds, byRoundingCorners: [.topLeft, .topRight], cornerRadii: CGSize(width: 10, height: 10)).cgPath
        self.backgroundView.layer.mask = maskLayer
        locationManager.delegate = self
        locationManager.desiredAccuracy = kCLLocationAccuracyHundredMeters
        locationManager.distanceFilter = 50
        locationManager.allowsBackgroundLocationUpdates = true
        locationManager.pausesLocationUpdatesAutomatically = false
        locationManager.activityType = .automotiveNavigation
        locationManager.requestAlwaysAuthorization()
        locationManager.startUpdatingLocation()
        self.navigationItem.hidesBackButton = true
        timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(self.onEachSecond), userInfo: nil, repeats: true)
    }
    
    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        self.requestRefresh()
    }
    
    @IBAction func onStartTapped(_ sender: UIButton) {
        LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
        Start().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(let response):
                Request.shared = response
                self.refreshScreen()
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    @IBAction func onFinishTapped(_ sender: UIButton) {
        if Request.shared.confirmationCode != nil {
            showConfirmationDialog()
        }
        if route.count > 2 {
            Request.shared.log = MapsUtil.encode(items: MapsUtil.simplify(items: route, tolerance: 50))
        }
        let f = FinishService(cost: Request.shared.costBest!, log: Request.shared.log, distance: Request.shared.distanceReal!)
        finishTravel(finishService: f)
    }
    
    func showConfirmationDialog() {
        let question = UIAlertController(title: "Confirmation", message: "Finishing this service needs confirmation code delivered to user. Please enter it in following field:", preferredStyle: .alert)
        question.addAction(UIAlertAction(title: "OK", style: .default) { action in
            let code = question.textFields![0].text!
            let f = FinishService(cost: Request.shared.costBest!, distance: Request.shared.distanceReal!, confirmationCode: Int(code)!)
            self.finishTravel(finishService: f)
        })
        question.addTextField() { textField in
            textField.placeholder = "code"
        }
        self.present(question, animated: true)
    }
    
    func finishTravel(finishService: FinishService) {
        timer.invalidate()
        LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
        Finish(confirmationCode: finishService.confirmationCode, distance: finishService.distance ?? 0, log: finishService.log ?? "").execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(let response):
                Request.shared.status = response.status ? Request.Status.Finished : Request.Status.WaitingForPostPay
                if response.status {
                    let alert = UIAlertController(title: "Message", message: "Payment has been settled and credit has been added to your wallet.", preferredStyle: .alert)
                    alert.addAction(UIAlertAction(title: "OK", style: .default) { action in
                        self.navigationController?.popViewController(animated: true)
                    })
                    self.present(alert, animated: true)
                } else {
                    let vc = Bundle.main.loadNibNamed("WaitingForPayment", owner: self, options: nil)?.first as! WaitingForPaymentViewController
                    vc.onDone = { b in
                        self.requestRefresh()
                    }
                    self.present(vc, animated: true)
                }
                
            case .failure(let error):
                if error.status == ErrorStatus.ConfirmationCodeRequired {
                    self.showConfirmationDialog()
                } else {
                    error.showAlert()
                }
            }
        }
    }
    
    @IBAction func onButtonArrivedTapped(_ sender: ColoredButton) {
        LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
        Arrived().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(let response):
                Request.shared = response
                self.refreshScreen()
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    @IBAction func onMessageTapped(_ sender: UIButton) {
        let vc = ChatViewController()
        vc.sender = Request.shared.rider!
        self.navigationController!.pushViewController(vc, animated: true)
    }
    
    @IBAction func onCancelTapped(_ sender: UIButton) {
        LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
        Cancel().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(_):
                Request.shared.status = .DriverCanceled
                self.refreshScreen()
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    @objc private func requestRefresh() {
        LoadingOverlay.shared.showOverlay(view: self.navigationController?.view)
        GetCurrentRequestInfo().execute() { result in
            LoadingOverlay.shared.hideOverlayView()
            switch result {
            case .success(let response):
                Request.shared = response.request
                self.refreshScreen()
                
            case .failure(_):
                self.navigationController?.popViewController(animated: true)
            }
        }
    }
    
    private func refreshScreen(travel: Request = Request.shared) {
        switch travel.status! {
        case .RiderCanceled, .DriverCanceled:
            let alert = UIAlertController(title: "Success", message: "Service Has Been Canceled.", preferredStyle: .alert)
            alert.addAction(UIAlertAction(title: "Allright!", style: .default) { action in
                self.navigationController?.popViewController(animated: true)
            })
            present(alert, animated: true)
            break;
            
        case .DriverAccepted:
            buttonFinish.isHidden = true
            buttonStart.isHidden = true
            let ann = MKPointAnnotation()
            ann.coordinate = travel.points[0]
            ann.title = travel.addresses[0]
            pointAnnotations.append(ann)
            map.addAnnotation(ann)
            if map.annotations.count > 1 {
                map.showAnnotations(map.annotations, animated: true)
            } else {
                map.setCenter(map.annotations[0].coordinate, animated: true)
            }
            break;
            
        case .Arrived:
            buttonStart.isHidden = false
            buttonArrived.isHidden = true
            
        case .Started:
            buttonMessage.isHidden = true
            buttonCall.isHidden = true
            buttonCancel.isHidden = true
            buttonStart.isHidden = true
            buttonFinish.isHidden = false
            buttonArrived.isHidden = true
            if pointAnnotations.count > 0 {
                for point in pointAnnotations {
                    map.removeAnnotation(point)
                }
            }
            for (index, point) in travel.points.dropFirst().enumerated() {
                let ann = MKPointAnnotation()
                ann.coordinate = point
                ann.title = travel.addresses[index + 1]
                pointAnnotations.append(ann)
                map.addAnnotation(ann)
            }
            if map.annotations.count > 1 {
                map.showAnnotations(map.annotations, animated: true)
            } else {
                map.setCenter(map.annotations[0].coordinate, animated: true)
            }
            break;
            
        case .WaitingForPostPay:
            let vc = Bundle.main.loadNibNamed("WaitingForPayment", owner: self, options: nil)?.first as! WaitingForPaymentViewController
            vc.onDone = { b in
                self.requestRefresh()
            }
            self.present(vc, animated: true)
            
            
        case .Finished, .WaitingForReview:
            SPAlert.present(title: "Paid!", preset: .card)
            self.navigationController?.popViewController(animated: true)
            
        default:
            let alert = UIAlertController(title: "Error", message: "Unkown Status: \(travel.status!.rawValue)", preferredStyle: .alert)
            alert.addAction(UIAlertAction(title: "Allright!", style: .default) { action in
                self.navigationController?.popViewController(animated: true)
            })
            self.present(alert, animated: true)
        }
    }
    
    @objc func travelCanceled() {
        Request.shared.status = .RiderCanceled
        refreshScreen()
    }
    
    enum MarkerType: String {
        case pickup = "pickup"
        case dropoff = "dropOff"
        case driver = "driver"
    }
    
    func mapView(_ mapView: MKMapView, viewFor annotation: MKAnnotation) -> MKAnnotationView? {
        guard let annotation = annotation as? MKPointAnnotation else { return nil }
        let identifier = pointAnnotations.contains(annotation) ? MarkerType.dropoff : MarkerType.driver
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
                view.canShowCallout = true
                let button = UIButton(type: .detailDisclosure)
                button.addTarget(self, action: #selector(annotationTapped), for: UIControl.Event.touchUpInside)
                view.rightCalloutAccessoryView = button
                break;
                
            case .dropoff:
                view.markerTintColor = UIColor(hex: 0xFFA000)
                view.canShowCallout = true
                let button = UIButton(type: .detailDisclosure)
                button.addTarget(self, action: #selector(annotationTapped), for: UIControl.Event.touchUpInside)
                view.rightCalloutAccessoryView = button
                break;
                
            default:
                view.glyphImage = UIImage(named: "annotation_glyph_car")
            }
        }
        return view
    }
    
    @objc func annotationTapped() {
        openNavigationMenu(location: map.selectedAnnotations.last!.coordinate)
    }
    
    func openNavigationMenu(location: CLLocationCoordinate2D) {
        let alert = UIAlertController(title: "Navigation", message: "Select App to navigate with", preferredStyle: .actionSheet)
        alert.addAction(UIAlertAction(title: "Apple Maps", style: .default, handler: { action in
            if let url = URL(string: "http://maps.apple.com/?q=\(location.latitude),\(location.longitude)&z=10&t=s"),
                UIApplication.shared.canOpenURL(url) {
                UIApplication.shared.open(url)
            }
        }))
        alert.addAction(UIAlertAction(title: "Google Maps", style: .default, handler: { action in
            if let url = URL(string: "comgooglemaps://?daddr=\(location.latitude),\(location.longitude)&directionsmode=driving"),
                UIApplication.shared.canOpenURL(URL(string: "comgooglemaps://")!) {
                UIApplication.shared.open(url)
            }
        }))
        alert.addAction(UIAlertAction(title: "Waze", style: .default, handler: { action in
            if let url = URL(string: "https://www.waze.com/ul?ll=\(location.latitude),\(location.longitude)&navigate=yes"),
                UIApplication.shared.canOpenURL(url) {
                UIApplication.shared.open(url)
            }
        }))
        alert.addAction(UIAlertAction(title: "Yandex.Maps", style: .default, handler: { action in
            if let url = URL(string: "yandexmaps://maps.yandex.com/?ll=\(location.latitude),\(location.longitude)&z=12"),
                UIApplication.shared.canOpenURL(URL(string: "yandexmaps://")!) {
                UIApplication.shared.open(url)
            }
            
        }))
        alert.addAction(UIAlertAction(title: "Cancel", style: .cancel, handler: nil))
        self.present(alert, animated: true, completion: nil)
    }
    
    @objc func onEachSecond() {
        let now = Date()
        let etaInterval = Request.shared.startTimestamp != nil ? (Request.shared.startTimestamp! / 1000) + Double(Request.shared.durationBest!) : Request.shared.etaPickup! / 1000
        let etaTime = Date(timeIntervalSince1970: etaInterval)
        if etaTime <= now {
            labelTime.text = NSLocalizedString("Soon!", comment: "When driver is coming later than expected.")
        } else {
            let formatter = DateComponentsFormatter()
            formatter.allowedUnits = [.minute, .second]
            formatter.unitsStyle = .short
            labelTime.text = formatter.string(from: now, to: etaTime)
        }
        recalculateCost()
        
    }
    
    func locationManager(_ manager: CLLocationManager, didUpdateLocations locations: [CLLocation]) {
        let userLocation:CLLocation = locations[0] as CLLocation
        if route.count > 0 {
            let distance = Int(userLocation.distance(from: CLLocation(latitude: route[route.count - 1].latitude, longitude: route[route.count - 1].longitude)))
            Request.shared.distanceReal! += distance
        } else {
            route.append(userLocation.coordinate)
            Request.shared.distanceReal = 0
        }
        LocationUpdate(jwtToken: UserDefaultsConfig.jwtToken!, location: userLocation.coordinate, inTravel: true).execute() { _ in
            
        }
        route.append(userLocation.coordinate)
        driverMarker.coordinate = userLocation.coordinate
        map.addAnnotation(driverMarker)
        map.showAnnotations(pointAnnotations, animated: true)
    }
    
    func recalculateCost() {
        if let service = Request.shared.service , service.feeEstimationMode == .Dynamic {
            var cost = 0.0
            if Request.shared.status == Request.Status.Started {
                let duration = (Date().timeIntervalSince1970 - (Double(Request.shared.startTimestamp!) / 1000)) / 60
                cost = service.baseFare + (distance / 100 * service.perHundredMeters) + (duration * service.perMinuteDrive)
            }
            labelCost.text = String(format: "~\(MyLocale.formattedCurrency(amount: cost, currency: Request.shared.currency!))")
        }
    }
    
    @IBAction func onCallTouched(_ sender: UIButton) {
        if let call = Request.shared.rider?.mobileNumber,
            let url = URL(string: "tel://\(call)"),
            UIApplication.shared.canOpenURL(url) {
                UIApplication.shared.open(url)
        }
    }
}
