
import UIKit
import WoWonderTimelineSDK
import ActiveLabel


class DetailOfferTableItem: UITableViewCell {
    @IBOutlet weak var thumbnailImage: UIImageView!
    
    @IBOutlet weak var descriptionLabel: ActiveLabel!
    
    @IBOutlet weak var discountLabel: UILabel!
    @IBOutlet weak var endDateLabel: UILabel!
    @IBOutlet weak var pageNamelabel: UILabel!
    @IBOutlet weak var pageImage: Roundimage!
    
    @IBOutlet weak var backBtn: UIButton!
    
    var object: GetOffersModel.Datum?
    var vc:DetailOfferVC?
    override func awakeFromNib() {
        super.awakeFromNib()
       addTapGesture()
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
  

    }
    func addTapGesture() {
         let labelTapGesture = UITapGestureRecognizer(target: self, action: #selector(doSomethingOnTap))
         //Add this line to enable user interaction on your label
         pageNamelabel.isUserInteractionEnabled = true
         pageNamelabel.addGestureRecognizer(labelTapGesture)
     }
    @objc func doSomethingOnTap() {
        self.gotoPageController()
       }
    func bind(_ object: GetOffersModel.Datum,index:Int){
        self.object = object
           self.endDateLabel.text = object.expireDate ?? ""
           let url = URL(string: object.image ?? "")
                       self.thumbnailImage.kf.setImage(with: url)
        let pageURL = URL(string: object.page?.avatar ?? "")
                            self.pageImage.kf.setImage(with: pageURL)
        self.pageNamelabel.text = "@\(object.page?.pageName ?? "")"
        self.descriptionLabel.text = object.datumDescription?.htmlToString ?? ""
        self.discountLabel.text = object.discountedItems ?? ""
        self.endDateLabel.text = object.expireDate ?? ""
       }
    func gotoPageController () {
          let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
          let vc = storyboard.instantiateViewController(withIdentifier: "PageVC") as! PageController
          
          vc.isPageOwner = false
        
        let pageData = ForwardPageData(page_Name: object?.page?.pageName ?? "", page_Title: object?.page?.pageTitle ?? "", cateforyId: object?.page?.pageCategory ?? "", categoryName: object?.page?.category ?? "", callActionType: object?.page?.callActionType ?? "", callActionUrl: object?.page?.callActionTypeURL ?? "", company: object?.page?.company ?? "", about: object?.page?.about ?? "", phone: object?.page?.phone ?? "", address: object?.page?.address ?? "", website: object?.page?.website ?? "", pageId: object?.page?.pageID ?? "", facebook: object?.page?.facebook ?? "", twitter: object?.page?.twitter ?? "", instagrm: object?.page?.instgram ?? "", linkdin: object?.page?.linkedin ?? "", youtube: object?.page?.youtube ?? "", vk: object?.page?.vk ?? "", pageCover: object?.page?.cover ?? "", pageIcon: object?.page?.avatar ?? "", rating: Double(object?.page?.rating ?? 0), pageurl: object?.page?.url ?? "", isLike: false)
          
          vc.pageData = pageData
        vc.page_id = object?.page?.pageID ?? ""
//          vc.delegate = self
//          vc.deleteDelegate = self
          vc.isPageOwner = true
          vc.isFromList = true
        self.vc?.navigationController?.pushViewController(vc, animated: true)
      }
      
    
}
