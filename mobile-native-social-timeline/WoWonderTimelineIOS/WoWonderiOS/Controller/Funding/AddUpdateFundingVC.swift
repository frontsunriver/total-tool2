

import UIKit
import ActionSheetPicker_3_0
import ZKProgressHUD
import WoWonderTimelineSDK

class AddUpdateFundingVC: UIViewController {
    @IBOutlet weak var titleTextField: UITextField!
    @IBOutlet weak var descriptionLabel: UITextView!
    @IBOutlet weak var amountBtn: UIButton!
    @IBOutlet weak var fundingImage: UIImageView!
    @IBOutlet weak var descriptionTextView: UITextView!
    @IBOutlet weak var tiltleLabel: UITextField!
    @IBOutlet weak var ImageBtn: RoundButton!
    
    var delegate: CreateFundDelegate!
    
    var imageData:Data? = nil
    var amountString:String? = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
        self.ImageBtn.setTitle(NSLocalizedString("Select Image", comment: "Select Image"), for: .normal)
        self.titleTextField.placeholder = NSLocalizedString("Title", comment: "Title")
        self.amountBtn.setTitle(NSLocalizedString("Amount", comment: "Amount"), for: .normal)
        self.descriptionTextView.placeholder = NSLocalizedString("Description", comment: "Description")
        
        
    }
    private func setupUI(){
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
           let save = UIBarButtonItem(title: NSLocalizedString("Submit", comment: "Submit"), style: .done, target: self, action: #selector(Save))
                 self.navigationItem.rightBarButtonItem  = save
        self.navigationItem.largeTitleDisplayMode = .never
        self.title = NSLocalizedString("New Funding", comment: "New Funding")
       }
       @objc func Save(){
        if self.titleTextField.text!.isEmpty{
            self.view.makeToast("Please enter title")
            
        }else if self.amountString == ""{
            self.view.makeToast("Please enter amount")
            
        }else if self.descriptionLabel.text!.isEmpty{
            self.view.makeToast("Please enter description")
            
        }else if self.imageData == nil{
            self.view.makeToast("Please select Image")
            
        }else{
            self.addFunding(imageData: self.imageData!)
            
        }
       }
    private func addFunding(imageData:Data){
           ZKProgressHUD.show()
        FundingManager.instance.addDunding(type:"create",title: self.titleTextField.text ?? "", description: self.descriptionLabel.text ?? "", amount: self.amountString ?? "", imageData: imageData) { (success, authError, error) in
            if success != nil {
                self.delegate.createFund()
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "Funds"), object: nil, userInfo: nil)
                ZKProgressHUD.dismiss()
                self.navigationController?.popViewController(animated: true)
                
            }
            else if authError != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors?.errorText)
                self.showAlert(title: "", message: (authError?.errors?.errorText)!)
            }
            else if error  != nil {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                
            }
        }
       }
    func openImageController(){
        ActionSheetStringPicker.show(withTitle: NSLocalizedString("Source", comment: ""),
                                     rows: [NSLocalizedString("Gallery", comment: ""),
                                            NSLocalizedString("Camera", comment: "")
            ],
                                     initialSelection: 0,
                                     doneBlock: { (picker, value, index) in
                                        
                                        if value == 0 {
                                            let imagePickerController = UIImagePickerController()
                                            
                                            imagePickerController.delegate = self
                                            
                                            imagePickerController.allowsEditing = true
                                            imagePickerController.sourceType = .photoLibrary
                                            self.present(imagePickerController, animated: true, completion: nil)
                                            
                                            
                                        }else if value == 1 {
                                            let imagePickerController = UIImagePickerController()
                                            
                                            imagePickerController.delegate = self
                                            
                                            imagePickerController.allowsEditing = true
                                            imagePickerController.sourceType = .camera
                                            self.present(imagePickerController, animated: true, completion: nil)
                                        }
                                        return
                                        
        }, cancel:  { ActionStringCancelBlock in return }, origin:self.view)
        
    }
    
    @IBAction func sleectImagePressed(_ sender: Any) {
        self.openImageController()
    }
    @IBAction func amountPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "Funding", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectAmountVC") as! SelectAmountVC
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
}

extension AddUpdateFundingVC : UIImagePickerControllerDelegate , UINavigationControllerDelegate {
    
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        
        let img = info[UIImagePickerController.InfoKey.originalImage] as? UIImage
        self.fundingImage.image = img
        let data = img!.jpegData(compressionQuality: 0.1)
        self.imageData = data
        self.dismiss(animated: true, completion: nil)
    }
    
    public func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
        picker.dismiss(animated: true)
    }
    
}
extension AddUpdateFundingVC : didGetFundingAmountDelegate {
    func didGetFundingAmount(amount: String, index: Int) {
        self.amountString = amount 
        self.amountBtn.setTitle(amount, for: .normal)
    }
}


