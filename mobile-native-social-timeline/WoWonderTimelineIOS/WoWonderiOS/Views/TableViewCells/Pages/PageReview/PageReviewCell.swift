

import UIKit
import WoWonderTimelineSDK


class PageReviewCell: UITableViewCell {
    
    @IBOutlet weak var userImage: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var ratingLabel: UILabel!
    @IBOutlet weak var ReviewLabel: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    
}
