
import UIKit
import XLPagerTabStrip
import Toast_Swift
import WoWonderTimelineSDK


class GetProductsController: UIViewController,ProductDistanceDelegate{

    @IBOutlet weak var gradientView: UIView!
    @IBOutlet weak var searchBar: UISearchBar!
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var productsCollectionView: UICollectionView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var stackView: UIStackView!
    @IBOutlet weak var marketBtn: UIButton!
    @IBOutlet weak var productBtn: UIButton!
    @IBOutlet weak var noProductView: UIView!
    @IBOutlet weak var scrollView: UIScrollView!
    @IBOutlet weak var contentView: UIView!
    @IBOutlet weak var contentViewHeight: NSLayoutConstraint!
    
    let status = Reach().connectionStatus()
    let screenHeight = UIScreen.main.bounds.height
    
    let Storyboard = UIStoryboard(name: "MarketPlaces-PopularPost-Events", bundle: nil)
    
    var categoryArray = ["Others","Autos & Vehicles","Baby & Children's Product","Beauty Products & Services","Computers & Peripherals","Consumer Electronics","Dating Services","Financial Services","Gifts & Occasions","Home & Garden"]
    var marketProducts = [[String:Any]]()
    var myProducts = [[String:Any]]()
    var offset = ""
    var myProductoffset = ""
    var categoryId = "0"
    var distance = 0
    var flag = false
    var indicatorView = UIView()
    let indicatorHeight : CGFloat = 4.0
    var selectIndex : Int? = nil
    var selectedIndex = 0
    var proCollectionIndex = 1
    

    override func viewDidLoad() {
        super.viewDidLoad()
    NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
    self.navigationController?.navigationBar.isHidden = true
        self.noProductView.isHidden = true
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
        self.productsCollectionView.delegate = self
        self.productsCollectionView.dataSource = self
        self.contentViewHeight.constant = self.screenHeight + 100.0
        self.scrollView.delegate = self
        self.searchBar.delegate = self
//        self.searchBar.placeholder = NSLocalizedString("Search", comment: "Search")
        self.searchBar.backgroundImage = UIImage()
        self.activityIndicator.startAnimating()
        indicatorView.backgroundColor = UIColor.hexStringToUIColor(hex: "#FFFFFF")
        indicatorView.frame = CGRect(x: self.stackView.bounds.minX, y: self.stackView.bounds.maxY - indicatorHeight, width: self.stackView.bounds.width / 2.5, height: indicatorHeight)
        self.stackView.addSubview(indicatorView)
        self.getMarketProduts(userId: "", categoryId: "0", distance: 0, keyword: "")
        self.getMyProduts(userId: UserData.getUSER_ID()!, categoryId: "", distance: 0, keyword: "")
        self.SetUpSearchField()
        self.navigationController?.interactivePopGestureRecognizer?.isEnabled = false
        self.marketBtn.setTitle(NSLocalizedString("MARKET", comment: "MARKET"), for: .normal)
        self.productBtn.setTitle(NSLocalizedString("MY PRODUCTS", comment: "MY PRODUCTS"), for: .normal)
    }
    
    private func SetUpSearchField(){
        if let textfield = self.searchBar.value(forKey: "searchField") as? UITextField {
            textfield.backgroundColor = .clear
            //                UIColor.hexStringToUIColor(hex: "#984243")
            textfield.attributedPlaceholder = NSAttributedString(string:NSLocalizedString("Search...", comment: "Search..."), attributes:[NSAttributedString.Key.foregroundColor: UIColor.yellow])
            textfield.textColor = .white
            if let leftView = textfield.leftView as? UIImageView {
                leftView.image = leftView.image?.withRenderingMode(.alwaysTemplate)
                leftView.tintColor = UIColor.white
            }
        }
    }

    override func viewWillAppear(_ animated: Bool) {
        self.setGradientBackground()
    }
    
    override func viewDidAppear(_ animated: Bool) {
    }
    
