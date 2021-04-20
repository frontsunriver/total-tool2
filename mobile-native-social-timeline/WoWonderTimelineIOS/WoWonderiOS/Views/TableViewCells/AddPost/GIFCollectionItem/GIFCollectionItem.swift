

import UIKit
import WoWonderTimelineSDK

import WebKit
class GIFCollectionItem: UICollectionViewCell {
    @IBOutlet weak var contentWebView: WKWebView!
    @IBOutlet weak var typeImgView: UIImageView!
    override func awakeFromNib() {
        super.awakeFromNib()
    }
    func bindGif(item:GIFModel.Datum,indexPath:IndexPath){
        self.typeImgView.image = UIImage(named: "ic_type_gif")
        
        DispatchQueue.main.async {
            let htmlString = "<html style=\"margin: 0;\"><body style=\"margin: 0;\"><img src=\"\(item.images?.fixedHeightStill?.url ?? "")\" style=\"width: 100%; height: 100%\" /></body></html>"
            self.contentWebView.loadHTMLString(htmlString, baseURL: nil)
        }
    }
}
