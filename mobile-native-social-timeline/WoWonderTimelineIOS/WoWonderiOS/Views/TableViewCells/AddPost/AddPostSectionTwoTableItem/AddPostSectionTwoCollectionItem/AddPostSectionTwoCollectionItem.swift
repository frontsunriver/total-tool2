
import UIKit

import WoWonderTimelineSDK

class AddPostSectionTwoCollectionItem: UICollectionViewCell {
    
    @IBOutlet weak var profileImage: Roundimage!
    override func awakeFromNib() {
        super.awakeFromNib()
        
    }
    func bind(_ object:String){
        let url = URL(string: object )
        self.profileImage.kf.setImage(with: url)
    }
    
}
