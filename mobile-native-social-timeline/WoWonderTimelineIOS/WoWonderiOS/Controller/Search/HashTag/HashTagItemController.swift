
import UIKit
import WoWonderTimelineSDK


class HashTagItemController: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    
    let status = Reach().connectionStatus()
    
    var trending_hashtag = [[String:Any]]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        self.getGeneralData()
    }
    /// Network Connectivity
      @objc func networkStatusChanged(_ notification: Notification) {
          if let userInfo = notification.userInfo {
              let status = userInfo["Status"] as! String
              print("Status",status)
          }
      }
    
    private func getGeneralData(){
        let status = Reach().connectionStatus()
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetGeneralDataManager.sharedInstance.getGeneralDataManager(fetch: "trending_hashtag", offset: "") { (success, authError, Error) in
                    if success != nil {
                        for i in success!.trending_hashtag{
                            self.trending_hashtag.append(i)
                        }
                        self.collectionView.reloadData()
                    }
                    else if authError != nil{
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if Error != nil{
                        self.view.makeToast(Error?.localizedDescription)
                    }
                }
            }
        }
    }
}
extension HashTagItemController: UICollectionViewDelegate,UICollectionViewDataSource{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.trending_hashtag.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "hashTagCell", for: indexPath) as! SearchHastTagCell
        let index = self.trending_hashtag[indexPath.row]
        if let tag = index["tag"] as? String{
            cell.hashTagLabel.text = "\("#")\(tag)"
        }
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let index = self.trending_hashtag[indexPath.row]
        let vc = storyboard?.instantiateViewController(withIdentifier: "PostHashTagVC") as! PostHashTagController
        if let tag = index["tag"] as? String{
            vc.hashtag = tag
        }
        
        vc.modalTransitionStyle = .coverVertical
        vc.modalPresentationStyle = .fullScreen
        self.present(vc, animated: true, completion: nil)
    }
    
    
    
    
}
