

import UIKit
import Toast_Swift
import WoWonderTimelineSDK


class AllJobsController: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    var jobs  = [[String:Any]]()
    var offset = ""
    var selectedIndex = 0
    
    let pulltoRefresh = UIRefreshControl()
    let spinner = UIActivityIndicatorView(style: .gray)

    
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)

    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.title = NSLocalizedString("Jobs", comment: "Jobs")
        self.tableView.backgroundColor = .white
        self.tableView.tableFooterView = UIView()
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.tableView.register(UINib(nibName: "CreateJobCell", bundle: nil), forCellReuseIdentifier: "CreateJob")
        self.tableView.tableFooterView = UIView()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        Reach().monitorReachabilityChanges()
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        self.activityIndicator.startAnimating()
        self.getAllJobs(offset: self.offset, type: "search", distance: 0, categoryId: "", jobType: "", keyword: "")
    }
    
    
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    //Pull To Refresh
    @objc func refresh(){
        self.offset = ""
        self.jobs.removeAll()
        self.tableView.reloadData()
        self.getAllJobs(offset: self.offset, type: "search", distance: 0, categoryId: "", jobType: "", keyword: "")
       }
    
    private func getAllJobs(offset:String,type:String,distance:Int,categoryId:String,jobType:String,keyword:String){
        switch status {
            case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetAllJobsManager.sharedInstance.getAllJobs(offset: offset, type: type, distance: distance, categoryId: categoryId, jobType: jobType, keyword: keyword) { (success, authError, error) in
                    if success != nil{
                        for i in success!.data{
                            self.jobs.append(i)
                        }
                        self.offset = self.jobs.last!["id"] as? String ?? ""
                        self.pulltoRefresh.endRefreshing()
                        self.activityIndicator.stopAnimating()
                        self.tableView.reloadData()
                    }
                    else if authError != nil {
                        self.activityIndicator.stopAnimating()
                        self.pulltoRefresh.endRefreshing()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil{
                        print(error?.localizedDescription)
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
            
        }
    }
    
    @IBAction func JobFilter(_ sender: Any) {
     let vc = Storyboard.instantiateViewController(withIdentifier: "JobFilterVC") as! JobFilterController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)

    }
    
    
    deinit{
        NotificationCenter.default.removeObserver(self)
    }
}
extension AllJobsController: UITableViewDelegate,UITableViewDataSource,JobFilterDelegate{

    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        self.jobs.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "CreateJob") as! CreateJobCell
        let index = self.jobs[indexPath.row]
        self.tableView.rowHeight = UITableView.automaticDimension
        self.tableView.estimatedRowHeight = 270.0
        var minimumSalary = ""
        cell.appliesBtn.tag = indexPath.row

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
            let url = URL(string: coverImage)
            cell.coverImage.kf.setImage(with: url)
        }
        if let userId = index["user_id"] as? Int{
            if userId == Int(UserData.getUSER_ID()!){
                if let applyCount = index["apply_count"] as? Int{
                    cell.appliesBtn.setTitle("\("Show Applies ")\("(\(applyCount))")", for: .normal)
                    cell.appliesBtn.addTarget(self, action: #selector(self.gotoAppliesJobVC(sender:)), for: .touchUpInside)
                }
            }
            else {
                cell.appliesBtn.setTitle("Apply Now", for: .normal)
            }
        }
        if let is_apply = index["apply"] as? Bool{
            if is_apply == true{
                cell.appliesBtn.setTitle("Already Applied", for: .normal)
            }
        }
        
        
        return  cell
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let index = jobs[indexPath.row]
               self.selectedIndex = indexPath.row
               let vc = Storyboard.instantiateViewController(withIdentifier: "JobDetailVC") as! JobDetailController
               if let postId = self.jobs[indexPath.row]["post_id"] as? String {
               vc.postId = postId
               }
               vc.jobData = index
              // vc.delegate = self
               //vc.deleteDelegate = self
               self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func jobFilter(categoryId: String, jobType: String, distance: Int) {
        
        self.jobs.removeAll()
        self.tableView.reloadData()
        self.activityIndicator.startAnimating()
        self.getAllJobs(offset: self.offset, type: "search", distance: distance, categoryId: categoryId, jobType: jobType, keyword: "")
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
    }
}
