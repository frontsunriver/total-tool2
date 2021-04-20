//
//  PageRatingController.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 3/25/20.
//

import UIKit
import Cosmos
import ZKProgressHUD

class PageRatingController: UIViewController,UITextViewDelegate{
    
    @IBOutlet weak var pageName: UILabel!
    @IBOutlet weak var RatingView: CosmosView!
    @IBOutlet weak var reviewView: UITextView!
    @IBOutlet weak var saveBtn: RoundButton!
    @IBOutlet weak var view1: DesignView!
    var pageId: String? = nil
    var page_name: String? = nil
    var rating: Double? = nil
    var text: String? = nil
    
    var delegate: PageRatingDelegate?
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        
        self.reviewView.delegate = self
        self.RatingView.settings.fillMode = .half
        self.RatingView.rating = 0.0
        self.pageName.text = self.page_name ?? ""
        RatingView.didTouchCosmos = { [weak self] rating in
            print(rating)
            self?.rating = rating
        }
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    
    func textViewDidBeginEditing(_ textView: UITextView) {
        self.reviewView.text = nil
    }
    
    func textViewDidEndEditing(_ textView: UITextView) {
        if self.reviewView.text == nil || self.reviewView.text == ""{
            self.reviewView.text = "  Write your Review"
        }
    }
    
    
    private func giveRating(){
        switch status {
        case .unknown, .offline:
            showAlert(title: "", message: "Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
          
//                ZKProgressHUD.show()
                RatePageManager.sharedInstance.ratepage(page_id: self.pageId ?? "", val: "\(self.rating)", text: self.reviewView.text ?? "") { (success, authError, error) in
                    if success != nil{
                        self.dismiss(animated: true) {
                            self.delegate?.pageRating(rating: self.rating ?? 0.0)
                        }
                    }
                    else if authError != nil{
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil{
                         ZKProgressHUD.dismiss()
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    @IBAction func Save(_ sender: Any) {
        if self.rating == 0.0 || self.rating == nil{
            self.view.makeToast("Please enter Rating")
        }
        else if self.reviewView.text == "  Write your Review"{
            self.view.makeToast("Please Enter your Review")
        }
        else {
            self.giveRating()
        }
    }
    
    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
}
