
import UIKit
import WoWonderTimelineSDK

class NotificationTwoTableItem: UITableViewCell {
    
    
    @IBOutlet weak var converLbl: UILabel!
    @IBOutlet weak var descLbl: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.converLbl.text = NSLocalizedString("Conversation Tones", comment: "Conversation Tones")
        self.descLbl.text = NSLocalizedString("Play sounds for incoming and outgoing messages", comment: "Play sounds for incoming and outgoing messages")
        
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}
