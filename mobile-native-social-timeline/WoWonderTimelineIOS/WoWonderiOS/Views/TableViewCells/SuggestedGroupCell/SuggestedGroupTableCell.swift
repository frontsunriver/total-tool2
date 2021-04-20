//
//  SuggestedGroupTableCell.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/8/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class SuggestedGroupTableCell: UITableViewCell,UICollectionViewDelegate,UICollectionViewDataSource,UICollectionViewDelegateFlowLayout,JoinGroupDelegate,DeleteGroupDelegate {

    
    @IBOutlet weak var collectionView: UICollectionView!
    @IBOutlet weak var sellLbl: UILabel!
    
    var suggestedGroups = [[String:Any]]()
    var selectedIndex = 0
    
    var vc: GroupsDiscoverController?
    
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.collectionView.delegate = self
        self.collectionView.dataSource = self
        self.collectionView.register(UINib(nibName: "SuggestedGroupCollectionCell", bundle: nil), forCellWithReuseIdentifier: "suggestedCollectionCell")
        self.sellLbl.text = NSLocalizedString("See All", comment: "See All")
    }
    
    
    private func JoinGroup(groupId: String){
        switch self.vc!.status {
        case .unknown, .offline:
            self.vc!.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            JoinGroupManager.sharedInstance.joinGroup(groupId: Int(groupId) ?? 0) { (success, authError, error) in
                if success != nil {
                    self.vc!.view.makeToast(success?.join_status)
                    
                }
                else if authError != nil {
                    self.vc!.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    self.vc!.view.makeToast(error?.localizedDescription)
                }
            }
            
        }
    }
    
    @IBAction func SeeAll(_ sender: Any) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "ShowAllSuggestedGroupVC") as! ShowAllSuggestedGroups
        self.vc?.navigationController?.pushViewController(vc, animated: true)
    }
    
    @IBAction func Groupjoin(sender: UIButton){
        switch self.vc!.status {
        case .unknown, .offline:
            self.vc!.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let cell = self.collectionView.cellForItem(at: IndexPath(row: sender.tag, section: 0)) as! SuggestedGroupCollectionCell
            let index = self.suggestedGroups[sender.tag]
            var group_id: String? = nil
            if let groupid = index["id"] as? String{
                group_id = groupid
            }
            if let isLike = index["is_joined"] as? Bool{
                if isLike == true{
                    cell.joinBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
                    cell.joinBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                    cell.joinBtn.setTitleColor(.white, for: .normal)
                    self.JoinGroup(groupId: group_id ?? "")
                    self.suggestedGroups[sender.tag]["is_joined"] = false
                }
                else{
                    cell.joinBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
                    cell.joinBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                    cell.joinBtn.setTitleColor(.black, for: .normal)
                    self.JoinGroup(groupId: group_id ?? "")
                    self.suggestedGroups[sender.tag]["is_joined"] = true
                }
            }
            self.collectionView.reloadData()
        }
    }
    
    
    func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        self.suggestedGroups.count
    }
    
    func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        let cell = collectionView.dequeueReusableCell(withReuseIdentifier: "suggestedCollectionCell", for: indexPath) as! SuggestedGroupCollectionCell
        let index = self.suggestedGroups[indexPath.row]
        cell.joinBtn.tag = indexPath.row
        cell.joinBtn.addTarget(self, action: #selector(self.Groupjoin(sender:)), for: .touchUpInside)
        if let name = index["group_name"] as? String{
            cell.groupName.text = name
        }
        if let members = index["members"] as? String{
            cell.members.text = members
        }
        if let isOwner = index["is_owner"] as? Bool{
            if (isOwner == true){
                cell.joinBtn.isHidden = true
            }
            else{
                cell.joinBtn.isHidden = false
            }
        }
        if let isLike = index["is_joined"] as? Bool{
            if isLike == false{
                cell.joinBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
                cell.joinBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#984243")
                cell.joinBtn.setTitleColor(.white, for: .normal)
            }
            else{
                cell.joinBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
                cell.joinBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
                cell.joinBtn.setTitleColor(.black, for: .normal)
            }
        }
        return cell
    }
    
    func collectionView(_ collectionView: UICollectionView, didSelectItemAt indexPath: IndexPath) {
        let storyboard = UIStoryboard(name: "GroupsAndPages", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "GroupVC") as! GroupController
        let index = self.suggestedGroups[indexPath.row]
        self.selectedIndex = indexPath.row
        if let groupId = index["group_id"] as? String{
            vc.groupId = groupId
        }
        if let groupName = index["group_name"] as? String{
            vc.groupName = groupName
        }
        if let groupTitle = index["group_title"] as? String{
            vc.groupTitle = groupTitle
        }
        if let groupIcon = index["avatar"] as? String{
            vc.groupIcon = groupIcon
        }
        if let groupCover = index["cover"] as? String{
            vc.groupCover = groupCover
        }
        if let groupcategory = index["category"] as? String{
            vc.category = groupcategory
        }
        if let privacy = index["privacy"] as? String{
            vc.privacy = privacy
        }
        if let categoryId = index["category_id"] as? String{
            print(categoryId)
            vc.categoryId = categoryId
        }
        
        if let about  = index["about"] as? String{
            print(about)
            vc.aboutGroup = about
        }
        if let isJoined = index["is_joined"] as? Bool{
             vc.isJoined = isJoined
        }
        vc.delegte1 = self
        vc.delegate = self
        vc.groupData = index
        self.vc?.navigationController?.pushViewController(vc, animated: true)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        return CGSize(width: 190.0, height: 245.0)
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumLineSpacingForSectionAt section: Int) -> CGFloat {
        return 0.0
    }
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, minimumInteritemSpacingForSectionAt section: Int) -> CGFloat {
        return 0.0
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
        
        // Configure the view for the selected state
    }
    
    func joinGroup(isJoin: Bool) {
        let cell = self.collectionView.cellForItem(at: IndexPath(row: self.selectedIndex, section: 0)) as! SuggestedGroupCollectionCell
          if isJoin == false{
            cell.joinBtn.setTitle(NSLocalizedString("Join Group", comment: "Join Group"), for: .normal)
              cell.joinBtn.backgroundColor =  UIColor.hexStringToUIColor(hex: "#984243")
              //                    UIColor.hexStringToUIColor(hex: "#984243")
            cell.joinBtn.setTitleColor(.white, for: .normal)
            self.suggestedGroups[self.selectedIndex]["is_joined"] = false
              
          }
          else{
              cell.joinBtn.setTitle(NSLocalizedString("Joined", comment: "Joined"), for: .normal)
              cell.joinBtn.backgroundColor = UIColor.hexStringToUIColor(hex: "#e5e5e5")
            cell.joinBtn.setTitleColor(.black, for: .normal)
              self.suggestedGroups[self.selectedIndex]["is_joined"] = true
              
          }
    }
    
    func deleteGroup(groupId: String) {
        print("Nothing")
    }
    
    
}
