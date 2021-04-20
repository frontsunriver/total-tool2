
import UIKit
import WoWonderTimelineSDK


class SelectPostTypeTableItem: UITableViewCell {
    
    @IBOutlet weak var iconImgView: UIImageView!
       @IBOutlet weak var titleLbl: UILabel!
       
    override func awakeFromNib() {
        super.awakeFromNib()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(image:UIImage,title:String){
        self.iconImgView.image = image
        self.titleLbl.text = title
    }
    
}