    func setGradientBackground() {
        let colorTop =  UIColor.hexStringToUIColor(hex: "#eb5757").cgColor
        let colorBottom = UIColor.hexStringToUIColor(hex: "#000000").cgColor

        let gradientLayer = CAGradientLayer()
        gradientLayer.colors = [colorTop, colorBottom]
        gradientLayer.locations = [0.0, 1.0]
//        gradientLayer.frame = self.gradientView.bounds
//        self.gradientView.layer.addSublayer(gradientLayer)
        
    }
    
    
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    @IBAction func Distance(_ sender: Any) {
    let vc = Storyboard.instantiateViewController(withIdentifier: "ProductDistanceVC") as! ProductDistanceController
    vc.delegate = self
    vc.distance = "\(self.distance)"
    vc.modalPresentationStyle = .overCurrentContext
    vc.modalTransitionStyle = .crossDissolve
    self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func AddProduct(_ sender: Any) {
        let vc = Storyboard.instantiateViewController(withIdentifier: "CreateProductVC") as! CreateProductController
        vc.delegate = self
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
        self.present(vc, animated: true, completion: nil)
    }
    
    
    private func getMarketProduts(userId :String, categoryId :String,distance :Int, keyword :String) {
        switch status {
        case .unknown, .offline:
            self.activityIndicator.stopAnimating()
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            performUIUpdatesOnMain {
                GetProductsManager.sharedInstance.getProducts(userId: userId, categoryId: categoryId, distance: distance, offset: self.offset, keyword: keyword) { (success, authError, error) in
                    if (success != nil) {
                        print(success?.products)
                        for i in success!.products{
                            self.marketProducts.append(i)
                        }
                    self.offset = self.marketProducts.last?["id"] as? String ?? "0"
                    self.productsCollectionView.reloadData()
                    self.activityIndicator.stopAnimating()
                     }
                    else if (authError != nil) {
                        self.activityIndicator.stopAnimating()
                       self.view.makeToast(authError?.errors.errorText)
                    }
                    else if error != nil {
                        print(error?.localizedDescription)
                    }
                }
            }
            
        }
    }
    
    private func getMyProduts(userId :String, categoryId :String,distance :Int, keyword :String) {
        GetProductsManager.sharedInstance.getProducts(userId: userId, categoryId: categoryId, distance: distance, offset: self.myProductoffset, keyword: keyword) { (success, authError, error) in
             if (success != nil) {
                
                 print(success?.products)
                 for i in success!.products{
                    self.myProducts.append(i)
                 }
            self.offset = self.myProducts.last?["id"] as? String ?? "0"
             self.productsCollectionView.reloadData()
        
              }
             else if (authError != nil) {
                self.view.makeToast(authError?.errors.errorText)
             }
             else if error != nil {
                 print(error?.localizedDescription)
             }
         }
     }
    
    
    
    @IBAction func MarketProductBtn(_ sender: Any) {
        
        self.productsCollectionView.scrollToItem(at: IndexPath.init(item: 0, section: 0), at: .left, animated: true)
        self.productBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
        self.marketBtn.setTitleColor(.white, for: .normal)
 
        let desiredX = (self.stackView.bounds.width / 2.0 ) * CGFloat(0)
        
        UIView.animate(withDuration: 0.3) {
            self.indicatorView.frame = CGRect(x: 0, y: self.stackView.bounds.maxY - self.indicatorHeight, width:(self.stackView.bounds.width / 2.0 ), height: self.indicatorHeight)
        }
    }
    
    @IBAction func MyProductsBtn(_ sender: Any) {
       self.productsCollectionView.scrollToItem(at: IndexPath.init(item: 1, section: 0), at: .right, animated: true)
       self.marketBtn.setTitleColor(UIColor.hexStringToUIColor(hex: "DAC2C0"), for: .normal)
       self.productBtn.setTitleColor(.white, for: .normal)

        let desiredX = (self.stackView.bounds.width / 2.0 ) * CGFloat(1)
               
               UIView.animate(withDuration: 0.3) {
                   self.indicatorView.frame = CGRect(x: desiredX, y: self.stackView.bounds.maxY - self.indicatorHeight, width:(self.stackView.bounds.width / 2.0 ), height: self.indicatorHeight)
               }
    }
    
}

extension GetProductsController : UICollectionViewDelegate, UICollectionViewDataSource, UICollectionViewDelegateFlowLayout,UIScrollViewDelegate,UISearchBarDelegate,CreateProductDelegate{

    
   func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
    if (collectionView == self.collectionView){
        return self.categoryArray.count
    }
    else {
        return 2
        }
    }
    
 func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
    if (collectionView == self.collectionView){
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "Productcategory", for: indexPath) as! ProductCategoryCell
        let index = self.categoryArray[indexPath.row]

