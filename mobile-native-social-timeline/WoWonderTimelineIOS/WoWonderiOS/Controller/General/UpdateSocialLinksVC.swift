

import UIKit
import WoWonderTimelineSDK
import ZKProgressHUD
class UpdateSocialLinksVC: UIViewController {
    
    @IBOutlet weak var LinkTextField: UITextField!
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var cancelBtn: UIButton!
    @IBOutlet weak var saveBtn: UIButton!
    
    var titleString:String? = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.cancelBtn.setTitle(NSLocalizedString("CANCEL", comment: "CANCEL"), for: .normal)
        self.saveBtn.setTitle(NSLocalizedString("SAVE", comment: "SAVE"), for: .normal)
        
        self.titleLabel.text = self.titleString ?? ""
        if self.titleString ?? "" == "Facebook"{
            LinkTextField.text = AppInstance.instance.profile?.userData?.facebook ?? ""
            
        }else  if self.titleString ?? "" == "Twitter"{
             LinkTextField.text = AppInstance.instance.profile?.userData?.twitter ?? ""
            
        }else  if self.titleString ?? "" == "Google+"{
            LinkTextField.text = AppInstance.instance.profile?.userData?.google ?? ""

            
        }else  if self.titleString ?? "" == "Vkontakle"{
            LinkTextField.text = AppInstance.instance.profile?.userData?.vk ?? ""
        }else  if self.titleString ?? "" == "Linkedin"{
              LinkTextField.text = AppInstance.instance.profile?.userData?.linkedin ?? ""
        }else  if self.titleString ?? "" == "Instagram"{
               LinkTextField.text = AppInstance.instance.profile?.userData?.instagram ?? ""
            
        }else  if self.titleString ?? "" == "Youtube"{
            LinkTextField.text = AppInstance.instance.profile?.userData?.youtube ?? ""
            
        }
    }
    
    
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func savePressed(_ sender: Any) {
        if self.LinkTextField.text!.isEmpty{
            self.view.makeToast("please enter your social link")
        }
        else{
            if self.titleString ?? "" == "Facebook"{
                       self.updateLink(type: "facebook", link: self.LinkTextField.text ?? "")
                   }else  if self.titleString ?? "" == "Twitter"{
                        self.updateLink(type: "twitter", link: self.LinkTextField.text ?? "")
                   }else  if self.titleString ?? "" == "Google+"{
                            self.updateLink(type: "google", link: self.LinkTextField.text ?? "")
                   }else  if self.titleString ?? "" == "Vkontakle"{
                       self.updateLink(type: "vk", link: self.LinkTextField.text ?? "")
                   }else  if self.titleString ?? "" == "Linkedin"{
                        self.updateLink(type: "linkedin", link: self.LinkTextField.text ?? "")
                   }else  if self.titleString ?? "" == "Instagram"{
                        self.updateLink(type: "instagram", link: self.LinkTextField.text ?? "")
                   }else  if self.titleString ?? "" == "Youtube"{
                       self.updateLink(type: "youtube", link: self.LinkTextField.text ?? "")
                   }
        }
       
    }
    private func updateLink(type:String,link:String){
        performUIUpdatesOnMain {
            UpdateUserManager.instance.updateSocialLinks(paramType: type, Link: link) { (success, authError, error) in
                if success != nil {
                    AppInstance.instance.getProfile()
                    self.view.makeToast(success?.message ?? "")
                    ZKProgressHUD.dismiss()
                    
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
        
        
    }
}
