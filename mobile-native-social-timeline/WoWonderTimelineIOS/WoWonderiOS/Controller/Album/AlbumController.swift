//

import UIKit
import Toast_Swift
import WoWonderTimelineSDK
import Kingfisher
class AlbumController: UIViewController,CreateAlbumDelegate{

    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var noAlbumView: UIView!
    
    
    var albums = [[String:Any]]()
    var offset = "0"
    var estimateWidth = 160.0
     var cellMarginSize = 10.0
    
    let status = Reach().connectionStatus()
    let Storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.rightBarButtonItem = UIBarButtonItem(title: NSLocalizedString("Create", comment: "Create"), style: .done, target: self, action: #selector(self.Create(sender:)))
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Albums", comment: "Albums")
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
print("abc")
        self.activityIndicator.startAnimating()
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
//          self.setupGridView()
        self.getAlbum()
    }
    
//    override func viewDidLayoutSubviews() {
//        super.viewDidLayoutSubviews()
//
//        self.setupGridView()
//        DispatchQueue.main.async {
//            self.collectionView.reloadData()
//        }
//    }

    
    
    func setupGridView() {
        let flow = collectionView?.collectionViewLayout as! UICollectionViewFlowLayout
        flow.minimumInteritemSpacing = CGFloat(self.cellMarginSize)
        flow.minimumLineSpacing = CGFloat(self.cellMarginSize)
    }
    
    
    private func getAlbum(){
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetAlbumManager.sharedInstance.getAlbum(userId: UserData.getUSER_ID()!, offset: self.offset) { (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            self.albums.append(i)
                        }
                        if self.albums.count != 0{
                            if let postId =  self.albums.last!["post_id"] as? String{
                                self.offset = postId
                                self.collectionView.isHidden = false
                            }
                        }
                        else{
                            self.collectionView.isHidden = true
                        }
                        self.activityIndicator.stopAnimating()
                        self.collectionView.reloadData()
                    }
                    else if authError != nil {
                        self.activityIndicator.stopAnimating()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
            
        }
    }

    @IBAction func Create(sender:UIBarButtonItem){
        let vc = Storyboard.instantiateViewController(withIdentifier: "CreateAlbumVC") as! CreateAlbumController
        vc.createAlbumDelegate = self
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
}

extension AlbumController : UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.albums.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "GetAlbumImageCell", for: indexPath) as! GetAlbumCell
        let index = self.albums[indexPath.row]
        if let albumName = index["album_name"] as? String{
            cell.albumName.text! = albumName
        }
        
        if let album = index["photo_album"] as? [[String:Any]]{
            if let image = album.first!["image"] as? String{
                let url = URL(string: image)
                cell.albumImage.kf.setImage(with: url)
            }
            cell.imageCount.text! = "\(album.count)"
        }
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let index = self.albums[indexPath.row]
        let vc = Storyboard.instantiateViewController(withIdentifier: "AlbumImagesVC") as! AlbumImagesController
       if let albumName = index["album_name"] as? String{
        vc.albumname = albumName
        }
        
        if let photo_album  = index["photo_album"] as? [[String:Any]]{
          vc.album  = photo_album
        }
        vc.albums = index
        self.navigationController?.pushViewController(vc, animated: true)
    }
    
        func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
            let collectionWidth = collectionView.frame.size.width
            let widht = (collectionWidth / 2) - 10
            let height = (self.view.frame.size.height / 4) - 10
            return CGSize(width: widht , height: widht)
        
      }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 15
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, insetForSectionAt section: Int) -> UIEdgeInsets {
        return UIEdgeInsets.zero
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 10
    }
    
    func calculateWith() -> CGFloat {
        let estimatedWidth = CGFloat(estimateWidth)
        let cellCount = floor(CGFloat(self.view.frame.size.width / estimatedWidth))
        
        let margin = CGFloat(cellMarginSize * 2)
        let width = (self.view.frame.size.width - CGFloat(cellMarginSize) * (cellCount - 1) - margin) / cellCount
        
        return width
    }
    
    func createAlbum(data: [String : Any]) {
        self.albums.append(data)
        self.collectionView.isHidden = false
        self.collectionView.reloadData()
    }
}
