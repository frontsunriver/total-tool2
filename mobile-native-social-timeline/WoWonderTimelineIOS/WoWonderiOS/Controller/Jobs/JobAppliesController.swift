

import UIKit
import Toast_Swift
import Kingfisher
import WoWonderTimelineSDK
class JobAppliesController: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    private var appliesArray = [[String:Any]]()
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)

    var jobId = 0
    private var offset = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tableView.register(UINib(nibName: "JobAppliesCell", bundle: nil), forCellReuseIdentifier: "AppliesCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.tableView.tableFooterView = UIView()
        self.getApplyJob()
    }
    

    private func getApplyJob() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                AppliesJobManager.sharedInstance.appliesJob(jobId: self.jobId, offset: self.offset) { (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            self.appliesArray.append(i)
                        }
                        if let off_set = self.appliesArray.last!["id"] as? Int {
                            self.offset = off_set
                        }
                        if self.appliesArray.count > 0 {
                            self.tableView.reloadData()
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
        }
    }
    
    
    @IBAction func Back(_ sender: Any) {
    self.navigationController?.popViewController(animated: true)
    }
    

}

extension JobAppliesController : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.appliesArray.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "AppliesCell") as! JobAppliesCell
        self.tableView.rowHeight = UITableView.automaticDimension
        self.tableView.estimatedRowHeight = 200.0
        let index = self.appliesArray[indexPath.row]
        if let userName = index["user_name"] as? String{
            cell.userName.text = userName
        }
        if let position = index["position"] as? String{
            cell.positionLabel.text = position
            cell.currentPosition.text = position
        }
        if let location = index["location"] as? String{
            cell.locationLabel.text = location
        }
        if let phoneNumber = index["phone_number"] as? String{
            cell.phoneNumber.text = phoneNumber
        }
        if let email = index["email"] as? String{
           cell.emailLabel.text = email
        }
        if let description = index["experience_description"] as? String{
            cell.descriptionLabel.text = description
        }
        if let startDate = index["experience_start_date"] as? String {
            cell.startDate.text = startDate
        }
        if let endDate = index["experience_end_date"] as? String{
            cell.EndDate.text = endDate
        }
        if let workPlace = index["where_did_you_work"] as? String{
            cell.work.text = workPlace
        }
        if let userData = index["user_data"] as? [String:Any]{
            if let image = userData["avatar"] as? String{
               let url = URL(string: image)
                cell.profileImage.kf.setImage(with: url)
            }
        }
        return cell
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        
        let count = self.appliesArray.count
        let lastElement = count
        
        if indexPath.row == lastElement {
            self.spinner.startAnimating()
            self.spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
            self.tableView.tableFooterView = spinner
            self.tableView.tableFooterView?.isHidden = false
            self.getApplyJob()
        }
        
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let index = self.appliesArray[indexPath.row]
        
    }
    
}


