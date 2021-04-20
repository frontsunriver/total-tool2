import WoWonderTimelineSDK
import UIKit

class JobAppliesCell: UITableViewCell {

    
    @IBOutlet weak var profileImage: Roundimage!
    @IBOutlet weak var userName: UILabel!
    @IBOutlet weak var positionLabel: UILabel!
    @IBOutlet weak var locationLabel: UILabel!
    @IBOutlet weak var phoneNumber: UILabel!
    @IBOutlet weak var emailLabel: UILabel!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var messageBtn: UIButton!
    @IBOutlet weak var work: UILabel!
    @IBOutlet weak var currentPosition: UILabel!
    @IBOutlet weak var EndDate: UILabel!
    @IBOutlet weak var startDate: UILabel!
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    
}
