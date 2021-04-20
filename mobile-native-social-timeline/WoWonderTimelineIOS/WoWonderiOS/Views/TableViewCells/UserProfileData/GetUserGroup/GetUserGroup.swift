

import UIKit
import WoWonderTimelineSDK
import Kingfisher

class GetUserGroup: UITableViewCell,UICollectionViewDelegate,UICollectionViewDataSource{
    
    
 
@IBOutlet weak var groupCollectionView: UICollectionView!
    
    @IBOutlet weak var groupLbl: UILabel!
    
var groupArray = [[String:Any]]()
var didSelectItemAction: ((IndexPath) -> Void)?
    
    override func awakeFromNib() {
        super.awakeFromNib()
    self.groupCollectionView.delegate = self
    self.groupCollectionView.dataSource = self
//        self.groupLbl.text = NSLocalizedString("Groups", comment: "Groups")
        
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
       return self.groupArray.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "groupImage", for: indexPath) as! UserGroupImageCell
        let index =  self.groupArray[indexPath.row]
        if let image = index["avatar"] as? String {
        let trimmedString = image.trimmingCharacters(in: .whitespaces)
               print(trimmedString)
        let url = URL(string: trimmedString)
            cell.groupImage.kf.setImage(with: url)
        }
        if let groupTitle = index["group_title"] as? String{
            cell.groupTitle.text = groupTitle
        }
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        didSelectItemAction?(indexPath)

    }
    

}
