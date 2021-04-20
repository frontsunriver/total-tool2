
import UIKit
import WoWonderTimelineSDK
struct FeelingType{
    var id:Int?
    var feelingType:String?
    var feelingName:String?
    var feelingImage:String?
    
    
}

class FeelingSelectVC: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    
    var feelTypeDataSet = [FeelingType]()
     var delegate:didSelectingFeelingTypeDelegate?
     var feelingTypeString:String? = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
       
        
    }
    private func setupUI(){
        collectionView.register(UINib(nibName: "SelectFeelingTypeCollectionItem", bundle: nil), forCellWithReuseIdentifier: "SelectFeelingTypeCollectionItem")
        
        self.feelTypeDataSet = [
                   FeelingType(id: 1, feelingType: "angry", feelingName: "Angry", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f621.png"),
                   FeelingType(id: 2, feelingType: "funny", feelingName: "Funny", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f602.png"),
                   FeelingType(id: 3, feelingType: "loved", feelingName: "Loved", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f60d.png"),
                   FeelingType(id: 4, feelingType: "cool", feelingName: "Cool", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f60e.png"),
                   FeelingType(id: 5, feelingType: "happy", feelingName: "Happy", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f603.png"),
                   FeelingType(id: 6, feelingType: "tired", feelingName: "Tired", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f62b.png"),
                   FeelingType(id: 7, feelingType: "sleepy", feelingName: "Sleepy", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f634.png"),
                   FeelingType(id: 8, feelingType: "expressionless", feelingName: "Expressionless", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f611.png"),
                   FeelingType(id: 9, feelingType: "confused", feelingName: "Confused", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f615.png"),
                   FeelingType(id: 10, feelingType: "shocked", feelingName: "Shocked", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f631.png"),
                   FeelingType(id: 11, feelingType: "blessed", feelingName: "VerySad", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f62d.png"),
                   
                   FeelingType(id: 12, feelingType: "blessed", feelingName: "Blessed", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f607.png"),
                   
                   FeelingType(id: 13, feelingType: "bored", feelingName: "Bored", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f610.png"),
                   FeelingType(id: 14, feelingType: "broke", feelingName: "Broke", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f494.png"),
                   FeelingType(id: 15, feelingType: "lovely", feelingName: "Lovely", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f494.png"),
                   
                   FeelingType(id: 16, feelingType: "smirk", feelingName: "Hot", feelingImage: "https://abs.twimg.com/emoji/v1/72x72/1f60f.png"),
               ]
    }
    
    
}
extension FeelingSelectVC:UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.feelTypeDataSet.count ?? 0
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "SelectFeelingTypeCollectionItem", for: indexPath) as? SelectFeelingTypeCollectionItem
        let object = self.feelTypeDataSet[indexPath.row]
        cell?.bind(object)
        return cell!
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        self.delegate?.didselectFeelingType(type: self.feelingTypeString ?? "", feelingString: self.feelTypeDataSet[indexPath.row].feelingName ?? "")
        self.navigationController?.popViewController(animated: true)
    }
    
    func collectionView(_ collectionView: UICollectionView,
                        layout collectionViewLayout: UICollectionViewLayout,
                        sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: self.collectionView.frame.width / 3 , height: 130)
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

