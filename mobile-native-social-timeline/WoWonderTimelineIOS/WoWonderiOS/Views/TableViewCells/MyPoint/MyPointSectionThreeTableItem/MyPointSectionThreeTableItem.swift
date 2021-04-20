

import UIKit
import WoWonderTimelineSDK

class MyPointSectionThreeTableItem: UITableViewCell {
    
    @IBOutlet weak var tapView: RoundButton!
    
    @IBOutlet weak var earbLBl: UILabel!
    @IBOutlet weak var waleetLbl: UILabel!
    
    var vc:MyPointsVC?
    override func awakeFromNib() {
        super.awakeFromNib()
        let tap = UITapGestureRecognizer(target: self, action: #selector(self.handleTap(_:)))
        tapView.addGestureRecognizer(tap)
        self.earbLBl.text = NSLocalizedString("You erarned points will automatically go to", comment: "You erarned points will automatically go to")
        self.waleetLbl.text = NSLocalizedString("#Wallet", comment: "#Wallet")
    }
    
    @objc func handleTap(_ sender: UITapGestureRecognizer? = nil) {
        
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "WalletVC") as! WalletMainController
        vc.mybalance = AppInstance.instance.profile?.userData?.wallet ?? ""
        if (ControlSettings.showPaymentVC == true){
            self.vc?.navigationController?.pushViewController(vc, animated: true)
        }
    }
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
    }
    
}
