

import UIKit
import WoWonderTimelineSDK

class ActivitiesSectionOneTableitem: UITableViewCell {

    @IBOutlet weak var collectionView: UICollectionView!
    var proUsers = [ProUserModel.ProUser]()
    var didSelectItemAction: ((IndexPath) -> Void)?
    override func awakeFromNib() {
        super.awakeFromNib()
        self.setupUI()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self 
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    private func setupUI(){
        collectionView.register(UINib(nibName: "ActivitiesCollectionItem", bundle: nil), forCellWithReuseIdentifier: "ActivitiesCollectionItem")
    }
    func bind(_ object:[ProUserModel.ProUser]){
        self.proUsers = object
        print(self.proUsers.count)
        self.collectionView.reloadData()
    }
}
extension ActivitiesSectionOneTableitem:UICollectionViewDelegate,UICollectionViewDataSource{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.proUsers.count
    }
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "ActivitiesCollectionItem", for: indexPath) as? ActivitiesCollectionItem
        let object = proUsers[indexPath.row]
        cell?.bind(object)
        return cell!
    }
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        didSelectItemAction?(indexPath)
    }
}
