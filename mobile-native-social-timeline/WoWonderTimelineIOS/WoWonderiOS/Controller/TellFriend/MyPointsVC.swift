

import UIKit
import WoWonderTimelineSDK
import FontAwesome_swift
struct dataSetMyPoints {
    var   BGColor:UIColor?
    var iconImage:UIImage?
    var title:String?
}

class MyPointsVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    var dataSet = [dataSetMyPoints]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.title = NSLocalizedString("My Points", comment: "My Points")
        
        
        self.setupUI()
        
    }
    
    private func setupUI(){
        self.tableView.separatorStyle = .none
        
        tableView.register(UINib(nibName: "MyPointSecionOneTableItem", bundle: nil), forCellReuseIdentifier: "MyPointSecionOneTableItem")
        tableView.register(UINib(nibName: "MyPointSectionTwoTableItem", bundle: nil), forCellReuseIdentifier: "MyPointSectionTwoTableItem")
        tableView.register(UINib(nibName: "MyPointSectionThreeTableItem", bundle: nil), forCellReuseIdentifier: "MyPointSectionThreeTableItem")
        self.dataSet = [
            dataSetMyPoints(BGColor: .green, iconImage: UIImage.fontAwesomeIcon(name: .comment, style: .regular, textColor: UIColor.white, size: CGSize(width: 40, height: 40)) , title: NSLocalizedString("Earn 10 by commenting any post", comment: "Earn 10 by commenting any post")),
            dataSetMyPoints(BGColor: .blue, iconImage: UIImage.fontAwesomeIcon(name: .newspaper, style: .regular, textColor: UIColor.white, size: CGSize(width: 40, height: 40)) , title: NSLocalizedString("Earn 20 by creating new post", comment: "Earn 20 by creating new post")),
            dataSetMyPoints(BGColor: .orange, iconImage: UIImage.fontAwesomeIcon(name: .smile, style: .regular, textColor: UIColor.white, size: CGSize(width: 40, height: 40)) , title: NSLocalizedString("Earn 5 by reacting on any post", comment: "Earn 5 by reacting on any post")),
            dataSetMyPoints(BGColor: .gray, iconImage: UIImage.fontAwesomeIcon(name: .grinBeam, style: .regular, textColor: UIColor.white, size: CGSize(width: 40, height: 40)) , title: NSLocalizedString("Earn 15 by creating a new blog", comment: "Earn 15 by creating a new blog"))
        ]
        
    }
    
}

extension MyPointsVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        switch indexPath.section {
        case 0:
            return 200.0
        case 1,2:
            return 80.0
            
        default:
            return 50.0
        }
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        let storyboard = UIStoryboard(name: "HelpSupport", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "HelpWebViewVC") as! HelpWebViewVC
        
        if indexPath.section == 0 {
            if indexPath.row == 0 {
                vc.URLString = "https://demo.wowonder.com/contact-us"
                vc.title = "help Center"
            } else if indexPath.row == 1 {
                vc.URLString = "https://demo.wowonder.com/contact-us"
                vc.title = "Report a Problem"
            }
        }else if indexPath.section == 1{
            switch indexPath.row {
            case 0:
                vc.URLString = "https://demo.wowonder.com/terms/about-us"
                vc.title = "About Us"
            case 1:
                vc.URLString = "https://demo.wowonder.com/terms/privacy-policy"
                vc.title = "Privacy Policy"
            case 2:
                vc.URLString = "https://demo.wowonder.com/terms/terms"
                vc.title = "Terms of Service"
            default:
                break
            }
            
        }
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
    
}

extension MyPointsVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        return 3
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        
        switch section {
        case 0: return 1
        case 1: return self.dataSet.count ?? 0
        case 2: return 1
        default: return 0
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        switch indexPath.section {
        case 0:
            
            let cell = tableView.dequeueReusableCell(withIdentifier: "MyPointSecionOneTableItem") as! MyPointSecionOneTableItem
            cell.bind()
            
            return cell
            
        case 1:
            
            let cell = tableView.dequeueReusableCell(withIdentifier: "MyPointSectionTwoTableItem") as! MyPointSectionTwoTableItem
            let object = self.dataSet[indexPath.row]
            cell.bind(object)
            
            return cell
        case 2:
            
            let cell = tableView.dequeueReusableCell(withIdentifier: "MyPointSectionThreeTableItem") as! MyPointSectionThreeTableItem
            cell.vc = self
            return cell
            
            
        default:
            return UITableViewCell()
        }
    }
}
