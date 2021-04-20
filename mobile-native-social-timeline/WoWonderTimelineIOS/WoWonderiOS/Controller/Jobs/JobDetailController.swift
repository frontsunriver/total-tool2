
import UIKit
import Kingfisher
import Toast_Swift
import WoWonderTimelineSDK

class JobDetailController: UIViewController,EditJobDelegate,EditJobDataDelegate,DeleteJobDelegate{
    @IBOutlet weak var coverImage: UIImageView!
    @IBOutlet weak var iconImage: Roundimage!
    @IBOutlet weak var jobTitle: UILabel!
    @IBOutlet weak var pageTitle: UILabel!
    @IBOutlet weak var Location: UILabel!
    @IBOutlet weak var time: UILabel!
    @IBOutlet weak var jobType: UILabel!
    @IBOutlet weak var jobCategory: UILabel!
    @IBOutlet weak var minimum: UILabel!
    @IBOutlet weak var maximum: UILabel!
    @IBOutlet weak var jobDescription: UITextView!
    @IBOutlet weak var NavLabel: UILabel!
    @IBOutlet weak var appliesBtn: UIButton!
    @IBOutlet weak var editBtn: UIButton!
    @IBOutlet weak var minimumLbl: UILabel!
    @IBOutlet weak var maximumLbl: UILabel!
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
    let status = Reach().connectionStatus()
    
    var jobData = [String:Any]()
    var jobDetails : JobDetails!
    var postId = ""
    var applyCount = 0
    var salaryDate = "",jobId = 0,categoryId = "",salaryType = "",job_type = "",minimum_sal = "",maximum_sal = ""
    
