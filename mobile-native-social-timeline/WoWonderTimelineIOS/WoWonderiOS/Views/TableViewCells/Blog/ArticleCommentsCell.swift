
import UIKit
import WoWonderTimelineSDK


class ArticleCommentsCell: UITableViewCell {
    
    
    @IBOutlet weak var noCommentsView: UIView!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.noCommentsView.isHidden = true
        
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
