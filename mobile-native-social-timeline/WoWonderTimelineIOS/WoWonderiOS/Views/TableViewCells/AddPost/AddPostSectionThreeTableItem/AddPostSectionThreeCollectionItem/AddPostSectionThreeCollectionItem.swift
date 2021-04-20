

import UIKit
import WebKit
import WoWonderTimelineSDK

class AddPostSectionThreeCollectionItem: UICollectionViewCell {

 @IBOutlet weak var itemImageView: UIImageView!
    @IBOutlet weak var removeBtn: UIButton!
    @IBOutlet weak var itemWebView: WKWebView!
    
    var bindingType:String? = ""
    var vc:AddPostSectionThreeTableItem?
    var addPostVC:AddPostVC?
    var index:Int? = 0
    override func awakeFromNib() {
        super.awakeFromNib()
    }
    
    override func prepareForReuse() {
        super.prepareForReuse()
        
    }
    
    @IBAction func crossPressed(_ sender: Any) {
       self.vc?.isEmptyMedia = true
        
        if bindingType == "IMAGE"{
            self.vc?.imageArray.remove(at: self.index ?? 0)
            if self.vc?.imageArray.count  == 0{
                AppInstance.instance.isAlbumVisible = false

            }else{
                AppInstance.instance.isAlbumVisible = true

            }
            self.addPostVC?.tableView.reloadData()
            self.vc?.collectionView.reloadData()

        }else if  bindingType == "GIF"{
            self.vc?.GifsArray.remove(at: self.index ?? 0)
            self.vc?.collectionView.reloadData()
            
        } else if bindingType == "VIDEO"{
            self.vc?.thumbnailiamgeArray.remove(at: self.index ?? 0)
             self.vc?.videoArray.remove(at: self.index ?? 0)
            self.vc?.collectionView.reloadData()
        }
    }
  
    func imageBinding(image : UIImage,row:Int,Type:String){
        self.index = row
         self.bindingType = Type
            self.itemWebView.isHidden = true
            self.itemImageView.image = image
        self.bringSubviewToFront(self.removeBtn)
    }
    func videoBinding(videoThumb:UIImage,row:Int,Type:String){
        self.index = row
         self.bindingType = Type
        self.itemWebView.isHidden = true
        self.itemImageView.image = videoThumb
        
    }
    func gifBinding(gifURL:String,row:Int,Type:String){
        self.index = row
        self.bindingType = Type
        self.itemWebView.isHidden = false
        self.itemImageView.isHidden = true
        let htmlString = "<html style=\"margin: 0;\"><body style=\"margin: 0;\"><img src=\"\(gifURL)\" style=\"width: 100%; height: 100%\" /></body></html>"
        self.itemWebView.loadHTMLString(htmlString, baseURL: nil)
    }

}
