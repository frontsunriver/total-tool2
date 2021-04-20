import WoWonderTimelineSDK
import UIKit
import SDWebImage

class CollectionViewCell: UICollectionViewCell {
    
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var usernameLabel: UILabel!
    
    func bind(_ object:GetStoriesModel.UserDataElement?,section:Int?){
        if section == 1{
            let url = URL(string: object?.stories?[0].thumbnail ?? "")
                   self.profileImage.kf.setImage(with: url)
                   self.usernameLabel.text = object?.username ?? ""
        }else{
            let url = URL(string: UserData.getImage()  ?? "")
            self.profileImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "no-avatar"), options: [], completed: nil)
            self.usernameLabel.text = UserData.getUSER_NAME() ?? ""
        }
    }
}
