

import UIKit
import WoWonderTimelineSDK


class ChangedPictureController: UIViewController,UIImagePickerControllerDelegate,UINavigationControllerDelegate{
    
    
    @IBOutlet weak var changeLabel: UILabel!
    @IBOutlet weak var avatarBtn: UIButton!
    @IBOutlet weak var coverBtn: UIButton!
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate: changeProfilePicDelegate?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.changeLabel.text = NSLocalizedString("Change Image", comment: "Change Image")
        self.avatarBtn.setTitle(NSLocalizedString("Avatar", comment: "Avatar"), for: .normal)
        self.coverBtn.setTitle(NSLocalizedString("Cover", comment: "Cover"), for: .normal)
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
        
    }
    
   private func showAlert() {

        let alert = UIAlertController(title: "", message: NSLocalizedString("Select Source", comment: "Select Source"), preferredStyle: .actionSheet)
        alert.addAction(UIAlertAction(title: NSLocalizedString("Camera", comment: "Camera"), style: .default, handler: {(action: UIAlertAction) in
            self.getImage(fromSourceType: .camera)
        }))
        alert.addAction(UIAlertAction(title: NSLocalizedString("Photo Album", comment: "Photo Album"), style: .default, handler: {(action: UIAlertAction) in
            self.getImage(fromSourceType: .photoLibrary)
        }))
    alert.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .default, handler: nil))
    if let popoverController = alert.popoverPresentationController {
        popoverController.sourceView = self.view
        popoverController.sourceRect = CGRect(x: self.view.bounds.midX, y: self.view.bounds.midY, width: 0, height: 0)
        popoverController.permittedArrowDirections = []
    }
        self.present(alert, animated: true, completion: nil)
    }
    
    private func getImage(fromSourceType sourceType: UIImagePickerController.SourceType) {

        //Check is source type available
        if UIImagePickerController.isSourceTypeAvailable(sourceType) {

            let imagePickerController = UIImagePickerController()
            imagePickerController.delegate = self
            imagePickerController.sourceType = sourceType
            imagePickerController.modalTransitionStyle = .coverVertical
            imagePickerController.modalPresentationStyle = .fullScreen
            self.present(imagePickerController, animated: true, completion: nil)
        }
    }
    
    @IBAction func Avatar(_ sender: Any) {
        self.delegate?.changePic(image: "avatar")
        self.dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func Cover(_ sender: Any) {
         self.delegate?.changePic(image: "cover")
        self.dismiss(animated: true, completion: nil)
    }

    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
       self.dismiss(animated: true, completion: nil)
    }
    
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        var chosenImage = info[UIImagePickerController.InfoKey.originalImage] as! UIImage
        print(chosenImage)
        self.dismiss(animated: true, completion: nil)
    }
    
    

}
