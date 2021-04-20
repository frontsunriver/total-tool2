

import UIKit
import ZKProgressHUD
import WoWonderTimelineSDK

class SelectGIFVC: UIViewController {
    
    @IBOutlet weak var contentCollectionView: UICollectionView!
    private var gifArray = [GIFModel.Datum]()
    
    var contentIndexPath : IndexPath?
    var delegate:didSelectGIFDelegate?
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        self.fetchGIFS()
    }
    
    func setupUI(){
        self.title = NSLocalizedString("Select GIF", comment: "")
        self.contentCollectionView.register(UINib(nibName: "GIFCollectionItem", bundle: nil), forCellWithReuseIdentifier: "GIFCollectionItem")
    }
    private func fetchGIFS(){
        ZKProgressHUD.show()
        
        performUIUpdatesOnMain {
            GIFManager.instance.getGIFS(limit: 20, offset: 0) { (success, error) in
                if success != nil {
                 //   print("userList = \(success?.data ?? [])")
                    self.gifArray = success?.data ?? []
                    self.contentCollectionView.reloadData()
                    ZKProgressHUD.dismiss()
                }
                    
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
        }
    }
    
}
extension SelectGIFVC : UICollectionViewDelegate, UICollectionViewDataSource , UICollectionViewDelegateFlowLayout {
    
    
    func collectionView(_ collectionView: UICollectionView,
                        numberOfItemsInSection section: Int) -> Int {
        
        return self.gifArray.count ?? 0
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        
        self.contentIndexPath = indexPath
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "GIFCollectionItem", for: indexPath) as! GIFCollectionItem
        
        cell.bindGif(item: self.gifArray[indexPath.row], indexPath: indexPath)
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: collectionView.frame.size.width / 2 , height: 150)
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        insetForSectionAt section: Int) -> UIEdgeInsets {
        return UIEdgeInsets(top: 0, left: 0, bottom: 0, right: 0)
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
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        collectionView.deselectItem(at: indexPath, animated: true)
        self.delegate?.didSelectGIF(GIFUrl: self.gifArray[indexPath.row].images?.fixedHeightStill?.url ?? "", id: self.gifArray[indexPath.row].id ?? "")
       // self.navigationController?.popViewController(animated: true)
        self.dismiss(animated: true, completion: nil)
        
    }
}
