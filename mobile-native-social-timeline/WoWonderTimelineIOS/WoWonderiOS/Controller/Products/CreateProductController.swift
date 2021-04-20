

import UIKit
import Toast_Swift
import ZKProgressHUD
import WoWonderTimelineSDK

class CreateProductController: UIViewController {
    
    
    @IBOutlet weak var productName: RoundTextField!
    @IBOutlet weak var productPrice: RoundTextField!
    @IBOutlet weak var currency: RoundTextField!
    @IBOutlet weak var location: RoundTextView!
    @IBOutlet weak var category: RoundTextField!
    @IBOutlet weak var productDescription: RoundTextView!
    @IBOutlet weak var newProductView: DesignView!
    @IBOutlet weak var oldProductView: DesignView!
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var sellLabel: UILabel!
    @IBOutlet weak var sellDesc: UILabel!
    @IBOutlet weak var createLabel: UILabel!
    @IBOutlet weak var addBtn: UIButton!
    @IBOutlet weak var usedBtn: UIButton!
    @IBOutlet weak var newBtn: UIButton!
    
    var delegate :CreateProductDelegate!
    
    var currencyId = ""
    var categoryId = ""
    var productType = ""
   
    
    var images = [UIImage]()
    var productImages = [Data]()
    
    let status = Reach().connectionStatus()

    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationController?.navigationBar.isHidden = true
        NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
        self.createLabel.text = NSLocalizedString("Create New Product", comment: "Create New Product")
        self.sellLabel.text = NSLocalizedString("Sell New Product", comment: "Sell New Product")
        self.sellDesc.text = NSLocalizedString("You can list product for sale from the Add a Product tool.", comment: "You can list product for sale from the Add a Product tool.")
        self.addBtn.setTitle(NSLocalizedString("Add", comment: "Add"), for: .normal)
        self.newBtn.setTitle(NSLocalizedString("New", comment: "New"), for: .normal)
        self.usedBtn.setTitle(NSLocalizedString("Used", comment: "Used"), for: .normal)
        self.productName.placeholder = NSLocalizedString("Product name", comment: "Product name")
        self.productPrice.placeholder = NSLocalizedString("Product price", comment: "Product price")
        self.currency.placeholder = NSLocalizedString("Currency", comment: "Currency")
        self.location.text = NSLocalizedString("Location", comment: "Location")
        self.category.placeholder = NSLocalizedString("Select A Category", comment: "Select A Category")
        self.productDescription.placeholder = NSLocalizedString("Product Description", comment: "Product Description")

        
    }
    
    
    
    
    private func createProduct(data : [Data]?) {
        
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
            performUIUpdatesOnMain {
                CreateProductManager.sharedInstance.createProduct(producName: self.productName.text!, price: Int(self.productPrice.text!) ?? 10, currency: self.currencyId, location: self.location.text!, categoryId: self.categoryId, description: self.productDescription.text!, type: self.productType, data: data) { (success, authError, error) in
                    if success != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast("Product Successfully Published")
                        self.dismiss(animated: true, completion: {
                            self.delegate.createProduct()
                        })
                    }
                    else if authError != nil {
                        ZKProgressHUD.dismiss()
                        self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        ZKProgressHUD.dismiss()
                        print(error?.localizedDescription)
                    }
                }
            }
        }
    }
    
    
    @IBAction func Currency(_ sender: Any) {
        self.currency.inputView = UIView()
        self.currency.inputAccessoryView = UIView()
        let Storyboard = UIStoryboard(name: "Jobs", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "CurrencyVC") as! CurrencyController
        vc.delegate = self
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func Category(_ sender: Any) {
        self.category.inputView = UIView()
        self.category.inputAccessoryView = UIView()
        let Storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
        let vc = Storyboard.instantiateViewController(withIdentifier: "ProductCategoryVC") as! ProductCategoryController
        vc.delegate = self
        vc.modalTransitionStyle = .crossDissolve
        vc.modalPresentationStyle = .overCurrentContext
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func New(_ sender: Any) {
        self.newProductView.backgroundColor = UIColor.hexStringToUIColor(hex:"#984243")
        self.oldProductView.backgroundColor = .white
        self.productType = "0"
    }
    
    @IBAction func Used(_ sender: Any) {
        self.oldProductView.backgroundColor = UIColor.hexStringToUIColor(hex:"#984243")
        self.newProductView.backgroundColor = .white
        self.productType = "1"
    }
    
    @IBAction func Back(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    @IBAction func Add(_ sender: Any) {
        if self.productName.text?.isEmpty == true {
            self.view.makeToast("Please Enter Product Name")
        }
        else if self.productPrice.text?.isEmpty == true{
            self.view.makeToast("Please Enter Product Price")
        }
        else if self.currency.text?.isEmpty == true{
            self.view.makeToast("Please Enter currency")
        }
        else if self.location.text.isEmpty == true{
            self.view.makeToast("Please Enter Location")
        }
        else if self.category.text?.isEmpty == true{
            self.view.makeToast("Please Enter Category")
        }
        else if self.productDescription.text.isEmpty == true{
            self.view.makeToast("Please Enter Description")
        }
        else if productType == "" {
            self.view.makeToast("Please Enter Product Type")
        }
        else if self.productImages.count == 0 {
            self.view.makeToast("Please Enter Product-Image")
        }
        else {
            self.createProduct(data: self.productImages)
        }
    }
}
extension CreateProductController :UICollectionViewDelegate,UICollectionViewDataSource,uploadImageDelegate,JobCurrencyDelegate,ProductCategoryDelegate{

    func numberOfSections(in collectionView: UICollectionView) -> Int {
        return 2
    }
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        if section == 0{
            return 1
        }
        else {
            return self.images.count
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        if indexPath.section == 0 {
            let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "AddImage", for: indexPath) as! AddImageCell
            return cell
        }
        else {
            let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "images", for: indexPath) as! ImagesCell
            let index = self.images[indexPath.row]
            cell.cancel.tag = indexPath.row
            cell.cancel.addTarget(self, action: #selector(imageCancel(sender:)), for: .touchUpInside)
            cell.albumImage.image = index
            return cell
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        if indexPath.section == 0{
            let Storyboard = UIStoryboard(name: "Main", bundle: nil)
            let vc = Storyboard.instantiateViewController(withIdentifier: "CropImageVC") as! CropImageController
            vc.delegate = self
            vc.imageType = "upload"
            vc.modalTransitionStyle = .coverVertical
            vc.modalPresentationStyle = .fullScreen
            self.present(vc, animated: true, completion: nil)
        }
    }
    
    func uploadImage(imageType: String, image: UIImage) {
        self.images.append(image)
        let image =  image.jpegData(compressionQuality: 0.1)
        self.productImages.append(image!)
        self.collectionView.reloadData()
        
    }
    
    @IBAction func imageCancel(sender:UIButton){
        let item = sender.tag
        self.images.remove(at: item)
        self.productImages.remove(at: item)
        self.collectionView.reloadData()
    }
    
    func jobCurrency(currency: String, currencyId: String) {
          self.currency.text = currency
          self.currencyId = currencyId
          self.currency.resignFirstResponder()
      }
    
    func category(categoryName: String, categoryId: String) {
        self.category.text = categoryName
        self.categoryId = categoryId
        self.category.resignFirstResponder()
    }
    
    
}
