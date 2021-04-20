

import UIKit
import WoWonderTimelineSDK


class CurrencyController: UIViewController {
    
    
    @IBOutlet weak var selectCurrencyLbl: UILabel!
    
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate : JobCurrencyDelegate!

    override func viewDidLoad() {
        super.viewDidLoad()
        self.selectCurrencyLbl.text = NSLocalizedString("Select Currency", comment: "Select Currency")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
    }
    

    @IBAction func SelectCurrency(_ sender: UIButton) {
        switch sender.tag {
        case 0:
            self.delegate.jobCurrency(currency: "$", currencyId: "0")
            self.dismiss(animated: true, completion: nil)
        case 1:
            self.delegate.jobCurrency(currency: "€", currencyId: "1")
            self.dismiss(animated: true, completion: nil)
        case 2:
            self.delegate.jobCurrency(currency: "₺", currencyId: "2")
            self.dismiss(animated: true, completion: nil)
        case 3:
            self.delegate.jobCurrency(currency: "£", currencyId: "3")
            self.dismiss(animated: true, completion: nil)
        case 4:
            self.delegate.jobCurrency(currency: "руб", currencyId: "4")
            self.dismiss(animated: true, completion: nil)
        case 5:
            self.delegate.jobCurrency(currency: "zl", currencyId: "5")
            self.dismiss(animated: true, completion: nil)
        case 6:
            self.delegate.jobCurrency(currency: "₪", currencyId: "6")
            self.dismiss(animated: true, completion: nil)
        case 7:
            self.delegate.jobCurrency(currency: "R$", currencyId: "7")
            self.dismiss(animated: true, completion: nil)
        case 8:
            self.delegate.jobCurrency(currency: "₹", currencyId: "8")
            self.dismiss(animated: true, completion: nil)
            
        default:
            print("Default")
        }
    }
    

    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
        self.delegate.jobCurrency(currency: "", currencyId: "")
    }
    
}
