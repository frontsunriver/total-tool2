

import UIKit
import XLPagerTabStrip
import WoWonderTimelineSDK

class SearchController:ButtonBarPagerTabStripViewController,UISearchBarDelegate{
    
    @IBOutlet weak var searchBar: UISearchBar!
    @IBOutlet weak var collectionView: UICollectionView!
    
    
    let Storyboard = UIStoryboard(name: "Search", bundle: nil)
    override func viewDidLoad() {
        self.setupTabbar()
//        self.collectionView.delegate = self
//        self.collectionView.dataSource = self
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        self.searchBar.delegate = self
        self.navigationController?.navigationBar.isHidden = true
        self.navigationItem.hidesBackButton = true
        self.SetUpSearchField()
        self.searchBar.tintColor = .white
        self.searchBar.backgroundColor = .clear
        self.searchBar.layer.borderColor = UIColor.hexStringToUIColor(hex: "#984243").cgColor
        self.searchBar.backgroundImage = UIImage()
        
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = true
        self.navigationItem.hidesBackButton = true
    }
    
    /// Network Connectivity
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print("Status",status)
        }
    }
    private func SetUpSearchField(){
        if let textfield = self.searchBar.value(forKey: "searchField") as? UITextField {
            textfield.clearButtonMode = .never
            textfield.backgroundColor = .clear
            textfield.attributedPlaceholder = NSAttributedString(string:"\(" ")\(NSLocalizedString("Search...", comment: "Search..."))", attributes:[NSAttributedString.Key.foregroundColor: UIColor.white])
            textfield.textColor = .white
            if let leftView = textfield.leftView as? UIImageView {
                leftView.image = leftView.image?.withRenderingMode(.alwaysTemplate)
                leftView.tintColor = UIColor.white
            }
        }
    }
    
    private func setupTabbar(){
        settings.style.buttonBarBackgroundColor = .clear
     settings.style.buttonBarItemBackgroundColor = .clear
//        UIColor.hexStringToUIColor(hex: "#702E2D")
     settings.style.selectedBarBackgroundColor = UIColor.hexStringToUIColor(hex:"#994141")
     settings.style.buttonBarItemFont = .boldSystemFont(ofSize: 14)
     settings.style.selectedBarHeight = 2.0
     settings.style.buttonBarMinimumLineSpacing = 0
     settings.style.buttonBarItemTitleColor = .white
     settings.style.buttonBarItemsShouldFillAvailableWidth = true
     settings.style.buttonBarLeftContentInset = 0
     settings.style.buttonBarRightContentInset = 0
     changeCurrentIndexProgressive = { [weak self] (oldCell: ButtonBarViewCell?, newCell: ButtonBarViewCell?, progressPercentage: CGFloat, changeCurrentIndex: Bool, animated: Bool) -> Void in
     guard changeCurrentIndex == true else { return }
        oldCell?.label.textColor = UIColor.hexStringToUIColor(hex: "#DAC2C0")
        newCell?.label.textColor = .white
     }}
    
    func changeIndex(){
        self.moveToViewController(at: 0)
    }
    
    override func viewControllers(for pagerTabStripController: PagerTabStripViewController) -> [UIViewController] {
        let child_1 = UIStoryboard(name: "Search", bundle: nil).instantiateViewController(withIdentifier: "SearchUserVC")
        let child_2 = UIStoryboard(name: "Search", bundle: nil).instantiateViewController(withIdentifier: "SearchPageVC")
        let child_3 = UIStoryboard(name: "Search", bundle: nil).instantiateViewController(withIdentifier: "SearchGroupVC")
    return [child_1,child_2,child_3]
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func Filter(_ sender: Any) {
        let vc = Storyboard.instantiateViewController(withIdentifier: "SearchFilterVC") as! SearchFilterController
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        self.present(vc, animated: true, completion: nil)
    }
    
    func searchBarSearchButtonClicked(_ searchBar: UISearchBar) {
        let userInfo =  ["gender": "","country": "","verified": "","status": "","profilePic": "","filterbyage": "","age_from": "", "age_to": "","keyword": self.searchBar.text!] as [String : Any]
        self.searchBar.resignFirstResponder()
        NotificationCenter.default.post(name: NSNotification.Name(rawValue: "loadFilterData"), object: nil, userInfo: userInfo)
    }
    
    
}

