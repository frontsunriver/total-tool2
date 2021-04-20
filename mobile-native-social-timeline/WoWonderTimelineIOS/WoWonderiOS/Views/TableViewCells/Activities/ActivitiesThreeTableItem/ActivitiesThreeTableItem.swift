

import UIKit

class ActivitiesThreeTableItem: UITableViewCell {
    

    @IBOutlet weak var lastLabel: UILabel!
    
    @IBOutlet weak var sellLabel: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.lastLabel.text = NSLocalizedString("Last Activites", comment: "Last Activites")
        self.sellLabel.text = NSLocalizedString("See All", comment: "See All")
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    
}
