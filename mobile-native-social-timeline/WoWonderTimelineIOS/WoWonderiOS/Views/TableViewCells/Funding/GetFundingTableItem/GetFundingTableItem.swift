
import UIKit
import LinearProgressBar
import WoWonderTimelineSDK

class GetFundingTableItem: UITableViewCell {
    @IBOutlet weak var timeLabel: UILabel!
    
    @IBOutlet weak var progressBar: LinearProgressBar!
    @IBOutlet weak var collectionLabel: UILabel!
    @IBOutlet weak var totalAmountLabel: UILabel!
    @IBOutlet weak var amountLabel: UILabel!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var usernameLabel: UILabel!
    @IBOutlet weak var profileImage: UIImageView!
    override func awakeFromNib() {
        super.awakeFromNib()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

    }
    func bind(_ object:GetFundingModel.Datum,index:Int){
        self.usernameLabel.text = object.userData?.username ?? ""
        self.titleLabel.text = object.title?.htmlToString ?? ""
        self.descriptionLabel.text = object.datumDescription?.htmlToString ?? ""
        self.amountLabel.text = "$\(object.raised ?? 0).00"
        self.totalAmountLabel.text = "$\(object.amount?.htmlToString ?? "").00"
        let epocTime = TimeInterval(Int(object.time ?? "") ?? 1601815559)
        let myDate = NSDate(timeIntervalSince1970: epocTime)
        let formate = DateFormatter()
        formate.dateFormat = "yyyy-MM-dd"
        let dat = formate.string(from: myDate as Date)
        print("Date",dat)
        print("Converted Time \(myDate)")
        self.timeLabel.text = "\(dat)"
        self.progressBar.progressValue = CGFloat(object.bar ?? 0)
             let url = URL(string: object.image ?? "")
             self.profileImage.kf.setImage(with: url)
         }
}
