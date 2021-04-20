

import UIKit
import WoWonderTimelineSDK


class MentionUserTableItem: UITableViewCell {

    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(_ object: UserListModel.Follow){
        self.usernameLabel.text = "\(object.firstName ?? "") \(object.lastName ?? "")"
        let url = URL(string: object.avatar ?? "")
                 self.profileImage.kf.setImage(with: url)
    }
    
}
