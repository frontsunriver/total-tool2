
import UIKit
import WoWonderTimelineSDK


class LikePagesCell : UITableViewCell {
    
    
    @IBOutlet weak var pageicon: Roundimage!
    @IBOutlet weak var pageName: UILabel!
    @IBOutlet weak var pageCategory: UILabel!
    @IBOutlet weak var likeBtn: RoundButton!
    var isLike = false
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
