
import UIKit
import WoWonderTimelineSDK


class SelectAmountTableItem: UITableViewCell {

    @IBOutlet weak var amountLabel: UILabel!
    override func awakeFromNib() {
        super.awakeFromNib()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    
    func bind(_ object:String){
        self.amountLabel.text = object
    }
}
