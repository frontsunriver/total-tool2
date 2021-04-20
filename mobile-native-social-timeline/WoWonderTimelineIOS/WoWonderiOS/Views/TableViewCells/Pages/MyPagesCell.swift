

import UIKit
import  Kingfisher
import WoWonderTimelineSDK


class MyPagesCell: UITableViewCell,UICollectionViewDelegate,UICollectionViewDataSource {
   
    @IBOutlet weak var collectionView: UICollectionView!
    
    
    var myPagesArray = [[String:Any]]()
    var didSelectItemAction: ((IndexPath) -> Void)?
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
    }

  func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
    self.myPagesArray.count
     }
     
     func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CollectionCell", for: indexPath) as! MyPagesCollectionCell
        let index = myPagesArray[indexPath.row]
        if let pageIcon = index["avatar"] as? String{
            let url = URL(string: pageIcon)
            cell.pageIcon.kf.setImage(with: url)
        }
        
        if let name = index["page_name"] as? String{
            cell.pageTitle.text = name
        }
       
        return cell
     }
    
        func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
           didSelectItemAction?(indexPath)
        }
  
    
}
