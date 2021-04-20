import WoWonderTimelineSDK
import UIKit
class BankTransferVC: BaseVC {
    
    @IBOutlet weak var cancelBtn: UIButton!
    @IBOutlet weak var receiptImage: UIImageView!
    
    var payType:String? = ""
    var Description:String? = ""
    var amount:Int? = 0
    var memberShipType:Int? = 0
    var credits:Int? = 0
    var paymentType:String? = ""
    var isMediaStatus:Bool? = false
    var mediaData:Data? = nil
    private let imagePickerController = UIImagePickerController()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.cancelBtn.isHidden = false
        
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.tabBarController?.tabBar.isHidden = true
    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        self.tabBarController?.tabBar.isHidden = false

    }
    @IBAction func backPressed(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.receiptImage.image = nil
        self.mediaData = nil
         self.cancelBtn.isHidden = false
        
    }
    
    @IBAction func sendPressed(_ sender: Any) {
        if !self.isMediaStatus!{
            self.view.makeToast("Please add receipt")
        }else{
//            self.uploadReceipt()
        }
    }
    
    @IBAction func selectPictureBtn(_ sender: Any) {
        
        let alert = UIAlertController(title: "", message: NSLocalizedString("Select Source", comment: "Select Source"), preferredStyle: .alert)
        let camera = UIAlertAction(title: NSLocalizedString("Camera", comment: "Camera"), style: .default) { (action) in
            self.imagePickerController.delegate = self
            self.imagePickerController.allowsEditing = true
            self.imagePickerController.sourceType = .camera
            self.present(self.imagePickerController, animated: true, completion: nil)
        }
        let gallery = UIAlertAction(title: NSLocalizedString("Gallery", comment: "Gallery"), style: .default) { (action) in
            self.imagePickerController.delegate = self
            self.imagePickerController.allowsEditing = true
            self.imagePickerController.sourceType = .photoLibrary
            self.present(self.imagePickerController, animated: true, completion: nil)
        }
        let cancel = UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .destructive, handler: nil)
        alert.addAction(camera)
        alert.addAction(gallery)
        alert.addAction(cancel)
        if let popoverController = alert.popoverPresentationController {
            popoverController.sourceView = self.view
            popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
            popoverController.permittedArrowDirections = []
        }
        self.present(alert, animated: true, completion: nil)
        
    }
  
}
extension  BankTransferVC:UIImagePickerControllerDelegate, UINavigationControllerDelegate{
    
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        let image = info[UIImagePickerController.InfoKey.originalImage] as! UIImage
        self.receiptImage.image = image
        self.mediaData = image.pngData()
        self.isMediaStatus = true
         self.cancelBtn.isHidden = false
        self.dismiss(animated: true, completion: nil)
        
    }
}
