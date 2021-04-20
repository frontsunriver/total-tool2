

import UIKit
import WoWonderTimelineSDK

class ActivitiesCollectionItem: UICollectionViewCell {

    @IBOutlet weak var changeICon: UIImageView!
    @IBOutlet weak var changeBg: UIView!
    @IBOutlet weak var profileIamge: Roundimage!
    override func awakeFromNib() {
        super.awakeFromNib()
    }
    func bind(_ object:ProUserModel.ProUser){
        let url = URL(string: object.avatar ?? "")
           self.profileIamge.kf.setImage(with: url)
        if object.proType == "1"{
            self.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "#61A470")
            self.changeICon.image = #imageLiteral(resourceName: "Stars")
        }
        else if object.proType == "2"{
            self.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "#F79B4D")
            self.changeICon.image = #imageLiteral(resourceName: "fire-1")

        }
        else if object.proType == "3"{
            self.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "#D31112")
            self.changeICon.image = #imageLiteral(resourceName: "flash-2")
        }
        else{
            self.changeBg.backgroundColor = UIColor.hexStringToUIColor(hex: "#015ECD")
            self.changeICon.image = #imageLiteral(resourceName: "rocket-1")
        }
    }

}
