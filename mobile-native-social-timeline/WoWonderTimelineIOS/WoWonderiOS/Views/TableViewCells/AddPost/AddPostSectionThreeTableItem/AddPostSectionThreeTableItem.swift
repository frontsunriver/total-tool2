
import UIKit
import WoWonderTimelineSDK

class AddPostSectionThreeTableItem: UITableViewCell {
    
    @IBOutlet weak var collectionView: UICollectionView!
    
    var imageArray = [UIImage]()
    var videoArray = [String]()
    var GifsArray = [ String]()
    var thumbnailiamgeArray = [UIImage]()
    var VideoData:Data? = nil
    var thumbData:Data? = nil
    var isEmptyMedia:Bool? = false
    var gifURLString:String? = ""

    var vc:AddPostVC?
    override func awakeFromNib() {
        super.awakeFromNib()
        
        collectionView.delegate = self
        collectionView.dataSource = self
        collectionView.register(UINib(nibName: "AddPostSectionThreeCollectionItem", bundle: nil), forCellWithReuseIdentifier: "AddPostSectionThreeCollectionItem")
        
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
    }
    
}
extension AddPostSectionThreeTableItem : UICollectionViewDelegate , UICollectionViewDataSource, UICollectionViewDelegateFlowLayout {
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        
        if self.vc!.type == "IMAGE"{
            return self.imageArray.count
        } else if self.vc!.type == "VIDEO"{
            return self.videoArray.count
        }else if self.vc!.type == "GIF"{
            return self.GifsArray.count
        }
        
        return 0
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        //        self.currentIndexPath = indexPath
        
        
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "AddPostSectionThreeCollectionItem", for: indexPath) as! AddPostSectionThreeCollectionItem
        cell.vc = self
        cell.addPostVC = vc 
        
        if self.vc!.type == "GIF" {
            cell.gifBinding(gifURL: self.GifsArray[indexPath.row], row: indexPath.row, Type:  self.vc!.type ?? "")
            
        }else if self.vc!.type == "IMAGE" {
            
            cell.imageBinding(image: self.imageArray[indexPath.row], row: indexPath.row, Type:  self.vc!.type ?? "")
            
        }else if self.vc!.type == "VIDEO" {
            
            cell.videoBinding(videoThumb: self.thumbnailiamgeArray[indexPath.row], row: indexPath.row, Type:  self.vc!.type ?? "")
            
        }
        return cell
        
    }
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        sizeForItemAt indexPath: IndexPath) -> CGSize {
        
        return CGSize(width: UIScreen.main.bounds.size.width / 3 , height: 150)
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        insetForSectionAt section: Int) -> UIEdgeInsets {
        
        return UIEdgeInsets(top: 0, left: 0, bottom: 5, right: 0)
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 0
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 0
    }
    
}
