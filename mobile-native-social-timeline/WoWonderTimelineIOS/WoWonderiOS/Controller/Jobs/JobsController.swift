
import UIKit
import NotificationCenter
import Toast_Swift
import Kingfisher
import WoWonderTimelineSDK


class JobsController: UIViewController,JobDataDelegate,EditJobDataDelegate,DeleteJobDelegate{

    @IBOutlet weak var tableView: UITableView!
    
    private var jobData = [[String:Any]]()
    private var jobs = [[String:Any]]()
    var pageId  = ""
    private var selectedIndex = 0
    private var offset = "0"
    
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)

    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tableView.register(UINib(nibName: "CreateJobCell", bundle: nil), forCellReuseIdentifier: "CreateJob")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
          self.navigationItem.largeTitleDisplayMode = .never
          self.navigationItem.title = NSLocalizedString("Offer a Job", comment: "Offer a Job")
          let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
          navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.rightBarButtonItem = UIBarButtonItem(title: NSLocalizedString("Create", comment: "Create"), style: .done, target: self, action: #selector(self.Create(_:)))
        
        self.tableView.tableFooterView = UIView()
        self.getPageJobs()
    }
    

    
    
    private func getPageJobs() {
        switch status {
        case .unknown, .offline:
            print("Internet Connection Failed")
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
            GetPageJobsManager.sharedInstance.getJobs(pageId: self.pageId, offset: self.offset) {[weak self] (success, authError, error) in
                    if success != nil {
                        for i in success!.data {
                            self?.jobData.append(i)
                            self?.jobs.append((i["job"] as? [String:Any])!)
                        }
                        if let id  = self?.jobs.last?["job_id"] as? String {
                            self?.offset  = id
                        }
                        print(self?.jobs.count)
                        self?.tableView.reloadData()
                    }
                    else if authError != nil {
                        self?.view.makeToast(authError?.errors.errorText)
                   }
                    else if error != nil {
                    print(error?.localizedDescription)
                   }
                }
            }
        }
    }
    
    @IBAction func Create (_ sender:UIButton){
        let vc = Storyboard.instantiateViewController(withIdentifier: "CreateJobVC") as! CreateJobController
        vc.pageId = self.pageId
        vc.delegate = self
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
  
    @IBAction func Back(_ sender:UIButton) {
        self.navigationController?.popViewController(animated: true)
    }
    
    
    
}

extension JobsController : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.jobs.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "CreateJob") as! CreateJobCell
        let index = self.jobs[indexPath.row]
        self.tableView.rowHeight = UITableView.automaticDimension
        self.tableView.estimatedRowHeight = 270.0
        var minimumSalary = ""
        cell.appliesBtn.tag = indexPath.row
        cell.appliesBtn.addTarget(self, action: #selector(self.gotoAppliesJobVC(sender:)), for: .touchUpInside)
        if let applyCount = index["apply_count"] as? Int{
           cell.appliesBtn.setTitle("\("Show Applies")\("(\(applyCount))")", for: .normal)
        }
        if let jobTitle = index["title"] as? String{
            cell.jobTitle.text = jobTitle
        }
        if let minimum = index["minimum"] as? String{
            minimumSalary = minimum
        }
        if let maximum = index["maximum"] as? String{
            cell.amount.text = "\("$")\(" ")\(minimumSalary)\("-")\("$")\(" ")\(maximum)"
        }
        if let description = index["description"] as? String{
            cell.descriptionLabel.text = description
        }
        if let coverImage = index["image"] as? String{
//            let fullImage = "\("https://demo.wowonder.com/")\(coverImage)"
            let fullImage = coverImage

            let url = URL(string: fullImage)
            cell.coverImage.kf.setImage(with: url)
        }
    
        return  cell
    }
    
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let index = jobs[indexPath.row]
        self.selectedIndex = indexPath.row
        let vc = Storyboard.instantiateViewController(withIdentifier: "JobDetailVC") as! JobDetailController
        if let postId = self.jobData[indexPath.row]["post_id"] as? String {
        vc.postId = postId
        }
        vc.jobData = index
        vc.delegate = self
        vc.deleteDelegate = self
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func sendJobData(jobData: [String : Any]) {
        self.jobData.insert(jobData, at: 0)
        if let job = jobData["job"] as? [String:Any]{
            self.jobs.insert(job,at: 0)
        }
        self.tableView.reloadData()
    }
    
    func editJobData(jobData: JobDetails) {
        print(jobData.salaryType)
        print(jobData.job_type)
        print(jobData.jobCategoryId)
        self.jobs[selectedIndex]["tile"] = jobData.jobTitle
        self.jobs[selectedIndex]["location"] = jobData.location
        self.jobs[selectedIndex]["minimum"] = jobData.minimumSal
        self.jobs[selectedIndex]["maximum"] = jobData.maximumSal
        self.jobs[selectedIndex]["category"] = jobData.jobCategoryId
        self.jobs[selectedIndex]["salary_date"] = jobData.salaryType
        self.jobs[selectedIndex]["job_type"] = jobData.job_type
        self.jobs[selectedIndex]["description"] = jobData.jobDescription
        }
    
    func deleteJob(jobId: Int) {
        self.jobs = self.jobs.filter({($0["job_id"] as? Int) != jobId})
        self.jobData = self.jobData.filter({$0["job_id"] as? Int != jobId})
        self.tableView.reloadData()
    }
    
    @IBAction func gotoAppliesJobVC (sender:UIButton){
        let row  = sender.tag
        let index = self.jobs[row]
       if let applyCount = index["apply_count"] as? Int{
        if applyCount == 0 {
        self.view.makeToast("There are no request")
           }
        else {
        let vc = Storyboard.instantiateViewController(withIdentifier: "AppliesVC") as! JobAppliesController
         if let id = index["id"] as? Int{
            vc.jobId = id
        }
        self.navigationController?.pushViewController(vc, animated: true)
          }
        
        }
     
//     let indexPath = IndexPath(row: row, section: 0)
//     let cell = self.tableView.cellForRow(at: indexPath) as! CreateJobCell
        
        
      }
    }


    

