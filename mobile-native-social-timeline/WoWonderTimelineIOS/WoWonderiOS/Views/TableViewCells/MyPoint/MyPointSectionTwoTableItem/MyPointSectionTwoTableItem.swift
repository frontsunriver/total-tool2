

import UIKit
import WoWonderTimelineSDK


class MyPointSectionTwoTableItem: UITableViewCell {

    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var iconImage: UIImageView!
    @IBOutlet weak var backGroundColor: RoundButton!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(_ object:dataSetMyPoints){
        self.backGroundColor.backgroundColor = object.BGColor!
        self.titleLabel.text = object.title ?? ""
        self.iconImage.image = object.iconImage ?? UIImage()
    }
    
}
