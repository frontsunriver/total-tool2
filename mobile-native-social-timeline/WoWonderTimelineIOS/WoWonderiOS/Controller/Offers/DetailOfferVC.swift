import WoWonderTimelineSDK
import UIKit


class DetailOfferVC: UIViewController {

    @IBOutlet weak var tableView: UITableView!
    
    var object:GetOffersModel.Datum?
    
    override func viewDidLoad() {
        super.viewDidLoad()
         self.setupUI()
        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = false
    }
    override func viewWillAppear(_ animated: Bool) {
           super.viewWillAppear(animated)
          
       }
       private func setupUI(){
        self.navigationController?.navigationBar.isHidden = true
           self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
           
           let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
           navigationController?.navigationBar.titleTextAttributes = textAttributes
           self.navigationItem.title = NSLocalizedString("Offers", comment: "Offers")
           
           self.tableView.separatorStyle = .none
           tableView.register(UINib(nibName: "DetailOfferTableItem", bundle: nil), forCellReuseIdentifier: "DetailOfferTableItem")
       
    }
    
}
extension DetailOfferVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableView.automaticDimension
    }
}


extension DetailOfferVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return 1
        
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "DetailOfferTableItem") as! DetailOfferTableItem
        cell.selectionStyle = .none
        cell.bind(object!,index:indexPath.row)
        cell.backBtn.addTarget(self, action: #selector(self.Back(sender:)), for: .touchUpInside)
        cell.vc = self
        
        return cell
    }
    
    @IBAction func Back(sender: UIButton){
        self.navigationController?.navigationBar.isHidden = false
        self.navigationController?.popViewController(animated: true)
    }
    
    //    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
    //
    //        let storyboard = UIStoryboard(name: "General", bundle: nil)
    //        let vc = storyboard.instantiateViewController(withIdentifier: "UpdateBlockUnBlockVC") as! UpdateBlockUnBlockVC
    //        vc.object = self.blockedUserArray[indexPath.row]
    //        self.present(vc, animated: true, completion: nil)
    //    }
}
