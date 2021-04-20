import WoWonderTimelineSDK
import UIKit

class AddPostSectionTwoTableItem: UITableViewCell {

    @IBOutlet weak var backGroundImage: UIImageView!
    @IBOutlet weak var textView: UITextView!
    @IBOutlet weak var collectionView: UICollectionView!
    
    var vc:AddPostVC?
    var imageString = [String]()
    var idString = [String]()
    var filter = [String]()
    var filterIDs = [String]()
    
    override func awakeFromNib() {
        super.awakeFromNib()
        
        let siteSetting = AppInstance.instance.siteSettings
        if let postColor = siteSetting["post_colors"] as? [String:[String:Any]]{
            for (key,value) in postColor{
                print("Key = \(key) , value = \(value)")
                if let values = value as? [String:Any] {
                    if let image = values["image"] as? String{
                        if image == ""{
                             
                         }else{
                             self.imageString.append(image)
                             self.idString.append(key)
                         }
                         print("Image String = \(imageString.count)")
                         idString.forEach { (it) in
                             print("images Link = \(it)")
                         }
                    }
                }
            }
        }
//        for (key,value) in (AppInstance.instance.siteSettings?.config?.postColors!)!{
//            print("Key = \(key) , value = \(value)")
//            if value.image == ""{
//
//            }else{
//                self.imageString.append(value.image)
//                           self.idString.append(key)
//            }
//            print("Image String = \(imageString.count)")
//            idString.forEach { (it) in
//                print("images Link = \(it)")
//            }
////           filter =   self.imageString.filter({!$0.isEmpty})
////            filterIDs =   self.idString.filter({!$0.isEmpty})
//
//        }
//        print("Image String = \(imageString)")
        self.filter.append("12")
        collectionView.delegate = self
        collectionView.dataSource = self
        collectionView.register(UINib(nibName: "AddPostSectionTwoCollectionItem", bundle: nil), forCellWithReuseIdentifier: "AddPostSectionTwoCollectionItem")
        
       
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(){
//        self.collectionView.reloadData()
    }
}
extension AddPostSectionTwoTableItem:UICollectionViewDelegate,UICollectionViewDataSource{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.imageString.count ?? 0
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "AddPostSectionTwoCollectionItem", for: indexPath) as? AddPostSectionTwoCollectionItem
        let object =   self.imageString[indexPath.row]
                         cell?.bind(object ?? "")
//        if indexPath.row == (imageString.count - 1)
//        {
//            self.backGroundImage.image = nil
//            cell?.profileImage.backgroundColor = .white
//        }else{
//            let object =   self.imageString[indexPath.row]
//                  cell?.bind(object ?? "")
//        }
        return cell!
    }
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
       
        if indexPath.row == (imageString.count - 1){
        self.vc?.PostColor = "0"
            
            AppInstance.instance.isBackGroundSelected = false
            if AppInstance.instance.isBackGroundSelected{
                       self.textView.textColor = .white
                       
                   }else{
                        self.textView.textColor = .black
                   }
     
        }else{
            let url = URL(string: self.imageString[indexPath.row]  ?? "")
            self.vc?.PostColor = self.idString[indexPath.row] ?? ""
                     self.backGroundImage.kf.setImage(with: url)
                   AppInstance.instance.isBackGroundSelected = true
            if AppInstance.instance.isBackGroundSelected{
                       self.textView.textColor = .white
                       
                   }else{
                        self.textView.textColor = .black
                   }
        }
        self.vc?.tableView.reloadData()
    }
    func collectionView(_ collectionView: UICollectionView,
                           layout collectionViewLayout: UICollectionViewLayout,
                           sizeForItemAt indexPath: IndexPath) -> CGSize {
           return CGSize(width: collectionView.frame.width , height: collectionView.frame.height )
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
    

}
