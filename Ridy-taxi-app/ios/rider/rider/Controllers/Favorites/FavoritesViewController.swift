//
//  FavoritesViewController.swift
//  Rider
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import SPAlert

class FavoritesViewController: UICollectionViewController, UICollectionViewDelegateFlowLayout, FavoriteAddressDialogDelegate {
    func delete(address: Address) {
        DeleteAddress(id: address.id!).execute() { result in
            switch result {
            case .success(_):
                self.refreshList(self)
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    func update(address: Address) {
        let vc = FavoriteAddressDialogViewController(nibName: "FavoriteAddressDialogViewController", bundle: nil)
        vc.address = address
        vc.preferredContentSize = CGSize(width: 1000,height: 400)
        let dialog = UIAlertController(title: NSLocalizedString("Favorite Address", comment: "Favorite Update Dialog Title"), message: "", preferredStyle: .alert)
        dialog.setValue(vc, forKey: "contentViewController")
        dialog.addAction(UIAlertAction(title: "Done", style: .default){ action in
            vc.address?.title = vc.textTitle.text
            vc.address?.address = vc.textAddress.text
            vc.address?.location = vc.map.camera.centerCoordinate
            UpsertAddress(address: vc.address!).execute() { result in
                switch result {
                case .success(_):
                    self.refreshList(self)
                    
                case .failure(let error):
                    error.showAlert()
                }
            }
        })
        dialog.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: ""), style: .cancel, handler: nil))
        self.present(dialog, animated: true)
    }
    
    //MARK: Properties
    
    var addresses = [Address]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.refreshList(self)
    }
    
    @IBAction func onAddFavoriteClicked(_ sender: Any) {
        let vc = FavoriteAddressDialogViewController(nibName: "FavoriteAddressDialogViewController", bundle: nil)
        vc.preferredContentSize = CGSize(width: 1000,height: 400)
        let dialog = UIAlertController(title: NSLocalizedString("Favorite Address", comment: "Favorites Add Dialog Title"), message: "", preferredStyle: .alert)
        dialog.setValue(vc, forKey: "contentViewController")
        dialog.addAction(UIAlertAction(title: NSLocalizedString("Done", comment: ""), style: .default) { action in
            if let title = vc.textTitle.text, title.isEmpty {
                SPAlert.present(title: NSLocalizedString("Title is required", comment: ""), preset: .exclamation)
                return
            }
            let address = Address()
            address.title = vc.textTitle.text
            address.address = vc.textAddress.text
            address.location = vc.map.camera.centerCoordinate
            UpsertAddress(address: address).execute() { result in
                switch result {
                case .success(_):
                    self.refreshList(self)
                    
                case .failure(let error):
                    error.showAlert()
                }
            }
        })
        dialog.addAction(UIAlertAction(title: "Cancel", style: .cancel, handler: nil))
        self.present(dialog, animated: true)
        
        
    }
    @IBAction func refreshList(_ sender: Any) {
        GetAddresses().execute() { result in
            switch result {
            case .success(let response):
                self.addresses = response
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
        return addresses.count
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let kWhateverHeightYouWant = 80
        return CGSize(width: collectionView.bounds.size.width, height: CGFloat(kWhateverHeightYouWant))
    }
    
    override func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        // Table view cells are reused and should be dequeued using a cell identifier.
        let cellIdentifier = "AddressCell"
        
        guard let cell = self.collectionView?.dequeueReusableCell(withReuseIdentifier: cellIdentifier, for: indexPath) as? AddressCell  else {
            fatalError("The dequeued cell is not an instance of AddressCell.")
        }
        // Fetches the appropriate meal for the data source layout.
        let address = addresses[indexPath.row]
        cell.textTitle.text = address.title
        cell.textAddress.text = address.address
        cell.address = address
        cell.delegate = self
        //cell.background.colorId = indexPath.row

        return cell
    }
}
