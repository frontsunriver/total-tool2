

import UIKit
import WoWonderTimelineSDK


class ActivitiesSectionTwoTableItem: UITableViewCell {

    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var changeIcon: UIImageView!
    @IBOutlet weak var profileImage: Roundimage!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(_ object:LastActivitiesModel.Activity){
        let url = URL(string: object.activator?.avatar ?? "")
               self.profileImage.kf.setImage(with: url)
        self.titleLabel.text = object.activityText
        self.descriptionLabel.text = "8 hours"
    }
    
}
