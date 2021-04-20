

import UIKit
import WoWonderTimelineSDK


class NotificationsTableItem: UITableViewCell {
    @IBOutlet weak var profileImage: UIImageView!
    
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var changeIcon: UIImageView!
    @IBOutlet weak var changeBg: UIView!
    override func awakeFromNib() {
        super.awakeFromNib()
        self.changeBg.layer.cornerRadius = self.changeBg.frame.height / 2
        self.selectionStyle = .none
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    
    func bind(_ object:GetNotificationModel.Notification){
        self.titleLabel.text = object.notifier?.username ?? ""
        self.descriptionLabel.text = object.typeText ?? ""
        let url = URL(string: object.notifier?.avatar ?? "")
        self.profileImage.kf.setImage(with: url)
        if object.icon == "users"{
            self.changeBg.backgroundColor = .blue
            self.changeIcon.image = UIImage(named: "tick")
        }else if object.icon == "user-plus"{
            self.changeBg.backgroundColor = .systemPink
            self.changeIcon.image = UIImage(named: "addusers")
            
        }else if object.icon == "like"{
            self.changeBg.backgroundColor = .brown
            self.changeIcon.image = UIImage(named: "like-2")
        }
    }
    
}
