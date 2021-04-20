

import UIKit
import WoWonderTimelineSDK

class ShowFundingDetailsVC: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    
    var object:GetFundingModel.Datum?
    var recentDonation = [[String:Any]]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.isHidden = true
        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = false
        self.tableView.register(UINib(nibName: "UserDonationCell", bundle: nil), forCellReuseIdentifier: "UserDonationCells")
        self.getFundsDonations()
    }

    override func viewWillAppear(_ animated: Bool) {
           super.viewWillAppear(animated)
           self.setupUI()
       }
    
    
    @IBAction func addFundingPressed(_ sender: Any) {
    }
    
    
       private func setupUI(){
           
           self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
           
           let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
           navigationController?.navigationBar.titleTextAttributes = textAttributes
           self.navigationItem.title = NSLocalizedString("Fundings", comment: "Fundings")
           
           self.tableView.separatorStyle = .none
           tableView.register(UINib(nibName: "FundingDetailsSectionOneTableItem", bundle: nil), forCellReuseIdentifier: "FundingDetailsSectionOneTableItem")
    }
    
    private func getFundsDonations() {
        GetFundingDonationsManager.sharedInstance.getDonations(fundId: self.object?.id ?? 0) { (success, authError, error) in
            if (success != nil){
                for i in success!.data{
                    self.recentDonation.append(i)
                }
                self.tableView.reloadData()
            }
            else if (authError != nil){
                self.view.makeToast(authError?.errors?.errorText)
            }
            else if (error != nil){
                self.view.makeToast(error?.localizedDescription)
            }
        }
    }
    
}
extension ShowFundingDetailsVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if (indexPath.section == 0){
            return 670.0
        }
        else{
            return 70.0
        }
    }
}


extension ShowFundingDetailsVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 2
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if section == 0{
            return 1
        }
        else{
            return self.recentDonation.count
        }
     
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if indexPath.section == 1{
            let storyBoard = UIStoryboard(name: "Main", bundle: nil)
            let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
            let index = self.recentDonation[indexPath.row]
            
            if let user_data = index["user_data"] as? [String:Any]{
                 vc.userData = user_data
                if let id = user_data["user_id"] as? String{
                    vc.user_id = id
                    
                }
            }
            self.navigationController?.pushViewController(vc, animated: true)
        }
        else{
            print("Nothing")
        }

    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if (indexPath.section == 0){
              let cell = tableView.dequeueReusableCell(withIdentifier: "FundingDetailsSectionOneTableItem") as! FundingDetailsSectionOneTableItem
            
              cell.vc = self
              cell.bind(object!,index:indexPath.row)
              cell.backBtn.addTarget(self, action: #selector(self.Back(sender:)), for: .touchUpInside)
              return cell
        }
        else{
            let cell = tableView.dequeueReusableCell(withIdentifier: "UserDonationCells") as! UserDonationCell
            let index = self.recentDonation[indexPath.row]
            cell.bind(index: index)
            cell.vc = self
            return cell
        }

    }
    //    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
    //
    //        let storyboard = UIStoryboard(name: "General", bundle: nil)
    //        let vc = storyboard.instantiateViewController(withIdentifier: "UpdateBlockUnBlockVC") as! UpdateBlockUnBlockVC
    //        vc.object = self.blockedUserArray[indexPath.row]
    //        self.present(vc, animated: true, completion: nil)
    //    }
    @IBAction func Back(sender: UIButton){
        self.navigationController?.popViewController(animated: true)
        self.navigationController?.navigationBar.isHidden = false
    }
}
