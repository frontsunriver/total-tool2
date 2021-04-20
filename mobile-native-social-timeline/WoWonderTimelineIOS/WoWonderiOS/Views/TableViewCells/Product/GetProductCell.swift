

import WoWonderTimelineSDK
import UIKit

class GetProductCell: UICollectionViewCell,UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout {
 
    @IBOutlet weak var collectionView: UICollectionView!

    let status = Reach().connectionStatus()
    
    var footerView :CustomFooterView?
    var isLoading:Bool = false
    let footerViewReuseIdentifier = "RefreshFooterView"
     
    var products = [[String:Any]]()
    var didSelectItemAction: ((IndexPath) -> Void)?
    var reachedTop : (()->Void)? = nil
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
        
        self.collectionView.register(UINib(nibName: "CustomFooterView", bundle: nil), forSupplementaryViewOfKind: UICollectionView.elementKindSectionFooter, withReuseIdentifier: footerViewReuseIdentifier)
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.products.count
     }
     
     func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "Market&MyProductscell", for: indexPath) as! MarketAndMyProductCell
        
        let index = self.products[indexPath.row]
         
         if let name = index["name"] as? String{
            cell.productName.text! = name
         }
         if let location = index["location"] as? String{
             cell.location.text! = location
         }
         if let posted = index["time_text"] as? String{
            cell.postedTime.text! = posted
         }
         if let price = index["price"] as? String{
            cell.priceLabel.text! = "\("$")\(" ")\(price)"
         }
         if let seller  = index["seller"] as? [String:Any]{
             if let username = seller["name"] as? String{
                cell.sellerName.text! = username
             }
             if let userImage = seller["avatar"] as? String{
                 let url = URL(string: userImage)
                cell.sellerProfileIcon.kf.setImage(with: url)
            }
         }
        
         if let productImages = index["images"] as? [[String:Any]] {
             if let image = productImages.first!["image"] as? String{
                 let url = URL(string: image)
                 cell.productImage.kf.setImage(with: url)
             }
         }
        
        return cell
     }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
         didSelectItemAction?(indexPath)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        
        if (indexPath.item == 0 || indexPath.item % 7==0){
                 return CGSize(width: collectionView.frame.size.width, height: 280.0)
             }
             else{
                 let padding: CGFloat = 10
                 let collectionViewSize = collectionView.frame.size.width - padding
                 return CGSize(width: collectionViewSize/2, height: 290.0)
             }

     }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, referenceSizeForFooterInSection section: Int) -> CGSize {
        if isLoading {
            return CGSize.zero
        }
        return CGSize(width: collectionView.bounds.size.width, height: 55)
    }
    
    func collectionView(_ collectionView: UICollectionView, viewForSupplementaryElementOfKind kind: String, at indexPath: IndexPath) -> UICollectionReusableView {
        if kind == UICollectionView.elementKindSectionFooter {
            let aFooterView = collectionView.dequeueReusableSupplementaryView(ofKind: kind, withReuseIdentifier: footerViewReuseIdentifier, for: indexPath) as! CustomFooterView
            self.footerView = aFooterView
            self.footerView?.backgroundColor = UIColor.clear
            return aFooterView
        } else {
            let headerView = collectionView.dequeueReusableSupplementaryView(ofKind: kind, withReuseIdentifier: footerViewReuseIdentifier, for: indexPath)
            return headerView
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, willDisplaySupplementaryView view: UICollectionReusableView, forElementKind elementKind: String, at indexPath: IndexPath) {
           if elementKind == UICollectionView.elementKindSectionFooter {
               self.footerView?.prepareInitialAnimation()
           }
       }
    
    func collectionView(_ collectionView: UICollectionView, didEndDisplayingSupplementaryView view: UICollectionReusableView, forElementOfKind elementKind: String, at indexPath: IndexPath) {
        if elementKind == UICollectionView.elementKindSectionFooter {
            self.footerView?.stopAnimate()
        }
    }
    
    
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
        if scrollView.contentOffset.y  < 0 {
          reachedTop!()
        }
        
        let threshold   = 100.0 ;
        let contentOffset = scrollView.contentOffset.y;
        let contentHeight = scrollView.contentSize.height;
        let diffHeight = contentHeight - contentOffset;
        let frameHeight = scrollView.bounds.size.height;
        var triggerThreshold  = Float((diffHeight - frameHeight))/Float(threshold);
        triggerThreshold   =  min(triggerThreshold, 0.0)
        let pullRatio  = min(fabs(triggerThreshold),1.0);
        self.footerView?.setTransform(inTransform: CGAffineTransform.identity, scaleFactor: CGFloat(pullRatio))
        if pullRatio >= 1 {
            self.footerView?.animateFinal()
        }
        print("pullRation:\(pullRatio)")
    }
    
    
    //compute the offset and call the load method
    func scrollViewDidEndDecelerating(_ scrollView: UIScrollView) {
        let contentOffset = scrollView.contentOffset.y;
        let contentHeight = scrollView.contentSize.height;
        let diffHeight = contentHeight - contentOffset;
        let frameHeight = scrollView.bounds.size.height;
        let pullHeight  = abs(diffHeight - frameHeight);
        print("pullHeight:\(pullHeight)");
        if pullHeight == 0.0
        {
            if (self.footerView?.isAnimatingFinal)! {
                print("load more trigger")
                self.isLoading = true
                self.footerView?.startAnimate()
                Timer.scheduledTimer(withTimeInterval: 2, repeats: false, block: { (timer:Timer) in
                    print ("LoadData")
//                    for i:Int in self.items.count + 1...self.items.count + 25 {
//                        self.items.append(i)
//                    }
                    self.collectionView.reloadData()
                    self.isLoading = false
                })
            }
        }
    }
}
