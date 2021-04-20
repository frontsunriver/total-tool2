
import UIKit
import CropViewController
import WoWonderTimelineSDK

class CropImageController: UIViewController,UIImagePickerControllerDelegate,UINavigationControllerDelegate,CropViewControllerDelegate{
    
    
    @IBOutlet weak var imageView: UIImageView!
    
    private var croppingStyle = CropViewCroppingStyle.default
    private var image: UIImage?
    private var croppedRect = CGRect.zero
    private var croppedAngle = 0
    
    var delegate : uploadImageDelegate!
    var imageType = ""
 
    override func viewDidLoad() {
        super.viewDidLoad()
        DispatchQueue.main.asyncAfter(deadline: .now() + 0.5) {
            self.showAlert()
        }
      
        
    }
    


    private func showAlert() {

         let alert = UIAlertController(title: "", message: NSLocalizedString("Select Source", comment: "Select Source"), preferredStyle: .actionSheet)
         alert.addAction(UIAlertAction(title: NSLocalizedString("Camera", comment: "Camera"), style: .default, handler: {(action: UIAlertAction) in
             self.croppingStyle = .default
             self.getImage(fromSourceType: .camera)
         }))
         alert.addAction(UIAlertAction(title: NSLocalizedString("Photo Album", comment: "Photo Album"), style: .default, handler: {(action: UIAlertAction) in
             self.croppingStyle = .default
             self.getImage(fromSourceType: .photoLibrary)
         }))
        alert.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: "Cancel"), style: .default, handler: {
            (action: UIAlertAction) in
            self.dismiss(animated: true, completion: nil)
        }))
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
    
    
     func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
         var chosenImage = info[UIImagePickerController.InfoKey.originalImage] as! UIImage
        
        let cropController = CropViewController(croppingStyle: croppingStyle, image: chosenImage)
         cropController.delegate = self

         self.image = chosenImage
        
        //If profile picture, push onto the same navigation stack
             if croppingStyle == .circular {
                 if picker.sourceType == .camera {
                     picker.dismiss(animated: true, completion: {
                         self.present(cropController, animated: true, completion: nil)
                     })
                 } else {
                     picker.pushViewController(cropController, animated: true)
                 }
             }
             else {
                 picker.dismiss(animated: true, completion: {
                     self.present(cropController, animated: true, completion: nil)
                 })
             }
         self.dismiss(animated: true, completion: nil)
     }
    
    public func cropViewController(_ cropViewController: CropViewController, didCropToImage image: UIImage, withRect cropRect: CGRect, angle: Int) {
         self.croppedRect = cropRect
         self.croppedAngle = angle
        self.updateImageViewWithImage(image, fromCropViewController: cropViewController)
        
     }
    
    public func updateImageViewWithImage(_ image: UIImage, fromCropViewController cropViewController: CropViewController) {

        imageView.image = image
        layoutImageView()
        
        self.navigationItem.rightBarButtonItem?.isEnabled = true
        
        if cropViewController.croppingStyle != .circular {
        imageView.isHidden = true

        cropViewController.dismissAnimatedFrom(self, withCroppedImage: image,
                                               toView: self.imageView,
                                                         toFrame: CGRect.zero,
                                                         setup: {  },
                                                         completion: {
                                                          self.imageView.isHidden = false })
        self.delegate.uploadImage(imageType: imageType, image: self.imageView.image!)
        self.dismiss(animated: true, completion: nil)

        }
        else {
           cropViewController.dismiss(animated: true) {
           self.dismiss(animated: true, completion: nil)
          }

        }
    }
    
    func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
        self.dismiss(animated: true) {
            self.dismiss(animated: true, completion: nil)
        }
      }
    
    func cropViewController(_ cropViewController: CropViewController, didFinishCancelled cancelled: Bool) {
                self.dismiss(animated: true) {
            self.dismiss(animated: true, completion: nil)
        }

    }
    

        public override func viewDidLayoutSubviews() {
            super.viewDidLayoutSubviews()
            layoutImageView()
        }
    //    .........////////////
        public func layoutImageView() {
            guard imageView.image != nil else { return }
            
            let padding: CGFloat = 20.0
            
            var viewFrame = self.view.bounds
            viewFrame.size.width -= (padding * 2.0)
            viewFrame.size.height -= ((padding * 2.0))
            
            var imageFrame = CGRect.zero
            imageFrame.size = imageView.image!.size;
            
            if imageView.image!.size.width > viewFrame.size.width || imageView.image!.size.height > viewFrame.size.height {
                let scale = min(viewFrame.size.width / imageFrame.size.width, viewFrame.size.height / imageFrame.size.height)
                imageFrame.size.width *= scale
                imageFrame.size.height *= scale
                imageFrame.origin.x = (self.view.bounds.size.width - imageFrame.size.width) * 0.5
                imageFrame.origin.y = (self.view.bounds.size.height - imageFrame.size.height) * 0.5
                imageView.frame = imageFrame
            }
            else {
                self.imageView.frame = imageFrame;
                self.imageView.center = CGPoint(x: self.view.bounds.midX, y: self.view.bounds.midY)
            }
        }



}
