

import UIKit
import SDWebImage
import WoWonderTimelineSDK


class AddPostCell: UITableViewCell {

    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var photoBtn: UIButton!
    @IBOutlet weak var moreBtn: UIButton!
    var is_group = false
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    func bind(page_data: [String:Any]){

        if let avatar = page_data["avatar"] as? String{
            let url = URL(string: avatar ?? "")
            if self.is_group{
              self.profileImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "GroupIcons-1"), options: [], completed: nil)
            }
            else{
            self.profileImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "d-page"), options: [], completed: nil)
            }
        }
    }
}
