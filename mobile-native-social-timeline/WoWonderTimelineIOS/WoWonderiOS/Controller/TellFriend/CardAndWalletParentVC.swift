

import UIKit
import WoWonderTimelineSDK
import XLPagerTabStrip
class CardAndWalletParentVC: ButtonBarPagerTabStripViewController {
    
    override func viewDidLoad() {
        self.setupUI()
        super.viewDidLoad()
        
        
    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        self.tabBarController?.tabBar.isHidden = false
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.tabBarController?.tabBar.isHidden = true
    }
    private func setupUI(){
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
            navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("My Balance", comment: "My Balance")
        
        let lineColor = UIColor.black
        settings.style.buttonBarItemBackgroundColor = .clear
        settings.style.selectedBarBackgroundColor = lineColor
        settings.style.buttonBarItemFont =  UIFont.systemFont(ofSize: 15.0)
        settings.style.selectedBarHeight = 2
        settings.style.buttonBarMinimumLineSpacing = 0
        settings.style.buttonBarItemTitleColor = .black
        settings.style.buttonBarItemsShouldFillAvailableWidth = true
        settings.style.buttonBarLeftContentInset = 0
        settings.style.buttonBarRightContentInset = 0
        let color = UIColor(red:26/255, green: 34/255, blue: 78/255, alpha: 0.4)
        let newCellColor = UIColor.black
        changeCurrentIndexProgressive = { [weak self] (oldCell: ButtonBarViewCell?, newCell: ButtonBarViewCell?, progressPercentage: CGFloat, changeCurrentIndex: Bool, animated: Bool) -> Void in
            guard changeCurrentIndex == true else { return }
            oldCell?.label.textColor = color
            newCell?.label.textColor = newCellColor
            print("OldCell",oldCell)
            print("NewCell",newCell)
        }
        
    }
    override func viewControllers(for pagerTabStripController: PagerTabStripViewController) -> [UIViewController] {
        let storyboard = UIStoryboard(name: "TellFriend", bundle: nil)
        
        let SendMoneyVC =  storyboard.instantiateViewController(withIdentifier: "SendMoneyVC") as! SendMoneyVC
        let AddFundsVC =  storyboard.instantiateViewController(withIdentifier: "AddFundsVC") as! AddFundsVC
        return [SendMoneyVC,AddFundsVC]
        
    }
    
}
