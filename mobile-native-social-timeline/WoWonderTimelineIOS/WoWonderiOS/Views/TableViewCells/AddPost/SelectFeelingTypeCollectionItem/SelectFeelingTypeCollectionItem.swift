

import UIKit
import WoWonderTimelineSDK


class SelectFeelingTypeCollectionItem: UICollectionViewCell {
    @IBOutlet weak var imageIcon: UIImageView!

    @IBOutlet weak var titleLabel: UILabel!
    override func awakeFromNib() {
        super.awakeFromNib()
        
       
    }
    func bind(_ object:FeelingType){
        let url = URL(string: object.feelingImage ?? "")
                        self.imageIcon.kf.setImage(with: url)
        self.titleLabel.text = object.feelingName ?? ""
    }

}