    var delegate : EditJobDataDelegate!
    var deleteDelegate : DeleteJobDelegate!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
          self.navigationItem.title = ""
          let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
          navigationController?.navigationBar.titleTextAttributes = textAttributes
         self.appliesBtn.setTitle("\(NSLocalizedString("SHOW APPLIES", comment: "SHOW APPLIES"))\("(\(0))")", for: .normal)
        self.minimumLbl.text = NSLocalizedString("Minimum", comment: "Minimum")
        self.maximumLbl.text = NSLocalizedString("Maximum", comment: "Maximum")
        self.loadData()
    }
    
    
    @IBAction func ShowApplies(_ sender: Any) {
        if self.appliesBtn.title(for: .normal) == "Apply NOW"{
            print("ApplyNow")
        }
        else{
            if let applyCount = self.jobData["apply_count"] as? Int{
                if applyCount == 0 {
                    self.view.makeToast("There are no request")
                }
                else {
                    let vc = Storyboard.instantiateViewController(withIdentifier: "AppliesVC") as! JobAppliesController
                    if let id = self.jobData["id"] as? Int{
                        vc.jobId = id
                    }
                    self.navigationController?.pushViewController(vc, animated: true)
                }
                
            }
        }
    }
    
    @IBAction func EditJob(_ sender: UIButton) {
        let vc = Storyboard.instantiateViewController(withIdentifier: "Edit&DeleteVC") as! EditAndDeleteController
        vc.delegate = self
        vc.deleteDelegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.navigationBar.isHidden = true
        self.navigationController?.popViewController(animated: true)
    }
    
    
    
    private func loadData(){
        if let coverImage = self.jobData["image"] as? String{
            //            let fullImage = "\("https://demo.wowonder.com/")\(coverImage)"
            let fullImage = coverImage
            let url = URL(string: fullImage)
            self.coverImage.kf.setImage(with: url)
        }
        if let job_Id = self.jobData["id"] as? Int{
            self.jobId = job_Id
            print(self.jobId)
        }
        
        if let applyCount = self.jobData["apply_count"] as? Int {
            self.applyCount = applyCount
        }
        
        if let jobTitle = self.jobData["title"] as? String{
            self.jobTitle.text = jobTitle
            self.navigationItem.title = jobTitle
        }
        
        if let location = self.jobData["location"] as? String{
            self.Location.text = location
        }
        if let salaryType = self.jobData["salary_date"] as? String{
            if salaryType == "per_hour"{
                self.salaryDate = "Per Hour"
                self.salaryType = "per_hour"
            }
            else if salaryType == "per_month"{
                self.salaryDate = "Per Month"
                self.salaryType = "per_month"
            }
            else if salaryType == "per_day"{
                self.salaryDate = "Per Day"
                self.salaryType = "per_day"
            }
            else if salaryType == "per_year"{
                self.salaryDate = "Per Year"
                self.salaryType = "per_year"
            }
            else if salaryType == "per_week"{
                self.salaryDate = "Per Week"
                self.salaryType = "per_week"
            }
        }
        if let minimumSal = self.jobData["minimum"] as? String{
            self.minimum.text = "\(minimumSal)\(" ")\(self.salaryDate)"
            self.minimum_sal = minimumSal
        }
        if let maximumSal = self.jobData["maximum"] as? String{
            self.maximum.text = "\(maximumSal)\(" ")\(self.salaryDate)"
            self.maximum_sal = maximumSal
        }
        if let descrip = self.jobData["description"] as? String{
            self.jobDescription.text = descrip
        }
        if let page = self.jobData["page"] as? [String:Any]{
            if let pageName = page["page_name"] as? String{
                self.pageTitle.text = "\("@")\(pageName)"
            }
            if let pageIcon = page["avatar"] as? String{
                let url = URL(string: pageIcon)
                self.iconImage.kf.setImage(with: url)
            }
            
            if let pageOwner = page["is_page_onwer"] as? Bool {
                if pageOwner == true {
                    self.appliesBtn.setTitle("\(NSLocalizedString("SHOW APPLIES", comment: "SHOW APPLIES"))\("(\(self.applyCount))")", for: .normal)
                    self.navigationItem.rightBarButtonItem = UIBarButtonItem(title: NSLocalizedString("Edit", comment: "Edit"), style: .done, target: self, action: #selector(self.EditJob(_:)))
                }
                else {
                    self.appliesBtn.setTitle("Apply NOW", for: .normal)
                }
            }
        }
        if let type = self.jobData["job_type"] as? String{
            if type == "full_time"{
                self.jobType.text = "Full Time"
                self.job_type = "full_time"
                
            }
            else if type == "part_time"{
                self.jobType.text = "Part Time"
                self.job_type = "part_time"
            }
            else if type == "internship"{
                self.jobType.text = "Internship"
                self.job_type = "internship"
                
            }
            else if type == "volunteer"{
                self.jobType.text = "Volunteer"
                self.job_type = "volunteer"
            }
            else {
                self.jobType.text = "Contract"
                self.job_type = "contract"
                
            }
        }
        
        if let category = self.jobData["category"] as? String{
            if category == "1"{
                self.jobCategory.text = "Other"
                self.categoryId = "1"
            }
            else if category == "2"{
                self.jobCategory.text = "Admin & Office"
                self.categoryId  = "2"
            }
            else if category == "3"{
                self.jobCategory.text = "Art & Design"
                self.categoryId  = "3"
            }
            else if category == "4"{
                self.jobCategory.text = "Business Operations"
                self.categoryId = "4"
            }
            else if category == "5"{
                self.jobCategory.text = "Cleaning & Facilites"
                self.categoryId  = "5"
            }
            else if category == "6"{
                self.jobCategory.text = "Community & Social Services"
                self.categoryId = "6"
            }
            else if category == "7"{
                self.jobCategory.text = "Computer & Data"
                self.categoryId = "7"
            }
            else if category == "8"{
                self.jobCategory.text = "Constraction & Mining"
                self.categoryId  = "8"
            }
            else if category == "9"{
                self.jobCategory.text = "Education"
                self.categoryId  = "9"
            }
            else if category == "10"{
                self.jobCategory.text = "Farming & Foresty"
                self.categoryId  = "10"
            }
            else if category == "11"{
                self.jobCategory.text = "Health Care"
                self.categoryId  = "11"
            }
            else if category == "12"{
                self.jobCategory.text = "Intsallation,Maintenance & Repair"
                self.categoryId  = "12"
            }
            else if category == "13"{
                self.jobCategory.text = "Legal"
                self.categoryId  = "13"
            }
            else if category == "14"{
                self.jobCategory.text = "Management"
                self.categoryId = "14"
            }
            else if category == "15"{
                self.jobCategory.text = "Manufacturing"
                self.categoryId = "15"
            }
            else if category == "16"{
                self.jobCategory.text = "Media & Communication"
                self.categoryId  = "16"
            }
            else if category == "17"{
                self.jobCategory.text = "Personal Care"
                self.categoryId  = "17"
            }
            else if category == "18"{
                self.jobCategory.text = "Protective Services"
                self.categoryId = "18"
            }
            else if category == "19"{
                self.jobCategory.text = "Restaurant & Hospitality"
                self.categoryId = "19"
            }
            else if category == "20"{
                self.jobCategory.text = "Retail & Sales"
                self.categoryId = "20"
            }
            else if category == "21"{
                self.jobCategory.text = "Science & Engineering"
                self.categoryId = "21"
            }
            else if category == "22"{
                self.jobCategory.text = "Sports & Entertainment"
                self.categoryId = "22"
            }
            else if category == "23"{
                self.jobCategory.text = "Transportation"
                self.categoryId = "23"
            }
        }
        
    }
    
    private func deleteJob(){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                DeleteJobManager.sharedInstance.deleteJob(postId: self.postId) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.action)
                        self.deleteDelegate.deleteJob(jobId: self.jobId)
                        self.navigationController?.popViewController(animated: true)
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    func editJob() {
        let jobDetails = JobDetails(jobId: self.jobId, jobTitle: self.jobTitle.text!, jobType: self.jobType.text!, job_type: self.job_type, jobCategory: self.jobCategory.text!, jobCategoryId: self.categoryId, jobDescription: self.jobDescription.text!, minimumSal: self.minimum_sal, maximumSal: self.maximum_sal, salaryDate: self.salaryDate, salaryType: self.salaryType, location: self.Location.text!)
        let vc = Storyboard.instantiateViewController(withIdentifier: "EditJobVC") as! EditJobController
        vc.jobDetails = jobDetails
        vc.delegate = self
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
    
    func editJobData(jobData: JobDetails) {
        self.jobDetails = jobData
        print(jobData.salaryType)
        print(jobData.job_type)
        self.jobTitle.text = self.jobDetails.jobTitle
        self.categoryId = self.jobDetails.jobCategoryId
        self.jobCategory.text = self.jobDetails.jobCategory
        self.job_type = self.jobDetails.job_type
        self.jobType.text = self.jobDetails.jobType
        self.salaryDate = self.jobDetails.salaryDate
        self.salaryType = self.jobDetails.salaryType
        self.minimum_sal = self.jobDetails.minimumSal
        self.maximum_sal = self.jobDetails.maximumSal
        self.Location.text = self.jobDetails.location
        self.jobDescription.text = self.jobDetails.jobDescription
        self.minimum.text = "\(self.minimum_sal)\(" ")\(self.salaryDate)"
        self.maximum.text = "\(self.maximum_sal)\(" ")\(self.salaryDate)"
        self.navigationItem.title = self.jobDetails.jobTitle
        self.delegate.editJobData(jobData: jobDetails)
    }
    
    func deleteJob(jobId: Int) {
        let alert = UIAlertController(title: "", message: NSLocalizedString("Are you sure Want to delete", comment: "Are you sure Want to delete"), preferredStyle: .actionSheet)
        alert.addAction(UIAlertAction(title: NSLocalizedString("Delete", comment: "Delete"), style: .destructive, handler: { (_) in
            self.deleteJob()
        }))
        alert.addAction(UIAlertAction(title: NSLocalizedString("No", comment: "No"), style: .cancel, handler: { (_) in
            print("User click No button")
        }))
        if let popoverController = alert.popoverPresentationController {
            popoverController.sourceView = self.view
            popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
            popoverController.permittedArrowDirections = []
        }
        self.present(alert, animated: true, completion: nil)
    }
    
}
