

import UIKit
import WoWonderTimelineSDK

class ArticleCell: UITableViewCell{
    @IBOutlet weak var thumbnailImage: UIImageView!
    @IBOutlet weak var BlogTitle: UILabel!
    @IBOutlet weak var profileIcon: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var postTime: UILabel!
    @IBOutlet weak var articleDescrip: UILabel!
    @IBOutlet weak var gotoProfile: UIButton!
    @IBOutlet weak var categoryLabel: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
