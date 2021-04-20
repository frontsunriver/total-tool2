

import UIKit
import Toast_Swift
import AVFoundation
import WoWonderTimelineSDK

class MyVideosController: UIViewController {
    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var noVideoView: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!

     var myVideos = [[String:Any]]()
    let status = Reach().connectionStatus()
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
    self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
//        self.navigationController?.navigationItem.title = NSLocalizedString("My Videos", comment: "My Videos")
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
         navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.title = NSLocalizedString("My Videos", comment: "My Videos")
//        self.collectionView.register(UINib(nibName: "MyVideosCell", bundle: nil), forCellWithReuseIdentifier: "MyVideosCell")
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
//        self.activityIndicator.startAnimating()
        

        self.getVideos()
    }
    
   
    private func getVideos() {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                Get_User_ImageManager.sharedInstance.getUserImages(user_id: UserData.getUSER_ID()!, param: "video") { (success, authError, error) in
                    if success != nil {
                        for i in success!.data{
                            self.myVideos.append(i)
                        }
//                        self.myVideos = success!.data.map({$0})
                        if self.myVideos.count != 0{
                            self.collectionView.isHidden = false
                        }
                        else{
                            self.collectionView.isHidden = true
//                            self.noVideoView.isHidden = false
                        }
                        self.activityIndicator.stopAnimating()
                         self.collectionView.reloadData()
                    }
                    else if authError != nil {
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    func getThumbnailImageFromVideoUrl(url: URL, completion: @escaping ((_ image: UIImage?)->Void)) {
        DispatchQueue.global().async { //1
            let asset = AVAsset(url: url) //2
            let avAssetImageGenerator = AVAssetImageGenerator(asset: asset) //3
            avAssetImageGenerator.appliesPreferredTrackTransform = true //4
            let thumnailTime = CMTimeMake(value: 2, timescale: 1) //5
            do {
                let cgThumbImage = try avAssetImageGenerator.copyCGImage(at: thumnailTime, actualTime: nil) //6
                let thumbImage = UIImage(cgImage: cgThumbImage) //7
                DispatchQueue.main.async { //8
                    completion(thumbImage) //9
                }
            } catch {
                print(error.localizedDescription) //10
                DispatchQueue.main.async {
                    completion(nil) //11
                }
            }
        }
    }
}

extension MyVideosController : UICollectionViewDelegate,UICollectionViewDataSource{
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.myVideos.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "MyVideosCell", for: indexPath) as! MyVideosCell
        let index = self.myVideos[indexPath.row]
               if let image = index["postFile_full"] as? String{
                
                  let Url = "\("https://demo.wowonder.com/")\(image)"
                let videoThumb = URL(string: image)
                self.getThumbnailImageFromVideoUrl(url: videoThumb!) { (thumbImage) in
                    cell.myImage.kf.indicatorType = .activity
                    cell.myImage.image = thumbImage
                    
                }
               }
        return cell
    }
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
         let index = self.myVideos[indexPath.row]
        if let id = index["post_id"] as? String{
                        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
                        let vc = storyBoard.instantiateViewController(withIdentifier: "ShowPostVC") as! ShowPostController
                            vc.postId = id
            vc.isVideo = true
                        self.navigationController?.pushViewController(vc, animated: true)
                        }
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let padding: CGFloat =  50
        let collectionViewSize = collectionView.frame.size.width - padding
        
        return CGSize(width: collectionViewSize/2, height: collectionViewSize/2)
    }
    
    
}
