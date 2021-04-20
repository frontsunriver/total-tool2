

import UIKit
import WoWonderTimelineSDK
class HelpAndSupportVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.largeTitleDisplayMode = .never
        self.title = NSLocalizedString("Help and Support", comment: "Help and Support")
        self.setupUI()
    }
    
//    override func viewWillAppear(_ animated: Bool) {
//        super.viewWillAppear(animated)
//        self.navigationController?.isNavigationBarHidden = false
//        self.tabBarController?.tabBar.isHidden = true
//    }
//    override func viewWillDisappear(_ animated: Bool) {
//        super.viewWillDisappear(animated)
//        self.navigationController?.isNavigationBarHidden = true
//        self.tabBarController?.tabBar.isHidden = false
//    }
    
    private func setupUI(){
        self.tableView.separatorStyle = .none
        
        tableView.register(UINib(nibName: "HelpSupportTableItem", bundle: nil), forCellReuseIdentifier: "HelpSupportTableItem")
        
    }
    
}

extension HelpAndSupportVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        
        return 50.0
        
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
    
    func tableView(_ tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        
        let view = UIView(frame: CGRect(x: 0, y: 0, width: tableView.frame.size.width, height: 56))
        view.backgroundColor = UIColor.white
        
        let separatorView = UIView(frame: CGRect(x: 0, y: 0, width: view.frame.size.width, height: 8))
        separatorView.backgroundColor = UIColor(red: 220/255, green: 220/255, blue: 220/255, alpha: 1.0)
        
        let label = UILabel(frame: CGRect(x: 16, y: 8, width: view.frame.size.width, height: 48))
        label.textColor = UIColor.hexStringToUIColor(hex: "984243")
        label.font = UIFont(name: "ChalkboardSE-Light", size: 17)
        if section == 0{
            label.text = NSLocalizedString("Help", comment: "Help")
            
        } else if section == 1 {
            label.text = NSLocalizedString("About", comment: "About")
        }
        view.addSubview(separatorView)
        view.addSubview(label)
        return view
        
    }
    
    func tableView(_ tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        return 56
    }
}

extension HelpAndSupportVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 2
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        
        switch section {
        case 0: return 2
        case 1: return 3
        default: return 0
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        switch indexPath.section {
        case 0:
            switch indexPath.row {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Help Center", comment: "Help Center")
                
                return cell
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Report a Problem", comment: "Report a Problem")
                return cell
                
            default:
                return UITableViewCell()
            }
        case 1:
            switch indexPath.row{
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("About Us", comment: "About Us")
                return cell
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Privacy Policy", comment: "Privacy Policy")
                return cell
            case 2:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Term Of service", comment: "Term Of service")
                return cell
            default:
                return UITableViewCell()
            }
            
        default:
            return UITableViewCell()
        }
    }
}
