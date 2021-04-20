
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
    @IBOutlet weak var eventLabel: UILabel!
    @IBOutlet weak var addBtn: UIButton!
    @IBOutlet weak var imageBtn: RoundButton!
    
    var singleDate: Date = Date()
    let formatter = DateFormatter()
    let datePicker = UIDatePicker()
    var time = false
    var tag = 0
    var isEdit = 0
    var eventId = 0
    var eventDetails = [String:Any]()
    
    let status = Reach().connectionStatus()
    let createEventNotification = "createEvent"
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.eventLabel.text = NSLocalizedString("Create Event", comment: "Create Event")
        self.addBtn.setTitle(NSLocalizedString("Add", comment: "Add"), for: .normal)
        self.eventName.placeholder = NSLocalizedString("Event Name", comment: "Event Name")
        self.startDate.placeholder = NSLocalizedString("Start Date", comment: "Start Date")
        self.endDate.placeholder = NSLocalizedString("End Date", comment: "End Date")
        self.location.placeholder = NSLocalizedString("Location", comment: "Location")
        self.eventDescription.placeholder = NSLocalizedString("Description", comment: "Description")
        self.imageBtn.setTitle(NSLocalizedString("Image", comment: "Image"), for: .normal)
        if self.isEdit == 1{
            self.eventLabel.text = NSLocalizedString("Edit Event", comment: "Edit Event")
            self.addBtn.setTitle(NSLocalizedString("Edit", comment: "Edit"), for: .normal)
            if let image = self.eventDetails["cover"] as? String{
                let url = URL(string: image)
                self.eventImage.kf.setImage(with: url)
            }
            if let name = self.eventDetails["name"] as? String{
                self.eventName.text = name
            }
            if let date = self.eventDetails["end_date"] as? String{
                self.endDate.text  = date
            }
            if let desc = self.eventDetails["description"] as? String{
                self.eventDescription.text = desc
            }
            if let location = self.eventDetails["location"] as? String{
                self.location.text = location
            }
            if let startDate = self.eventDetails["start_date"] as? String{
                self.startDate.text  = startDate
            }
            if let startTime = self.eventDetails["start_time"] as? String{
                self.startTime.text  = startTime
            }
            if let endTime = self.eventDetails["end_time"] as? String{
                self.endTime.text  = endTime
            }
            if let id = self.eventDetails["id"] as? String{
                self.eventId = Int(id) ?? 0
            }
        }
        else{
            self.addBtn.setTitle(NSLocalizedString("Add", comment: "Add"), for: .normal)
            self.eventLabel.text = NSLocalizedString("Create Event", comment: "Create Event")
        }
        
    }
    
    private func createEvent(data :Data?) {
        switch status {
        case .unknown, .offline:
            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
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
    
    private func EditEvent(data:Data?){
        switch status {
        case .unknown, .offline:
            ZKProgressHUD.dismiss()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
            EditEventManager.sharedInstance.editEvent(eventId: self.eventId, eventName: self.eventName.text!, eventLocation: self.location.text!, eventDescription: self.eventDescription.text!, startDate: self.startDate.text!, endDate: self.endDate.text!, startTime: self.startTime.text!, endTime: self.endTime.text!, data: data) { (success, authError, error) in
                if (success != nil){
                    ZKProgressHUD.dismiss()
                    let eventData = ["event_name":self.eventName.text!,"start_date":self.startDate.text!,"start_time":self.startTime.text!,"end_date":self.endDate.text!,"end_time":self.endTime.text!,"location":self.location.text!,"description":self.eventDescription.text!,"image":self.eventImage.image,"event_id":self.eventId] as [String : Any]
                    NotificationCenter.default.post(name: Notification.Name(rawValue: "EditEvent"),
                    object: nil,userInfo: ["event_data": eventData])
                    self.dismiss(animated: true, completion: nil)
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors?.errorText)
                }
                else if error != nil {
                    ZKProgressHUD.dismiss()
                    print(error!.localizedDescription)
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
            let doneButton = UIBarButtonItem(title: NSLocalizedString("Done", comment: "Done"), style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .plain, target: self, action: #selector(cancelDatePicker));
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
            let doneButton = UIBarButtonItem(title: NSLocalizedString("Done", comment: "Done"), style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .plain, target: self, action: #selector(cancelDatePicker));
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
            let doneButton = UIBarButtonItem(title: NSLocalizedString("Done", comment: "Done"), style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .plain, target: self, action: #selector(cancelDatePicker));
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
            let doneButton = UIBarButtonItem(title: NSLocalizedString("Done", comment: "Done"), style: .plain, target: self, action: #selector(donedatePicker));
            let spaceButton = UIBarButtonItem(barButtonSystemItem: UIBarButtonItem.SystemItem.flexibleSpace, target: nil, action: nil)
            let cancelButton = UIBarButtonItem(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .plain, target: self, action: #selector(cancelDatePicker));
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
            self.view.makeToast(NSLocalizedString("Enter Event Name", comment: "Enter Event Name"))
        }
        else if (self.startDate.text?.isEmpty == true){
            self.view.makeToast(NSLocalizedString("Enter Event StartDate", comment: "Enter Event StartDate"))
            
        }
        else if (self.startTime.text?.isEmpty == true){
            self.view.makeToast(NSLocalizedString("Enter Event StartTime", comment: "Enter Event StartTime"))
        }
        else if (self.endDate.text?.isEmpty == true){
            self.view.makeToast(NSLocalizedString("Enter Event EndDate", comment: "Enter Event EndDate"))
        }
        else if (self.endTime.text?.isEmpty == true){
            self.view.makeToast(NSLocalizedString("Enter Event EndTime", comment: "Enter Event EndTime"))
        }
        else if (self.location.text?.isEmpty == true){
            self.view.makeToast(NSLocalizedString("Enter Event Location", comment: "Enter Event Location"))
        }
        else if (self.eventDescription.text.isEmpty == true){
            self.view.makeToast(NSLocalizedString("Enter Event Description", comment: "Enter Event Description"))
        }
        else {
            let imageData = self.eventImage.image!.jpegData(compressionQuality: 0.1)
            ZKProgressHUD.setForegroundColor(UIColor.hexStringToUIColor(hex: "984243"))
            ZKProgressHUD.show(NSLocalizedString("Loading", comment: "Loading"))
            if (self.isEdit == 1){
                self.EditEvent(data: imageData)
            }
            else{
            self.createEvent(data: imageData)
            }
        }
        
    }
    
    
    func uploadImage(imageType: String, image: UIImage) {
        self.eventImage.image = nil
        self.eventImage.image = image
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    // MARK: - Format dates
}
