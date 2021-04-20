

import WoWonderTimelineSDK
import UIKit

class NotificationOneTableItem: UITableViewCell {
    
    @IBOutlet weak var Switchlabel: UISwitch!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var titleLabel: UILabel!
    
    var privacyVC:PrivacyVC?
    var TypeString:String? = ""
    var value:Int? = 0
    override func awakeFromNib() {
        super.awakeFromNib()
        self.titleLabel.text = NSLocalizedString("Notification Popup", comment: "Notification Popup")
        self.descriptionLabel.text = NSLocalizedString("get notifications when you receive messages", comment: "get notifications when you receive messages")
      
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
    }
    @IBAction func switchAction(_ sender: Any) {
      
            if self.value == 0{
                if self.TypeString == "shareMyLocation"{
                                   AppInstance.instance.locationManager.startUpdatingLocation()
                               }
                self.value = 1
                self.Switchlabel.setOn(true, animated: true)
                self.privacyVC?.updateprivacy(Type: self.TypeString ?? "", value: self.value ?? 0)
            }else{
                if self.TypeString == "shareMyLocation"{
                    AppInstance.instance.locationManager.stopUpdatingLocation()
                }
                self.Switchlabel.setOn(false, animated: true)
                           self.value = 0
                           self.privacyVC?.updateprivacy(Type: self.TypeString ?? "", value: self.value ?? 0)
            }
    }
}
