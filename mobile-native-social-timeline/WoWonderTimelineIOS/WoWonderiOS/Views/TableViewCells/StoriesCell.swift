
import UIKit
import WoWonderTimelineSDK

class StoriesCell: UITableViewCell,UICollectionViewDelegate,UICollectionViewDataSource {
  
    
    @IBOutlet weak var collectionView: UICollectionView!

    private var storiesArray = [GetStoriesModel.UserDataElement]()
    var shouldRefreshStories = false
    var vc:ViewController?

    
    override func awakeFromNib() {
         super.awakeFromNib()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
    }
    func bind(_ object:[GetStoriesModel.UserDataElement]){
        self.storiesArray = object
        self.collectionView.reloadData()
    }
    
      func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        switch section{
        case 0 :
        return 1
        case 1:
            return self.storiesArray.count ?? 0
        default:
            return 1
        }
      }
    func numberOfSections(in collectionView: UICollectionView) -> Int {
        return 2
    }

      func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        switch indexPath.section {
        case 0:
             let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CollectionViewCell", for: indexPath) as! CollectionViewCell
             cell.bind(nil, section: indexPath.section)
                     return cell
            case 1:
                        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CollectionViewCell", for: indexPath) as! CollectionViewCell
                        let object = self.storiesArray[indexPath.row]
                        cell.bind(object, section: indexPath.section)
                                return cell
        default:
           let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "CollectionViewCell", for: indexPath) as! CollectionViewCell
             cell.bind(nil, section: indexPath.section)
                     return cell
        }
      }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
           
           if indexPath.section != 0 {
               self.shouldRefreshStories = true
               PPStoriesItemsViewControllerVC = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "StoryItemVC") as! StoryItemVC
               let vc = PPStoriesItemsViewControllerVC
               vc.refreshStories = {
                   //                self.viewModel?.refreshStories.accept(true)
               }
               vc.modalPresentationStyle = .overFullScreen
               vc.pages = (self.storiesArray)
               vc.currentIndex = indexPath.row
            self.vc!.present(vc, animated: true, completion: nil)
           }else{
               guard let cell = collectionView.cellForItem(at: indexPath) else {
                   return
               }
            self.vc?.showStoriesLog()
               
//            self.vc.showAddStory(cell: cell)
           }
           
       }
}
