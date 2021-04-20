

import UIKit
import SDWebImage
import WoWonderTimelineSDK


class PostStatusCell: UITableViewCell {
    
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var moreLabel: UILabel!
    @IBOutlet weak var photoLabel: UILabel!
    @IBOutlet weak var statusLabel: UILabel!
    @IBOutlet weak var photoBtn: UIButton!
    @IBOutlet weak var moreBtn: UIButton!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.statusLabel.text = NSLocalizedString("What's going on?#Hashtag..@Mention", comment: "What's going on?#Hashtag..@Mention")
        self.photoLabel.text = NSLocalizedString("Photos", comment: "Photos")
        self.moreLabel.text = NSLocalizedString("More", comment: "More")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        
    }
    func bind(){

        let url = URL(string: UserData.getImage() ?? "")
        print(url)
//        self.profileImage.kf.setImage(with: url)
        self.profileImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "no-avatar"), options: [], completed: nil)
    }

}
