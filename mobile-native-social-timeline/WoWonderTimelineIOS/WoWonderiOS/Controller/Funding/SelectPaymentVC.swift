

import UIKit
import WoWonderTimelineSDK


class SelectPaymentVC: UIViewController {
    
    @IBOutlet weak var tableVIew: UITableView!
    
    @IBOutlet weak var closeBtn: UIButton!
    
    
    let selectPaymentArray = [
        "Paypal","Bank Transfer"
    
    ]
    var delegate : didSelectPaymentTypeDelegate?
    override func viewDidLoad() {
        super.viewDidLoad()
        self.tableVIew.separatorStyle = .none
                  tableVIew.register(UINib(nibName: "SelectAmountTableItem", bundle: nil), forCellReuseIdentifier: "SelectAmountTableItem")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
    }
    @IBAction func cancelPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
}
extension SelectPaymentVC:UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return self.selectPaymentArray.count ?? 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "SelectAmountTableItem") as? SelectAmountTableItem
        cell?.bind(self.selectPaymentArray[indexPath.row])
        return cell!
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        self.dismiss(animated: true) {
            self.delegate?.didSelectPaymentType(typeString: self.selectPaymentArray[indexPath.row], index: indexPath.row)
        }
    }
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 50.0
    }
}
