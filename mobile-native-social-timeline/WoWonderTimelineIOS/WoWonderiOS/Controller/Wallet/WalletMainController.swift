
import UIKit
import WoWonderTimelineSDK

class WalletMainController: UIViewController {
    @IBOutlet weak var containerView1: UIView!
    @IBOutlet weak var containerView2: UIView!
    @IBOutlet weak var sendBtn: UIButton!
    @IBOutlet weak var fundBtn: UIButton!
    @IBOutlet weak var stackView: UIStackView!
    var mybalance: String? = nil
     var indicatorView = UIView()
        let indicatorHeight : CGFloat = 4.0
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
         self.navigationItem.title = NSLocalizedString("Wallet & Credits", comment: "Wallet & Credits")
         let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
         navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.containerView1.alpha = 1
        self.containerView2.alpha = 0
   indicatorView.backgroundColor = UIColor.hexStringToUIColor(hex: "#000000")
   indicatorView.frame = CGRect(x: self.stackView.bounds.minX, y: 0, width: self.stackView.bounds.width / 2, height: indicatorHeight)
   self.stackView.addSubview(indicatorView)

    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.tabBarController?.tabBar.isHidden = true

    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        self.tabBarController?.tabBar.isHidden = false
    }
    
    @IBAction func didTapButton(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.containerView1.alpha = 1
            self.containerView2.alpha = 0
            self.sendBtn.setImage(#imageLiteral(resourceName: "send-1"), for: .normal)
            self.sendBtn.setTitle("", for: .normal)
            self.fundBtn.setTitle(NSLocalizedString("AddFunds", comment: "AddFunds"), for: .normal)
            self.fundBtn.setImage(nil, for: .normal)
            UIView.animate(withDuration: 0.3) {
                self.indicatorView.frame = CGRect(x: 0, y: 0, width:(self.stackView.bounds.width / 2.0 ), height: self.indicatorHeight)
            }
        case 1:
            self.containerView1.alpha = 0
            self.containerView2.alpha = 1
            self.sendBtn.setTitle(NSLocalizedString("Send Money", comment: "Send Money"), for: .normal)
            self.sendBtn.setImage(nil, for: .normal)
            self.fundBtn.setTitle("", for: .normal)
            self.fundBtn.setImage(#imageLiteral(resourceName: "credit-card"), for: .normal)
            
            let desiredX = (self.stackView.bounds.width / 2.0 ) * CGFloat(1)
            UIView.animate(withDuration: 0.3) {
            self.indicatorView.frame = CGRect(x: desiredX, y: 0, width:(self.stackView.bounds.width / 2.0 ), height: self.indicatorHeight)
            }
        default:
            print("Nothing")
        }
    }
    
}
