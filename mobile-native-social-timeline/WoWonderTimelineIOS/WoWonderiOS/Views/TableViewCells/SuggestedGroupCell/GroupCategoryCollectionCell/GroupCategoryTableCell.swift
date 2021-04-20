//
//  GroupCategoryTableCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/8/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class GroupCategoryTableCell: UITableViewCell,UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout {

    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var cateLbl: UILabel!
    @IBOutlet weak var txtLbl: UILabel!
    
    var categorylist = [String:String]()
    var vc: GroupsDiscoverController?
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
        self.collectionView.register(UINib(nibName: "GroupCategoryCollectionCell", bundle: nil), forCellWithReuseIdentifier: "GroupCateCollectionCell")
        self.cateLbl.text = NSLocalizedString("Categories", comment: "Categories")
        self.txtLbl.text = NSLocalizedString("Find a group by browsing top groups", comment: "Find a group by browsing top groups")
    }
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return self.categorylist.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "GroupCateCollectionCell", for: indexPath) as! GroupCategoryCollectionCell
        let values = Array(self.categorylist.values)[indexPath.row]
        cell.cateName.text = values
        cell.catImage.image = UIImage(named: "CategoryImage")
//            .withRenderingMode(.alwaysTemplate)
//        cell.catImage.tintColor = .green
        
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let keys = Array(self.categorylist.keys)[indexPath.row]
        let values = Array(self.categorylist.values)[indexPath.row]
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "GroupCategoryVC") as! GroupByCategoryController
        print(keys)
        vc.cateId = Int(keys) ?? 0
        vc.navTitle = values
        self.vc?.navigationController?.pushViewController(vc, animated: true)
        
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: 180.0, height: 160.0)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 0.0
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 0.0
    }
    

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}
