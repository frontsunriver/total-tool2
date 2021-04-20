
import UIKit
import WoWonderTimelineSDK


class SocialLinksTableItem: UITableViewCell {

    @IBOutlet weak var changeImage: UIImageView!
    
    @IBOutlet weak var titleLabel: UILabel!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(_ object:dataSet){
        self.changeImage.image = object.image ?? UIImage()
        self.titleLabel.text = object.title ?? ""
    }
}
