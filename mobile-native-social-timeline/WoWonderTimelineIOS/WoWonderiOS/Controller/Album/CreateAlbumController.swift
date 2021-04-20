

import UIKit
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK
class CreateAlbumController: UIViewController,uploadImageDelegate {
    
    
    @IBOutlet weak var albumName: RoundTextField!
    @IBOutlet weak var CollectionView: UICollectionView!
    @IBOutlet weak var createLabel: UILabel!
    @IBOutlet weak var labelDes: UILabel!
    @IBOutlet weak var photoLabel: UILabel!
    @IBOutlet weak var createBtn: UIButton!
    @IBOutlet weak var createAlbumLbl: UILabel!
    
    var images = [UIImage]()
    var delegate : uploadImageDelegate!
    var Albumimages = [Data]()
    
    var createAlbumDelegate : CreateAlbumDelegate!
    let status = Reach().connectionStatus()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.isHidden = true
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.albumName.placeholder = NSLocalizedString("Album Name", comment: "Album Name")
        self.createLabel.text = NSLocalizedString("Create album", comment: "Create album")
        self.labelDes.text = NSLocalizedString("Make your own album to save your special moment", comment: "Make your own album to save your special moment")
        self.photoLabel.text = NSLocalizedString("Photos", comment: "Photos")
        self.createBtn.setTitle(NSLocalizedString("Create", comment: "Create"), for: .normal)
        self.createAlbumLbl.text = NSLocalizedString("Create Album", comment: "Create Album")
    }
    
    
    
    @IBAction func Create(_ sender: Any) {
        if self.albumName.text?.isEmpty == true {
            self.view.makeToast(NSLocalizedString("Enter Album-Name", comment: "Enter Album-Name"))
        }
        else{
            self.createAlbum(data: self.Albumimages)
        }
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
        
    }
    
    private func createAlbum(data : [Data]?) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
            performUIUpdatesOnMain {
                CreatePhotoAlbumManager.sharedInstance.createAlbum(data: data, albumName: self.albumName.text!) { (success, authError, error) in
                    if (success != nil) {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(NSLocalizedString("Album Created SuccessFully", comment: "Album Created SuccessFully"))
                        self.dismiss(animated: true, completion: {
                            self.createAlbumDelegate.createAlbum(data: success!.data)
                        })
                    }
                    else if (authError != nil) {
                         ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                         ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)
                    }
                }
            }
        }
        
    }
}
    
    extension CreateAlbumController : UICollectionViewDelegate,UICollectionViewDataSource{
        func numberOfSections(in collectionView: UICollectionView) -> Int {
            return 2
        }
        func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
            if section == 0{
                return 1
            }
            else {
                return self.images.count
            }
        }
        
        func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
            if indexPath.section == 0 {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "AddImage", for: indexPath) as! AddImageCell
                return cell
            }
            else {
                let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "images", for: indexPath) as! ImagesCell
                let index = self.images[indexPath.row]
                cell.cancel.tag = indexPath.row
                cell.cancel.addTarget(self, action: #selector(imageCancel(sender:)), for: .touchUpInside)
                cell.albumImage.image = index
                return cell
            }
        }
        
        func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
            if indexPath.section == 0{
                let Storyboard = UIStoryboard(name: "Main", bundle: nil)
                let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
                vc.delegate = self
                vc.imageType = "upload"
                vc.modalTransitionStyle = .coverVertical
                vc.modalPresentationStyle = .fullScreen
                self.present(vc, animated: true, completion: nil)
            }
        }
        
        func uploadImage(imageType: String, image: UIImage) {
            self.images.append(image)
            let image =  image.jpegData(compressionQuality: 0.1)
            self.Albumimages.append(image!)
            self.CollectionView.reloadData()
            
        }
        
        @IBAction func imageCancel(sender:UIButton){
            let item = sender.tag
            self.images.remove(at: item)
            self.Albumimages.remove(at: item)
            self.CollectionView.reloadData()
        }
}



