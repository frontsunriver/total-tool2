

import UIKit
import WoWonderTimelineSDK


class MyPointSecionOneTableItem: UITableViewCell {

    @IBOutlet weak var pointLabel: UILabel!
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var profileImage: UIImageView!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(){
           let url = URL(string: AppInstance.instance.profile?.userData?.avatar ?? "")
                         self.profileImage.kf.setImage(with: url)
        self.usernameLabel.text  = AppInstance.instance.profile?.userData?.username ?? ""
        self.pointLabel.text = AppInstance.instance.profile?.userData?.points ?? ""
       }
    
}
