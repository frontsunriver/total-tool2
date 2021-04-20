

import UIKit
import XLPagerTabStrip
import WoWonderTimelineSDK
import NotificationCenter

class SearchPageController:UIViewController,IndicatorInfoProvider{

    
    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var noContentView: UIView!
    
    @IBOutlet weak var noResultLbl: UILabel!
    @IBOutlet weak var descLbl: UILabel!
    @IBOutlet weak var searchBtn: RoundButton!
    
    let status = Reach().connectionStatus()
    var pages = [[String:Any]()]
    
    var selectedIndex: Int? = nil
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        NotificationCenter.default.addObserver(self, selector: #selector(self.LoadPages(notification:)), name: NSNotification.Name(rawValue: "loadpage"), object: nil)
        self.noContentView.isHidden = false
        self.tableView.isHidden = true
        self.tableView.tableFooterView = UIView()
        self.tableView.register(UINib(nibName: "LikePagesCell", bundle: nil), forCellReuseIdentifier: "LikePage")
        self.noResultLbl.text = NSLocalizedString("Sad no result!", comment: "Sad no result!")
        self.descLbl.text = NSLocalizedString("We cannot find the keyword  you are searching from maybe a little spelling mistake ?", comment: "We cannot find the keyword  you are searching from maybe a little spelling mistake ?")
        self.searchBtn.setTitle(NSLocalizedString("Search Random", comment: "Search Random"), for: .normal)
        
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    
    override func viewWillAppear(_ animated: Bool) {
    
    }


    private func likePage(pageId :Int){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                LikePageManager.sharedInstance.likePage(pageId:pageId) { (success, authError, error) in
                    if success != nil {
                        self.view.makeToast(success?.like_status)
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        self.view.makeToast(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    @IBAction func LoadPages(notification: NSNotification){
        self.pages.removeAll()
        self.tableView.reloadData()
        if let data = notification.userInfo?["pageData"] as? [[String:Any]] {
            self.pages = data
        }
        if self.pages.count == 0{
            self.noContentView.isHidden = false
            self.tableView.isHidden = true
        }
        else {
            self.noContentView.isHidden = true
            self.tableView.isHidden = false
        }
        self.tableView.reloadData()
    }
    
    func indicatorInfo(for pagerTabStripController: PagerTabStripViewController) -> IndicatorInfo {
        return IndicatorInfo(title: NSLocalizedString("PAGES", comment: "PAGES"))
    }
    
   
    @IBAction func RandomSearch(_ sender: Any) {
     let userInfo =  ["gender": "","country":  "","verified":  "","status": "","profilePic": "" ,"filterbyage":  "","age_from": "", "age_to": "","keyword": ""] as [String : Any]
    NotificationCenter.default.post(name: NSNotification.Name(rawValue: "loadFilterData"), object: nil, userInfo: userInfo)
    let parentViewController = self.parent as! SearchController
    parentViewController.moveToViewController(at: 0)
    }
    
}
extension SearchPageController: UITableViewDataSource,UITableViewDelegate,PageLikeDelegate{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        self.pages.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let index = self.pages[indexPath.row]
        let cell = tableView.dequeueReusableCell(withIdentifier: "LikePage") as! LikePagesCell
        if let image = index["avatar"] as? String{
            let url = URL(string: image)
            cell.pageicon.kf.setImage(with: url)
        }
        if let name = index["page_name"] as? String{
            cell.pageName.text = name
        }
        if let category = index["category"] as? String{
            cell.pageCategory.text = category
        }
        if let isLike = index["is_liked"] as? String{
            if isLike == "no"{
                cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.likeBtn.backgroundColor = .white
                
            }
            else {
                cell.likeBtn.setTitle(NSLocalizedString("UnLike", comment: "UnLike"), for: .normal)
                cell.likeBtn.setTitleColor(.white, for: .normal)
                cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
            }
        }
        cell.likeBtn.tag = indexPath.row
        cell.likeBtn.addTarget(self, action: #selector(self.pageLike(sender:)), for: .touchUpInside)
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let index = self.pages[indexPath.row]
        self.selectedIndex = indexPath.row
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
       let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
        if let id = index["page_id"] as? String{
            vc.page_id = id
        }
        vc.likeDelegate = self
        self.navigationController?.pushViewController(vc, animated: true)
    }
  
    func pageLiked(isLike: Bool) {
        if isLike == true{
            self.pages[self.selectedIndex ?? 0]["is_liked"] = "yes"
        }
        else{
            self.pages[self.selectedIndex ?? 0]["is_liked"] = "no"
        }
        self.tableView.reloadData()
    }
    
    func locationSearch(location: String, countryId: String) {
        print("")
    }
    
    

    
    @IBAction func pageLike(sender: UIButton){
        let cell = self.tableView.cellForRow(at: IndexPath(row: sender.tag, section: 0)) as! LikePagesCell
        let index = self.pages[sender.tag]
        var page_id: String? = nil
        if let pageid = index["page_id"] as? String{
            page_id = pageid
        }
        if let isLike = index["is_liked"] as? String{
            if isLike == "no"{
                cell.likeBtn.setTitle(NSLocalizedString("UnLike", comment: "UnLike"), for: .normal)
                cell.likeBtn.setTitleColor(.white, for: .normal)
                cell.likeBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                self.likePage(pageId: Int(page_id ?? "") ?? 0)
                self.pages[sender.tag]["is_liked"] = "yes"
            }
            else{
                cell.likeBtn.setTitle(NSLocalizedString("Like", comment: "Like"), for: .normal)
                cell.likeBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "#984243"), for: .normal)
                cell.likeBtn.backgroundColor = .white
                self.likePage(pageId: Int(page_id ?? "") ?? 0)
                self.pages[sender.tag]["is_liked"] = "no"
            }
        }
    }
}
