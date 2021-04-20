


import UIKit
import ActionSheetPicker_3_0
import WoWonderTimelineSDK

protocol didOpenSelectFeelingDelegate {
    func didOpenSelectFeeling()
}
class PostTypeVC: UIViewController {
    @IBOutlet weak var tableView: UITableView!
    //    var list = [String:UIImage]()
    var list:[(name: String, value: UIImage)] = []
    var delegate:didSelectPostType?
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.setupUI()
    }
    func setupUI(){
        list = [
            (NSLocalizedString("Image Gallery", comment: ""),UIImage(named: "photos")!),
            ( NSLocalizedString("Video Gallery", comment: ""),UIImage(named: "albums")!),
            ( NSLocalizedString("Mention Contact", comment: ""),UIImage(named: "people")!),
//            ( NSLocalizedString("Location/Place", comment: ""),UIImage(named: "ic_profile_small")!),
            ( NSLocalizedString("Feeling/Activity", comment: ""),UIImage(named: "smileys")!),
            (NSLocalizedString("Camera", comment: ""),UIImage(named: "Cameras")!),
            (NSLocalizedString("GIF", comment: ""),UIImage(named: "file")!),
            ( NSLocalizedString("File", comment: ""),UIImage(named: "directory")!),
            ( NSLocalizedString("Music", comment: ""),UIImage(named: "Musics")!),
        ]
        self.tableView.separatorStyle = .none
        self.tableView.register(UINib(nibName: "SelectPostTypeTableItem", bundle: nil), forCellReuseIdentifier: "SelectPostTypeTableItem")
    }
    
    func openFeelingPicker(){
        ActionSheetStringPicker.show(withTitle: NSLocalizedString("What are you doing", comment: ""),
                                     rows: [NSLocalizedString("Feeling", comment: ""),
                                            NSLocalizedString("Listening", comment: ""),
                                            NSLocalizedString("Playing", comment: ""),
                                            NSLocalizedString("Watching", comment: ""),
                                            NSLocalizedString("Treveling", comment: ""),
                                            
            ],
                                     initialSelection: 0,
                                     doneBlock: { (picker, value, index) in
                                        
                                        self.delegate?.didselectPostType(type: "FEELING",feelingType:index as? String,FeelingTypeString:nil)
                                        
                                        return
                                        
                                        
                                        
        }, cancel:  { ActionStringCancelBlock in return }, origin:self.view)
        
    }
}
extension PostTypeVC : UITableViewDelegate , UITableViewDataSource {
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.list.count ?? 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "SelectPostTypeTableItem") as! SelectPostTypeTableItem
        cell.selectionStyle = .none
        
        cell.bind(image: self.list[indexPath.row].1, title: self.list[indexPath.row].0)
        
        return cell
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        self.dismiss(animated: true) {
            switch indexPath.row {
            case 0:
                self.delegate?.didselectPostType(type: "IMAGE",feelingType:nil,FeelingTypeString:nil)
                break
            case 1:
                self.delegate?.didselectPostType(type: "VIDEO",feelingType:nil,FeelingTypeString:nil)
                break
            case 2:
                self.delegate?.didselectPostType(type: "MENTIONCONTACT",feelingType:nil,FeelingTypeString:nil)
                break
//            case 3:
//                self.delegate?.didselectPostType(type: "LOCATION",feelingType:nil,FeelingTypeString:nil)
//                break
                
            case 3:
                self.openFeelingPicker()
                break
                
            case 4:
                self.delegate?.didselectPostType(type: "CAMERA",feelingType:nil,FeelingTypeString:nil)
                break
                
            case 5:
                self.delegate?.didselectPostType(type: "GIF",feelingType:nil,FeelingTypeString:nil)
                break
                
            case 6:
                self.delegate?.didselectPostType(type: "FILE",feelingType:nil,FeelingTypeString:nil)
                break
                
            case 7:
                self.delegate?.didselectPostType(type: "MUSIC",feelingType:nil,FeelingTypeString:nil)
                break
                
                
            default:
                self.delegate?.didselectPostType(type: "FEELING",feelingType:nil,FeelingTypeString:nil)
                break
            }
        }
    }
    
}
