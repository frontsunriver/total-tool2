

import UIKit
import WoWonderTimelineSDK


class BlockedUsersTableItem: UITableViewCell {

    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    override func awakeFromNib() {
        super.awakeFromNib()
        self.selectionStyle = .none
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
    
    func bind(_ object:[String:Any],index:Int){
        if let name = object["name"] as? String{
            self.usernameLabel.text = name
        }
        if let lastSeen = object["lastseen_time_text"] as? String{
            self.timeLabel.text = "\("Last seen ")\(lastSeen)"
        }
        if let avatar = object["avatar"] as? String{
            let url = URL(string: avatar)
             self.profileImage.kf.setImage(with: url)
        }
       }
}
