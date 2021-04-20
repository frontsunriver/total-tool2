

import UIKit
import WoWonderTimelineSDK
import Toast_Swift

class PokeController: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    
    @IBOutlet weak var noUserView: UIView!
    
    var pokes = [[String:Any]]()
    let status = Reach().connectionStatus()
    var selectedIndex = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Pokes", comment: "Pokes")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.noUserView.isHidden = true
        self.tableView.tableFooterView = UIView()
        self.activityIndicator.startAnimating()
        self.fetchPokes()
        
    }
    
 
    private func fetchPokes(){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            GetPokeManager.sharedInstance.getPokes { (success, authError, error) in
                if success != nil {
                    self.pokes = success!.data.map({$0})
                    self.tableView.reloadData()
                    self.activityIndicator.stopAnimating()
                    if (self.pokes.count == 0){
                        self.noUserView.isHidden = false
                    }
                    else {
                        self.noUserView.isHidden = true
                        
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
    
    
    private func createPoke(user_id :String){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            CreatePokeManager.sharedInstance.createPokes(user_Id: user_id) { (success, authError, error) in
                if success != nil {
                    self.view.makeToast(success?.message_data)
                    self.pokes.remove(at: self.selectedIndex)
                    self.tableView.reloadData()
                    
                }
                else if authError != nil  {
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    print(error?.localizedDescription)
                }
                
            }
        }
        
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
}

extension PokeController: UITableViewDelegate,UITableViewDataSource{
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.pokes.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "pokeCell") as! PokeCell
        let index = self.pokes[indexPath.row]
        var first_Name = ""
        if let userData = index["user_data"] as? [String:Any]{
            if let firstName = userData["first_name"] as? String{
                first_Name = firstName
            }
            if let lastName = userData["last_name"] as? String{
                cell.profileName.text! = "\(first_Name)\(" ")\(lastName)"
            }
            if let profileIcon = userData["avatar"] as? String{
                let url = URL(string: profileIcon)
                cell.profileImage.kf.setImage(with: url)
            }
        }
        
        cell.pokeBtn.tag = indexPath.row
        cell.pokeBtn.addTarget(self, action: #selector(self.pokeBack(sender:)), for: .touchUpInside)
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    @IBAction func pokeBack(sender :UIButton){
    var index = 0
        index = sender.tag
        self.selectedIndex = index
        if let userId = self.pokes[index]["send_user_id"] as? String{
            self.createPoke(user_id: userId)
        }
    }
}
