import WoWonderTimelineSDK
import UIKit
import Kingfisher

class ReactionCells: UICollectionViewCell {
    
    @IBOutlet weak var tableView: UITableView!
    
    var reactions = [[String:Any]]()
    
    override func awakeFromNib() {
        self.tableView.delegate = self
        self.tableView.dataSource = self
        self.tableView.backgroundColor = .white
        self.tableView.tableFooterView = UIView()
    }
    
}
extension ReactionCells : UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        self.reactions.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "Postreactioncell") as! PostReactionCell
        let index = self.reactions[indexPath.row]
           if let name = index["username"] as? String{
                cell.profileName.text = name
            }
        if let lastSeen = index["lastseen_time_text"] as? String{
            cell.lastSeen.text = "\("Last seen ")\(lastSeen)"
        }
        if let proImage = index["avatar"] as? String{
            let url = URL(string: proImage)
            cell.profileImage.kf.setImage(with: url)
            
        }
            return cell
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 80.0
    }
    

    
    
}
