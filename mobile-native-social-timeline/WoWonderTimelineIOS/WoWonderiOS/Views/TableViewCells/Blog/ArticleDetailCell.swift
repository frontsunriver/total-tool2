
import UIKit
import WebKit
import WoWonderTimelineSDK
import ActiveLabel


class ArticleDetailCell: UITableViewCell {

    @IBOutlet weak var blogTitle: UILabel!
    @IBOutlet weak var blogImage: UIImageView!
    @IBOutlet weak var blogDescription: ActiveLabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}
