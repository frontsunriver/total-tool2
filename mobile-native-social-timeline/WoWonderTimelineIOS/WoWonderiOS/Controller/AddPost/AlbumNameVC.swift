

import UIKit
import WoWonderTimelineSDK

protocol didSelectAlbumNameDelegate {
    func didSelectAlbumName(albumNameString:String)
}
class AlbumNameVC: UIViewController {
    @IBOutlet weak var nameTextField: UITextField!
    var delegate:didSelectAlbumNameDelegate?

    override func viewDidLoad() {
        super.viewDidLoad()

    }
    
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func savePressed(_ sender: Any) {
        if self.nameTextField.text!.isEmpty{
            self.view.makeToast("please enter album name")
        }
        else{
            dismiss(animated: true) {
                self.delegate?.didSelectAlbumName(albumNameString: self.nameTextField.text ?? "")
            }
        }
       
    }
}
