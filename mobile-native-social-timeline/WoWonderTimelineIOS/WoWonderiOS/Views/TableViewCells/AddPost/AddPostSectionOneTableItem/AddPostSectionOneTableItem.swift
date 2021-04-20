

import UIKit
import ActionSheetPicker_3_0
import SDWebImage
import WoWonderTimelineSDK

class AddPostSectionOneTableItem: UITableViewCell {
    
    @IBOutlet weak var albumBtn: RoundButton!
    @IBOutlet weak var privacyBtn: RoundButton!
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var profileImage: Roundimage!
    
    var vc:AddPostVC?
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
    func bind(){
        self.usernameLabel.text = AppInstance.instance.profile?.userData?.username ?? ""
        let url = URL(string: UserData.getImage()  ?? "")
        self.profileImage.sd_setImage(with: url, placeholderImage: #imageLiteral(resourceName: "no-avatar"), options: [], completed: nil)
        if AppInstance.instance.isAlbumVisible{
            self.albumBtn.isHidden = false
        }else{
            self.albumBtn.isHidden = true
        }
    }
    
    @IBAction func privacyPressed(_ sender: UIButton) {
        
        
        ActionSheetStringPicker.show(withTitle: NSLocalizedString("Post Privacy", comment: ""),
                                     rows: ["Everyone", "People i Follow","People Follow me","Nobody"],
                                     initialSelection: 0,
                                     doneBlock: { (picker, value, index) in
                                        
                                        self.privacyBtn.setTitle(index as? String, for: .normal)
                                        self.vc?.postPrivacy = value
                                        return
                                        
        }, cancel:  { ActionStringCancelBlock in return }, origin:sender)
    }
    @IBAction func albumPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "AlbumNameVC") as! AlbumNameVC
        vc.delegate = self
        self.vc!.present(vc, animated: true, completion: nil)
    }
    
    
}
extension AddPostSectionOneTableItem:didSelectAlbumNameDelegate{
    func didSelectAlbumName(albumNameString: String) {
        self.albumBtn.setTitle(albumNameString, for: .normal)
    }
}
