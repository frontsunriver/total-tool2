
import UIKit
import WoWonderTimelineSDK

class GeneralVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
//        self.title = "General"
        self.setupUI()
    }
    
    private func setupUI(){
        self.tableView.separatorStyle = .none
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("General", comment: "General")
        
        tableView.register(UINib(nibName: "TellFriendTableItem", bundle: nil), forCellReuseIdentifier: "TellFriendTableItem")
        tableView.register(UINib(nibName: "HelpSupportTableItem", bundle: nil), forCellReuseIdentifier: "HelpSupportTableItem")
        
        
    }
}
extension GeneralVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 50.0
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        switch indexPath.section {
        case 0:
            switch indexPath.row {
            case 0:
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "EditProfileVC") as! EditProfileVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 1:
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "UpdateAboutMeVC") as! UpdateAboutMeVC
                self.present(vc, animated: true, completion: nil)
            case 2:
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "MyAccountVC") as! MyAccountVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 3:
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "SocialLinkVC") as! SocialLinkVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 4:
                
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "BlockedUsersVC") as! BlockedUsersVC
                self.navigationController?.pushViewController(vc, animated: true)
            default:
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "BlockedUsersVC") as! BlockedUsersVC
                self.navigationController?.pushViewController(vc, animated: true)
            }
//        case 1:
//
//            let storyboard = UIStoryboard(name: "General", bundle: nil)
//            let vc = storyboard.instantiateViewController(withIdentifier: "ManageSessionVC") as! ManageSessionVC
//            self.navigationController?.pushViewController(vc, animated: true)
//
            
        case 1:
            switch indexPath.row {
            case 0:
                
                
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "ChangePasswordVC") as! ChangePasswordVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 1:
                
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "TwoFactorAuthenticationVC") as! TwoFactorAuthenticationVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 2:
                
                
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "ManageSessionVC") as! ManageSessionVC
                self.navigationController?.pushViewController(vc, animated: true)
            case 3:
                
               let storyboard = UIStoryboard(name: "General", bundle: nil)
                               let vc = storyboard.instantiateViewController(withIdentifier: "DeleteAccountVc") as! DeleteAccountVc
               self.present(vc, animated: true, completion: nil)
                
                
                
            default:
                let storyboard = UIStoryboard(name: "General", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "ManageSessionVC") as! ManageSessionVC
                self.navigationController?.pushViewController(vc, animated: true)
            }
            
        default:
            let storyboard = UIStoryboard(name: "General", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "ManageSessionVC") as! ManageSessionVC
            self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    
    func tableView(_ tableView: UITableView, viewForHeaderInSection section: Int) -> UIView? {
        
        let view = UIView(frame: CGRect(x: 0, y: 0, width: tableView.frame.size.width, height: 56))
        view.backgroundColor = UIColor.white
        
        let separatorView = UIView(frame: CGRect(x: 0, y: 0, width: view.frame.size.width, height: 8))
        separatorView.backgroundColor = UIColor(red: 220/255, green: 220/255, blue: 220/255, alpha: 1.0)
        
        let label = UILabel(frame: CGRect(x: 16, y: 8, width: view.frame.size.width, height: 48))
        label.textColor = UIColor.hexStringToUIColor(hex: "984243")
        label.font = UIFont(name: "Arial", size: 17)
        if section == 0{
            label.text = NSLocalizedString("General", comment: "General")
            
        }
//        else if section == 1 {
//            label.text = "Mode"
//        }
        else if section == 1 {
            label.text = NSLocalizedString("Security", comment: "Security")
        }
        view.addSubview(separatorView)
        view.addSubview(label)
        return view
        
    }
    
    func tableView(_ tableView: UITableView, heightForHeaderInSection section: Int) -> CGFloat {
        return 56
    }
}

extension GeneralVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 2
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        
        switch section {
        case 0: return 5
//        case 1: return 1
        case 1: return 4
            
        default: return 0
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        
        switch indexPath.section {
        case 0:
            switch indexPath.row {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Edit Profile", comment: "Edit Profile")
                
                return cell
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "TellFriendTableItem") as! TellFriendTableItem
                cell.titleLabel.text = NSLocalizedString("About", comment: "About")
                cell.descriptionLabel.text = "Hi there  i am using WoWonder Timeline"
                return cell
            case 2:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("My Account", comment: "My Account")
                
                return cell
            case 3:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Social Links", comment: "Social Links")
                
                return cell
            case 4:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Blocked Users", comment: "Blocked Users")
                
                return cell
                
            default:
                return UITableViewCell()
            }
//        case 1:
//            let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
//            cell.titleLabel.text = "Night Mode"
//
//            return cell
            
        case 1:
            switch indexPath.row {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "TellFriendTableItem") as! TellFriendTableItem
                cell.titleLabel.text = NSLocalizedString("Password", comment: "Password")
                cell.descriptionLabel.text = NSLocalizedString("Change your Password", comment: "Change your Password")
                return cell
                
                
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Two-factor Authentication", comment: "Two-factor Authentication")
                return cell
                
            case 2:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Manage Sessions", comment: "Manage Sessions")
                
                return cell
            case 3:
                let cell = tableView.dequeueReusableCell(withIdentifier: "HelpSupportTableItem") as! HelpSupportTableItem
                cell.titleLabel.text = NSLocalizedString("Delete Account", comment: "Delete Account")
                
                return cell
            default:
                return UITableViewCell()
            }
            
        default:
            return UITableViewCell()
        }
    }
}
