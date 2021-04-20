//
//  CouponsViewController.swift
//  Rider
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit


class CouponsCollectionViewController: UICollectionViewController, UICollectionViewDelegateFlowLayout {
    //MARK: Properties
    let cellIdentifier = "CouponsCollectionViewCell"
    var selectMode = false
    var coupons = [Coupon]()
    var delegate:CouponsViewDelegate?
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        let nibCell = UINib(nibName: cellIdentifier, bundle: nil)
        collectionView?.register(nibCell, forCellWithReuseIdentifier: cellIdentifier)
        self.refreshList(self)
    }
    
    @IBAction func onAddCouponClicked(_ sender: UIBarButtonItem) {
        let title = NSLocalizedString("Add Coupon", comment: "Add Coupon message title")
        let message = NSLocalizedString("Enter your coupon code",comment: "")
        let dialog = UIAlertController(title: title, message: message, preferredStyle: .alert)
        dialog.addTextField() { textField in
            textField.placeholder = NSLocalizedString("Coupon Code",comment: "")
        }
        dialog.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "Message OK button"), style: .default) { action in
            AddCoupon(code: dialog.textFields![0].text!).execute() { result in
                switch result {
                case .success(_):
                    self.refreshList(self)
                    
                case .failure(let error):
                    error.showAlert()
                }
            }
        })
        dialog.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: "Message Cancel Button"), style: .cancel))
        self.present(dialog, animated: true)
    }
    
    @IBAction func refreshList(_ sender: Any) {
        GetCoupons().execute() { result in
            switch result {
            case .success(let response):
                self.coupons = response
                self.collectionView?.reloadData()
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    override func numberOfSections(in tableView: UICollectionView) -> Int {
        return 1
    }

    override func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return coupons.count
    }
    override func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        if selectMode {
            if delegate != nil {
                delegate?.didSelectedCoupon(coupons[indexPath.row])
            }
            self.navigationController?.popViewController(animated: true)
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let kWhateverHeightYouWant = 65
        return CGSize(width: collectionView.bounds.size.width, height: CGFloat(kWhateverHeightYouWant))
    }
    
    override func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        guard let cell = self.collectionView?.dequeueReusableCell(withReuseIdentifier: cellIdentifier, for: indexPath) as? CouponsCollectionViewCell  else {
            fatalError("The dequeued cell is not an instance of CouponsCollectionViewCell.")
        }
        // Fetches the appropriate meal for the data source layout.
        let coupon = coupons[indexPath.row]
        cell.coupon = coupon
        //cell.background.colorId = indexPath.row
        
        return cell
    }
}
protocol CouponsViewDelegate {
    // Classes that adopt this protocol MUST define
    // this method -- and hopefully do something in
    // that definition.
    func didSelectedCoupon(_ coupon: Coupon)
}
