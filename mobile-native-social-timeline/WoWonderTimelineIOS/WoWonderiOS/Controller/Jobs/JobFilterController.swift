
import UIKit
import Toast_Swift
import WoWonderTimelineSDK

class JobFilterController: UIViewController,JobTypeDelegate,JobCategoryDelegate {
    
    @IBOutlet weak var filterLbl: UILabel!
    @IBOutlet weak var distanceLbl: UILabel!
    @IBOutlet weak var jobTypeLbl: UILabel!
    @IBOutlet weak var categoryLbl: UILabel!
    @IBOutlet weak var distanceSlider: UISlider!
    @IBOutlet weak var distanceLabel: UILabel!
    @IBOutlet weak var filterBtn: RoundButton!
    @IBOutlet weak var jobType: UILabel!
    @IBOutlet weak var jobCategory: UILabel!
    
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
    let status = Reach().connectionStatus()
    
    var categoryId = ""
    var type = ""
    var distance = ""
    var delegate : JobFilterDelegate!
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()

        self.filterLbl.text = NSLocalizedString("Filter", comment: "Filter")
        self.distanceLbl.text = NSLocalizedString("Distance", comment: "Distance")
        self.jobTypeLbl.text = NSLocalizedString("Job Type", comment: "Job Type")
        self.categoryLbl.text  = NSLocalizedString("Category", comment: "Category")

    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
//            self.view.makeToast(status)
        }
    }
    
    
    @IBAction func ApplyFilter(_ sender: Any) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            print("filter")
            self.delegate.jobFilter(categoryId: self.categoryId, jobType: self.type, distance: Int(self.distance) ?? 0)
            self.dismiss(animated: true, completion: nil)
            
        }
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func SelectJobType(_ sender: Any) {
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobTypeVC") as! JobTypeController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func SelectCategory(_ sender: Any) {
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobCategoryVC") as! JobCategoryController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
        
    }
    
    @IBAction func DistanceSlider(_ sender: Any) {
        self.distanceLabel.text! = "\(String(format: "%i",Int(self.distanceSlider.value)))\(" km")"
        self.distance = String(format: "%i",Int(self.distanceSlider.value))
    }
    
    
    func jobType(jobType: String, type: String) {
        self.jobType.text = jobType
        self.type = type
    }
    
    func category(category: String, categoryId: String) {
        self.jobCategory.text = category
        self.categoryId = categoryId
    }
}
