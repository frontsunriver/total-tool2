//
//  CreateEventController.swift
//  News_Feed
//
//  Created by Ubaid Javaid on 2/7/20.
//

import UIKit
import Toast_Swift
import ZKProgressHUD
import NotificationCenter
import ZKProgressHUD

class CreateEventController: UIViewController,uploadImageDelegate{
    
    @IBOutlet weak var eventImage: UIImageView!
    @IBOutlet weak var eventName: RoundTextField!
    @IBOutlet weak var startDate: RoundTextField!
    @IBOutlet weak var endDate: RoundTextField!
    @IBOutlet weak var location: RoundTextField!
    @IBOutlet weak var eventDescription: RoundTextView!
    @IBOutlet weak var startTime: RoundTextField!
    @IBOutlet weak var endTime: RoundTextField!
    
    
    var singleDate: Date = Date()
    let formatter = DateFormatter()
    let datePicker = UIDatePicker()
    var time = false
    var tag = 0
    
    let status = Reach().connectionStatus()
    let createEventNotification = "createEvent"
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        
    }
    
    private func createEvent(data :Data?) {
        switch status {
        case .unknown, .offline:
             ZKProgressHUD.dismiss()
            self.view.makeToast("Internet Connection Failed")
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
            performUIUpdatesOnMain {
                CreateEventManager.sharedInstance.createEvent(eventName: self.eventName.text!, eventLocation: self.location.text!, eventDescription: self.eventDescription.text!, startDate: self.startDate.text!, endDate: self.endDate.text!, startTime: self.startTime.text!, endTime: self.endTime.text!, data: data) { (success, authError, error) in
                    if success != nil {
                        ZKProgressHUD.dismiss()
                        NotificationCenter.default.post(name: Notification.Name(rawValue: "createEvent"),
                                                        object: nil,userInfo: ["EventId": success!.event_id])
                        self.dismiss(animated: true, completion: nil)
                    }
                    else if authError != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        print(error!.localizedDescription)
                    }
                }
            }
        }
    }
    
    @IBAction func SelectImage(_ sender: Any) {
        let Storyboard = UIStoryboard(name: "Main", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
        vc.delegate = self
        vc.imageType = "upload"
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func StartDate(_ sender: UITextField) {
        if sender.tag == 0{
            self.tag = 0
            

              //Formate Date
              datePicker.minimumDate = Date()
              datePicker.datePickerMode = .date

             //ToolBar
             let toolbar = UIToolbar();
             toolbar.sizeToFit()
             let doneButton = UIBarButtonItem(title: "Done", style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: "Cancel", style: .plain, target: self, action: #selector(cancelDatePicker));
            toolbar.setItems([doneButton,spaceButton,cancelButton], animated: false)

            self.startDate.inputAccessoryView = toolbar
            self.startDate.inputView = datePicker
            
            self.eventName.resignFirstResponder()
            self.endDate.resignFirstResponder()
            self.startTime.resignFirstResponder()
            self.endTime.resignFirstResponder()
            self.location.resignFirstResponder()
            self.eventDescription.resignFirstResponder()
        }
        else {
            self.tag = 1
            
            //Formate Date
            datePicker.minimumDate = Date()
            datePicker.datePickerMode = .date
            
            //ToolBar
            let toolbar = UIToolbar();
            toolbar.sizeToFit()
            let doneButton = UIBarButtonItem(title: "Done", style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: "Cancel", style: .plain, target: self, action: #selector(cancelDatePicker));
            toolbar.setItems([doneButton,spaceButton,cancelButton], animated: false)
            
            self.endDate.inputAccessoryView = toolbar
            self.endDate.inputView = datePicker
            self.eventName.resignFirstResponder()
            self.startDate.resignFirstResponder()
//            self.endDate.resignFirstResponder()
            self.startTime.resignFirstResponder()
            self.endTime.resignFirstResponder()
            self.location.resignFirstResponder()
            self.eventDescription.resignFirstResponder()
            
        }
    }
    
    @objc func donedatePicker(){

      let formatter = DateFormatter()
      formatter.dateFormat = "dd/MM/yyyy"
        if self.tag == 0{
            self.startDate.text = formatter.string(from: datePicker.date)
        }
        else if (self.tag == 1) {
            self.endDate.text = formatter.string(from: datePicker.date)

        }
        else if (self.tag == 2){
            
            formatter.timeStyle = .short
            self.startTime.text = formatter.string(from: datePicker.date)
        }
        else{
            formatter.timeStyle = .short
            self.endTime.text = formatter.string(from: datePicker.date)
        }
      
      self.view.endEditing(true)
    }

    @objc func cancelDatePicker(){
       self.view.endEditing(true)
     }
    
    @IBAction func StartTime(_ sender: UITextField) {
        if sender.tag == 2{
            self.tag = 2
           self.time = true
            
            //Formate Date
//            datePicker.minimumDate = Date()
            datePicker.datePickerMode = .time
            
            //ToolBar
            let toolbar = UIToolbar();
            toolbar.sizeToFit()
            let doneButton = UIBarButtonItem(title: "Done", style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: "Cancel", style: .plain, target: self, action: #selector(cancelDatePicker));
            toolbar.setItems([doneButton,spaceButton,cancelButton], animated: false)
            
            self.startTime.inputAccessoryView = toolbar
            self.startTime.inputView = datePicker
            
            
//            self.startTime.inputView = UIView()
            self.eventName.resignFirstResponder()
            self.startDate.resignFirstResponder()
            self.endDate.resignFirstResponder()
//            self.startTime.resignFirstResponder()
            self.endTime.resignFirstResponder()
            self.location.resignFirstResponder()
            self.eventDescription.resignFirstResponder()
//        self.showCalendarandClock(showdateMonth: false, showMonth: false, showYear: false, showTime: true)
        }
        else {
            self.tag = 3
            //Formate Date
//            datePicker.minimumDate = Date()
            datePicker.datePickerMode = .time
            
            //ToolBar
            let toolbar = UIToolbar();
            toolbar.sizeToFit()
            let doneButton = UIBarButtonItem(title: "Done", style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: "Cancel", style: .plain, target: self, action: #selector(cancelDatePicker));
            toolbar.setItems([doneButton,spaceButton,cancelButton], animated: false)
            self.endTime.inputAccessoryView = toolbar
            self.endTime.inputView = datePicker
            
//            self.endTime.inputView = UIView()
            self.eventName.resignFirstResponder()
            self.startDate.resignFirstResponder()
            self.endDate.resignFirstResponder()
            self.startTime.resignFirstResponder()
//            self.endTime.resignFirstResponder()
            self.location.resignFirstResponder()
            self.eventDescription.resignFirstResponder()
//            self.showCalendarandClock(showdateMonth: false, showMonth: false, showYear: false, showTime: true)
        }
    }
    
    
    @IBAction func CreateEvent(_ sender: Any) {
        
        if (self.eventName.text?.isEmpty == true){
            self.view.makeToast("Enter Event Name")
        }
        else if (self.startDate.text?.isEmpty == true){
            self.view.makeToast("Enter Event StartDate")
            
        }
        else if (self.startTime.text?.isEmpty == true){
            self.view.makeToast("Enter Event StartTime")
        }
        else if (self.endDate.text?.isEmpty == true){
            self.view.makeToast("Enter Event EndDate")
        }
        else if (self.endTime.text?.isEmpty == true){
            self.view.makeToast("Enter Event EndTime")
        }
        else if (self.location.text?.isEmpty == true){
            self.view.makeToast("Enter Event Location")
        }
        else if (self.eventDescription.text.isEmpty == true){
            self.view.makeToast("Enter Event Description")
        }
        else {
        let imageData = self.eventImage.image!.jpegData(compressionQuality: 0.1)
        ZKProgressHUD.setForegroundColor(UIColor.hexStringToUIColor(hex: "984243"))
//        ZKProgressHUD.setBackgroundColor(UIColor.hexStringToUIColor(hex: "#696969"))
        ZKProgressHUD.show("Loading")
        self.createEvent(data: imageData)
        }
        
    }
    
    
    func uploadImage(imageType: String, image: UIImage) {
        self.eventImage.image = image
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    // MARK: - Format dates
}
