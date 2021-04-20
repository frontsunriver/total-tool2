

import UIKit
import FontAwesome_swift
import WoWonderTimelineSDK

struct dataSet{
    var image:UIImage?
    var title:String?
}
class SocialLinkVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    var dataArray = [dataSet]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        self.dataArray =  [
            dataSet(image:  #imageLiteral(resourceName: "facebook"), title: "Facebook"),
            dataSet(image: #imageLiteral(resourceName: "twitter"), title: "Twitter"),
            dataSet(image: #imageLiteral(resourceName: "Group-1"), title: "Google+"),
            dataSet(image: #imageLiteral(resourceName: "vk"), title: "Vkontakte"),
            dataSet(image: #imageLiteral(resourceName: "linkedin"), title: "Linkedin"),
            dataSet(image:  #imageLiteral(resourceName: "instagram"), title: "Instagram"),
            dataSet(image: #imageLiteral(resourceName: "youtube"), title: "Youtube")
        ]
    }
    
    private func setupUI(){
    
        self.tableView.separatorStyle = .none
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Social Link", comment: "Social Link")
        
        tableView.register(UINib(nibName: "SocialLinksTableItem", bundle: nil), forCellReuseIdentifier: "SocialLinksTableItem")
    }
    
}
extension SocialLinkVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        
        return 50.0
        
    }
}


extension SocialLinkVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.dataArray.count ?? 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "SocialLinksTableItem") as! SocialLinksTableItem
        let object = self.dataArray[indexPath.row]
        cell.bind(object)
        
        return cell
        
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
          let storyboard = UIStoryboard(name: "General", bundle: nil)
              let vc = storyboard.instantiateViewController(withIdentifier: "UpdateSocialLinksVC") as! UpdateSocialLinksVC
        vc.titleString = self.dataArray[indexPath.row].title ?? ""
              self.present(vc, animated: true, completion: nil)
    }
}
