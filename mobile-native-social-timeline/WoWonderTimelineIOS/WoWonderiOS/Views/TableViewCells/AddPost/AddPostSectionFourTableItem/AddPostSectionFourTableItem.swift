

import UIKit
import WoWonderTimelineSDK

class AddPostSectionFourTableItem: UITableViewCell {

    @IBOutlet weak var imageIcon: UIImageView!
   
    @IBOutlet weak var titLabel: UILabel!
    override func awakeFromNib() {
        super.awakeFromNib()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
    }
    func bind(imageType:String,URLString:String){
        if imageType == "FILE"{
            self.imageIcon.image = UIImage(named: "documents")
        }else{
             self.imageIcon.image = UIImage(named: "music")
            
        }
        self.titLabel.text = URLString
    }
    
}
