

import UIKit
import Toast_Swift
import WoWonderTimelineSDK

class CreateJobController: UIViewController,uploadImageDelegate,JobTypeDelegate,JobSalaryDelegate,JobCurrencyDelegate,JobCategoryDelegate,UITextFieldDelegate{

    @IBOutlet weak var jobTitleField: RoundTextField!
    @IBOutlet weak var Location: UITextView!
    @IBOutlet weak var maximumField: RoundTextField!
    @IBOutlet weak var minimumField: RoundTextField!
    @IBOutlet weak var currencyField: RoundTextField!
    @IBOutlet weak var dateField: RoundTextField!
    @IBOutlet weak var JobType: RoundTextField!
    @IBOutlet weak var categoryField: RoundTextField!
    @IBOutlet weak var Description: UITextView!
    @IBOutlet weak var thumbnail: UIImageView!
    @IBOutlet weak var imageLabel: UILabel!
    @IBOutlet weak var createJobLbl: UILabel!
    @IBOutlet weak var createBtn: UIButton!
    
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
    var pageId = ""
   private var categoryId = "0"
   private var currencyId = ""
   private var jobType = ""
   private var salaryType = ""
    var delegate : JobDataDelegate!
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.createJobLbl.text = NSLocalizedString("Create a Job", comment: "Create a Job")
        self.createBtn.setTitle(NSLocalizedString("Create", comment: "Create"), for: .normal)
        self.imageLabel.text  = NSLocalizedString("Select Image", comment: "Select Image")
        self.jobTitleField.placeholder = NSLocalizedString("Job Title", comment: "Job Title")
        self.maximumField .placeholder = NSLocalizedString("Maximum", comment: "Maximum")
        self.minimumField.placeholder = NSLocalizedString("Minimum", comment: "Minimum")
        self.currencyField.placeholder = NSLocalizedString("Currency", comment: "Currency")
        self.dateField.placeholder = NSLocalizedString("Date", comment: "Date")
        self.JobType.placeholder = NSLocalizedString("Job Type", comment: "Job Type")
        self.categoryField.placeholder = NSLocalizedString("Category", comment: "Category")
        self.Location.text = NSLocalizedString("Location", comment: "Location")
        self.Description.text = NSLocalizedString("Description", comment: "Description")
    }
    
    
    private func createJob (data:Data?) {
        CreateJobManager.sharedInstance.createJob(jobTitle: self.jobTitleField.text!, jobDescription: self.Description.text!, location: self.Location.text!, jobCategory:  self.categoryId, jobType: self.jobType, minimumSalary: self.minimumField.text!, maximumSalary: self.maximumField.text!, salaryDate: salaryType, currency: self.currencyId, pageId: self.pageId , cuurentLat: 24.247868979, currentLng: 67.0978687675, data: data) { (success, authError, error) in
            if success != nil {
                self.view.makeToast("Job Created Successfully")
                self.dismiss(animated: true) {
                    self.delegate.sendJobData(jobData: success!.data)
                    
                }
            }
            else if authError != nil {
                self.view.makeToast(authError?.errors.errorText)
                
            }
            else if error != nil {
                print(error?.localizedDescription)
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
    
    
    @IBAction func Create(_ sender: Any) {
        let imageData = self.thumbnail.image!.jpegData(compressionQuality: 0.1)
        self.createJob(data: imageData)
        
    }
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func SelectCurrency(_ sender: Any) {
        self.currencyField.inputView = UIView()
        self.currencyField.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "CurrencyVC") as! CurrencyController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func SelectDate(_ sender: Any) {
        self.dateField.inputView = UIView()
        self.dateField.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "SalaryVC") as! SalaryController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func SelectType(_ sender: Any) {
        self.JobType.inputView = UIView()
        self.JobType.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobTypeVC") as! JobTypeController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func SelectCategory(_ sender: Any) {
        self.categoryField.inputView = UIView()
        self.categoryField.inputAccessoryView = UIView()
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobCategoryVC") as! JobCategoryController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    func uploadImage(imageType: String, image: UIImage) {
        self.thumbnail.image = image
    }
    
    func jobCurrency(currency: String,currencyId: String) {
        self.currencyField.text = currency
        self.currencyId = currencyId
        self.currencyField.resignFirstResponder()
    }
    
    func jobType(jobType: String, type: String) {
        self.JobType.text = jobType
        self.jobType = type
        self.JobType.resignFirstResponder()
    }
    
    func salaryData(salaryDate: String, salaryType: String) {
        self.dateField.text = salaryDate
        self.salaryType = salaryType
        self.dateField.resignFirstResponder()
    }
    
    func category(category: String, categoryId: String) {
        self.categoryField.text = category
        self.categoryId = categoryId
        print(categoryId)
        self.categoryField.resignFirstResponder()
    }
    
}
