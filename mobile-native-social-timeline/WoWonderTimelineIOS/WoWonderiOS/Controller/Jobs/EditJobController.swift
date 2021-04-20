
import UIKit
import Toast_Swift
import WoWonderTimelineSDK
class EditJobController: UIViewController,UITextViewDelegate,JobTypeDelegate,JobSalaryDelegate,JobCategoryDelegate {
   
    
    
    @IBOutlet weak var saveBtn: UIButton!
    @IBOutlet weak var updateLbl: UILabel!
    @IBOutlet weak var jobTitle: RoundTextField!
    @IBOutlet weak var jobLocation: UITextView!
    @IBOutlet weak var minimumSalary: RoundTextField!
    @IBOutlet weak var maximumSalary: RoundTextField!
    @IBOutlet weak var currency: RoundTextField!
    @IBOutlet weak var date: RoundTextField!
    @IBOutlet weak var jobType: RoundTextField!
    @IBOutlet weak var Category: RoundTextField!
    @IBOutlet weak var descriptionField: UITextView!
      
    var jobDetails : JobDetails!
    var delegate : EditJobDataDelegate!
    var salaryDate = "",jobId = 0,categoryId = "",salaryType = "",job_type = "",minimum_sal = "",maximum_sal = ""
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)

    override func viewDidLoad() {
        super.viewDidLoad()
        self.descriptionField.delegate = self
        self.jobLocation.delegate = self
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        
        self.jobTitle.text = jobDetails.jobTitle
        self.jobLocation.text = jobDetails.location
        self.minimumSalary.text = jobDetails.minimumSal
        self.maximumSalary.text = jobDetails.maximumSal
        self.date.text = jobDetails.salaryDate
        self.Category.text = jobDetails.jobCategory
        self.descriptionField.text = jobDetails.jobDescription
        self.jobType.text = jobDetails.jobType
        self.categoryId = jobDetails.jobCategoryId
        self.updateLbl.text = NSLocalizedString("Update Job", comment: "Update Job")
        self.saveBtn.setTitle(NSLocalizedString("Save", comment: "Save"), for: .normal)
    }
    
    func textViewDidBeginEditing(_ textView: UITextView) {
        if (self.jobLocation.text == "Location") || (self.descriptionField.text == "Description"){
            
        }
    }
    
    
    @IBAction func SelectSalary(_ sender: Any) {
        self.date.inputView = UIView()
        self.date.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "SalaryVC") as! SalaryController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func SelectCategory(_ sender: Any) {
        self.Category.inputView = UIView()
        self.Category.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobCategoryVC") as! JobCategoryController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func SelectType(_ sender: Any) {
        self.jobType.inputView = UIView()
        self.jobType.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobTypeVC") as! JobTypeController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func Save(_ sender: Any) {
        self.editJobData()
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    private func editJobData () {
        switch status {
          case .unknown, .offline:
              print("Internet Connection Failed")
              self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
          case .online(.wwan),.online(.wiFi):
        performUIUpdatesOnMain {
            UpdateJobDataManager.sahredInstance.updateJobData(jobId: self.jobDetails.jobId, jobTitle: self.jobTitle.text!, jobDescription: self.descriptionField.text, location: self.jobLocation.text!, jobCategory: self.jobDetails.jobCategoryId, jobType: self.jobDetails.job_type, minimumSalary: self.minimumSalary.text!, maximumSalary: self.maximumSalary.text!, salaryDate: self.jobDetails.salaryType) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.message_data)
                        self.jobDetails.jobTitle = self.jobTitle.text!
                        self.jobDetails.location = self.jobLocation.text!
                        self.jobDetails.minimumSal = self.minimumSalary.text!
                        self.jobDetails.maximumSal = self.maximumSalary.text!
                        self.jobDetails.jobType = self.jobType.text!
                        self.jobDetails.salaryDate = self.date.text!
                        self.jobDetails.jobCategory = self.Category.text!
                        self.jobDetails.jobDescription = self.descriptionField.text!
//                        self.jobDetails.job_type = self.job_type
//                        self.jobDetails.salaryType = self.salaryType
//                        self.jobDetails.jobCategoryId = self.categoryId
                        self.delegate.editJobData(jobData: self.jobDetails)
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
    
    
    func jobType(jobType: String, type: String) {
        self.jobType.text = jobType
        self.jobDetails.job_type = type
        print(self.jobDetails.job_type)
        self.jobType.resignFirstResponder()
    }
       
       func salaryData(salaryDate: String, salaryType: String) {
        self.date.text = salaryDate
        self.jobDetails.salaryType = salaryType
        self.date.resignFirstResponder()
       }
       
       func category(category: String, categoryId: String) {
        self.Category.text = category
        self.jobDetails.jobCategoryId = categoryId
        print(categoryId)
        self.Category.resignFirstResponder()
       }
}
