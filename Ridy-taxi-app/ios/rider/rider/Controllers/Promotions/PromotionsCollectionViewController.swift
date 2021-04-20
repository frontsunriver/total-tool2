//
//  PromotionsCollectionViewController.swift
//  rider
//
//  Created by Manly Man on 11/23/18.
//  Copyright © 2018 minimal. All rights reserved.
//

import UIKit

class PromotionsCollectionViewController: UICollectionViewController, UICollectionViewDelegateFlowLayout {
    let cellIdentifier = "PromotionsCollectionViewCell"
    var promotions = [Promotion]()
    var colors = [[String]]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        let nibCell = UINib(nibName: cellIdentifier, bundle: nil)
        collectionView?.register(nibCell, forCellWithReuseIdentifier: cellIdentifier)
        self.refreshList(self)
    }
    
    @IBAction func refreshList(_ sender: Any) {
        GetPromotions().execute() { result in
            switch result {
            case .success(let response):
                self.promotions = response
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
        return promotions.count
    }
    
    override func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        guard let cell = self.collectionView?.dequeueReusableCell(withReuseIdentifier: cellIdentifier, for: indexPath) as? PromotionsCollectionViewCell  else {
            fatalError("The dequeued cell is not an instance of PromotionsCollectionViewCell.")
        }
        let promotion = promotions[indexPath.row]
        cell.promotion = promotion
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let kWhateverHeightYouWant = 60
        return CGSize(width: collectionView.bounds.size.width, height: CGFloat(kWhateverHeightYouWant))
    }
}
