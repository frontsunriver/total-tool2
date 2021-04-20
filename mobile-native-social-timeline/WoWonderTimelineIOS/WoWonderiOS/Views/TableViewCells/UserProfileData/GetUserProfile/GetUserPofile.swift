

import UIKit
import Kingfisher
import WoWonderTimelineSDK


class GetUserPofile: UITableViewCell,UICollectionViewDataSource,UICollectionViewDelegate {

    @IBOutlet weak var profileCollectionView: UICollectionView!
    
    @IBOutlet weak var profileLbl: UILabel!
    
    var profileArray = [[String:Any]]()
    
    var didSelectItemAction: ((IndexPath) -> Void)?


    override func awakeFromNib() {
        super.awakeFromNib()
      self.profileCollectionView.delegate = self
    self.profileCollectionView.dataSource = self
        self.profileLbl.text = NSLocalizedString("Profile Picture", comment: "Profile Picture")
        
   
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.profileArray.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "profileImageCell", for: indexPath) as!  UserProfileImageCell
        let index = self.profileArray[indexPath.row]
        if let image = index["postFile_full"] as? String{
            print(image)
            let url = URL(string: image)
            cell.profileImage.kf.setImage(with: url)
            
        }
       return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        didSelectItemAction?(indexPath)
    }
    
}
