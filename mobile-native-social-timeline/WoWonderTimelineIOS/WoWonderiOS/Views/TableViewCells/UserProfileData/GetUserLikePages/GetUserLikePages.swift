
import UIKit
import WoWonderTimelineSDK


class GetUserLikePages: UITableViewCell {

    @IBOutlet weak var pageiconImage: UIImageView!
    
    @IBOutlet weak var pageLbl: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.pageLbl.text = NSLocalizedString("Pages", comment: "Pages")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