        if let indx = self.selectIndex{
            if indx == indexPath.item{
                cell.categoryLabel.textColor = UIColor.hexStringToUIColor(hex: "984243")
                cell.categoryView.borderColor = UIColor.hexStringToUIColor(hex: "984243")
            }
            else{
                cell.categoryLabel.textColor = .white
                cell.categoryView.borderColor = .white
            }
            
        }
        
        
        cell.categoryLabel.text! = index
        return cell
      }
    else {
        //print(indexPath.item)
        if (indexPath.item == 0){
            let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "GetProductcell", for: indexPath) as! GetProductCell
            cell.products = self.marketProducts
            cell.collectionView.isScrollEnabled = self.flag
            cell.didSelectItemAction = { [weak self] indexPath in
                self?.selectedIndex = indexPath.item
                self?.gotoProductDetail(indexPath: indexPath)
            }
            cell.reachedTop = {
                self.flag = false
                self.productsCollectionView.reloadData()
            }
            cell.collectionView.reloadData()
            
            return cell
          }
        else {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "GetProductcell", for: indexPath) as! GetProductCell
            cell.products = self.myProducts
            cell.collectionView.isScrollEnabled = self.flag
            cell.didSelectItemAction = { [weak self] indexPath in
                self?.selectedIndex = indexPath.item
                self?.gotoMyProductDetail(indexPath: indexPath)
            }
            cell.collectionView.reloadData()
            return cell
         }
       }
    }

    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        if (collectionView == self.collectionView){
            let index = indexPath.item
            self.selectIndex = index
            let selectedCell = self.collectionView.cellForItem(at: indexPath) as! ProductCategoryCell
            selectedCell.categoryLabel.textColor = UIColor.hexStringToUIColor(hex: "984243")
            selectedCell.categoryView.borderColor = UIColor.hexStringToUIColor(hex: "984243")
            self.categoryId = "\(indexPath.item + 1)"
            self.marketProducts.removeAll()
            self.productsCollectionView.reloadData()
            self.collectionView.reloadData()
            self.activityIndicator.startAnimating()
            self.getMarketProduts(userId: "", categoryId: self.categoryId, distance: self.distance, keyword: "")
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
    
        if (collectionView == self.collectionView){
            print ("size",UICollectionViewFlowLayout.automaticSize)
            return CGSize(width: 150, height: 30)
        }
        else {
            let padding: CGFloat = 0
        let collectionViewSize = self.productsCollectionView.frame.size.width - padding
        return CGSize(width: collectionViewSize, height:self.productsCollectionView.frame.size.height )
        }
    
    }
   
    func searchBarSearchButtonClicked(_ searchBar: UISearchBar) {
        self.marketProducts.removeAll()
        self.productsCollectionView.reloadData()
        self.searchBar.searchTextField.resignFirstResponder()
        self.offset = ""
        self.activityIndicator.startAnimating()
        self.getMarketProduts(userId: "", categoryId: self.categoryId, distance: self.distance, keyword: self.searchBar.searchTextField.text!)
   }
    
  
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
      if scrollView.contentOffset.y >= 114.0 {
            self.flag = true
            self.productsCollectionView.reloadData()
        }
        
        if scrollView == productsCollectionView{
            
            UIView.animate(withDuration: 0.3) {
                self.indicatorView.frame = CGRect(x: scrollView.contentOffset.x / 2, y: self.stackView.bounds.maxY - self.indicatorHeight, width:(self.stackView.bounds.width / 2.0 ), height: self.indicatorHeight)
            }
        }
    
    }
    
    func scrollViewDidEndDecelerating(_ scrollView: UIScrollView) {
        
        var visibleRect = CGRect()

        visibleRect.origin = self.productsCollectionView.contentOffset
        visibleRect.size = self.collectionView.bounds.size

        let visiblePoint = CGPoint(x: visibleRect.midX, y: visibleRect.midY)
        guard let indexPath = collectionView.indexPathForItem(at: visiblePoint) else { return }
        self.proCollectionIndex = indexPath.last ?? 0
        print(indexPath.last ?? 0)
    
    }
    func estimatedFrame(text: String, font: UIFont) -> CGRect {
        let size = CGSize(width: 500, height: 1000) // temporary size
        let options = NSStringDrawingOptions.usesFontLeading.union(.usesLineFragmentOrigin)
        return NSString(string: text).boundingRect(with: size,
                                                   options: options,
                                                   attributes: [NSAttributedString.Key.font: font],
                                                   context: nil)
    }
    
    func createProduct() {
        self.activityIndicator.startAnimating()
        self.marketProducts.removeAll()
        self.myProducts.removeAll()
        self.productsCollectionView.reloadData()
        self.getMarketProduts(userId: "", categoryId: self.categoryId, distance: self.distance, keyword: "")
        self.getMyProduts(userId: UserData.getUSER_ID()!, categoryId:self.categoryId, distance: self.distance, keyword: "")

    }
    
    func productDistance(distance: Int) {
        self.distance = distance
        self.offset = ""
        self.activityIndicator.startAnimating()
        self.marketProducts.removeAll()
        self.productsCollectionView.reloadData()
        self.getMarketProduts(userId: "", categoryId: self.categoryId, distance: self.distance, keyword: "")
    }
    
    @objc func gotoProductDetail(indexPath :IndexPath){
        let index =  self.marketProducts[self.selectedIndex]
        let vc = Storyboard.instantiateViewController(withIdentifier: "ProductDetailsVC") as! ProductDetailController
        vc.productDetails = index
        self.navigationController?.pushViewController(vc, animated: true)
               
    }
    
    @objc func gotoMyProductDetail(indexPath :IndexPath){
        let index =  self.myProducts[self.selectedIndex]
        let vc = Storyboard.instantiateViewController(withIdentifier: "ProductDetailsVC") as! ProductDetailController
        vc.productDetails = index
        self.navigationController?.pushViewController(vc, animated: true)
    }
}
