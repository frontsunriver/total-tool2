

import WoWonderTimelineSDK
import UIKit

class GetUserFollowers: UITableViewCell,UICollectionViewDataSource,UICollectionViewDelegate{
   
@IBOutlet weak var followersCollectionView: UICollectionView!
    
    @IBOutlet weak var followingLbl: UILabel!
    
    var followersArray = [[String:Any]]()
    var delegate : blockUserDelegate?
    var didSelectItemAction: ((IndexPath) -> Void)?

    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.followersCollectionView.delegate = self
        self.followersCollectionView.dataSource = self
        self.followingLbl.text = NSLocalizedString("Following", comment: "Following")
        
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
       return self.followersArray.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
    let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "followerImage", for: indexPath) as! GetUserFollowerImageCell
    let index = self.followersArray[indexPath.row]
        if let image = index["avatar"] as? String{
       let url = URL(string: image)
       cell.followerImage.kf.setImage(with: url)
            
        }
      return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
       didSelectItemAction?(indexPath)
}
  
    
  
    

}
