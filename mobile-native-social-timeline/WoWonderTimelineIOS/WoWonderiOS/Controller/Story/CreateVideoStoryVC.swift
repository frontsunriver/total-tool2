
import UIKit
import MMPlayerView
import AVFoundation
import SDWebImage
import WoWonderTimelineSDK
class CreateVideoStoryVC: UIViewController {
    
    @IBOutlet weak var sendBtn: UIButton!
    @IBOutlet weak var captionTxtView: UITextView!
    @IBOutlet weak var contentImgView: UIImageView!
    
    
    var videoLinkString:String? = ""
    private var aspectConstraint: NSLayoutConstraint?
    
    
    lazy var mmPlayerLayer: MMPlayerLayer = {
        let l = MMPlayerLayer()
        l.cacheType = .memory(count: 200)
        l.coverFitType = .fitToPlayerView
        l.videoGravity = AVLayerVideoGravity.resizeAspect
        return l
    }()
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        setupUI()
        
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.tabBarController?.tabBar.isHidden = true
    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewDidDisappear(animated)
        self.tabBarController?.tabBar.isHidden = false
        
    }
    
    @IBAction func sendPressed(_ sender: Any) {
        self.createStory()
    }
    
    func setupUI(){
        self.title = "Add Story"
        
        self.mmPlayerLayer.playView = self.contentImgView
        mmPlayerLayer.replace(cover:  CoverView.instantiateFromNib())
        mmPlayerLayer.showCover(isShow: true)
        
        self.mmPlayerLayer.set(url: URL(string:(videoLinkString!)))
        self.mmPlayerLayer.resume()
        
        mmPlayerLayer.getStatusBlock { [weak self] (status) in
            switch status {
            case .failed( _):
                print("Failed")
            case .ready:
                print("Ready to Play")
            case .playing:
                print("Playing")
            case .pause:
                print("Pause")
            case .end:
                print("End")
            default: break
            }
        }
    }
    
    private func createStory(){
        let text = self.captionTxtView.text ?? ""
        
        let videoData = try? Data(contentsOf: URL(string:(self.videoLinkString ?? ""))!)
        
        StoriesManager.sharedInstance.createStory(story_data: videoData, mimeType: videoData!.mimeType, type: "video", text: text, completionBlock: { (success, sessionError, error) in
            if success != nil{
                
                
                print("success = \(success?.apiStatus ?? 0)")
//                self.view.makeToast(success?.apiStatus ?? 0)
                self.navigationController?.popViewController(animated: true)
                
            }else if sessionError != nil{
                print("sessionError = \(sessionError?.errors?.errorText)")
                self.view.makeToast(sessionError?.errors?.errorText ?? "")
                
            }else {
                print("error = \(error?.localizedDescription)")
                self.view.makeToast(error?.localizedDescription ?? "")
                
            }
        })
        
    }
    
    
}
