//
//  FilterFriendsVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/14/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class FilterFriendsVC: UIViewController {
    
    @IBOutlet weak var all: RoundButton!
    @IBOutlet weak var female: RoundButton!
    @IBOutlet weak var male: RoundButton!
    
    @IBOutlet weak var sliderValue: UILabel!
    @IBOutlet weak var silder: UISlider!
    @IBOutlet weak var statusAll: RoundButton!
    @IBOutlet weak var statusOffline: RoundButton!
    @IBOutlet weak var statusOnline: RoundButton!

    @IBOutlet weak var relationShipLabel: UILabel!
    
    var gender: String? = nil
    var status: String? = nil
    var relationship:String? = nil
    var distance:Int? = 0
    var delegate: filterFriendsDelegate?

    override func viewDidLoad() {
        super.viewDidLoad()

    }
    
    @IBAction func sliderValueChanged(_ sender: Any) {
        sliderValue.text = "\(silder.value.toInt() ?? 0)km"
        self.distance = silder.value.toInt() ?? 0
        
        
    }
    
    @IBAction func resetFilterPressed(_ sender: Any) {
        self.all.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
             self.all.setTitleColor(.white, for: .normal)
             self.male.backgroundColor = .white
             self.female.backgroundColor = .white
             self.female.setTitleColor(.black, for: .normal)
             self.male.setTitleColor(.black, for: .normal)
             self.gender = "all"
             
             self.statusAll.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                         self.statusAll.setTitleColor(.white, for: .normal)
                         self.statusOnline.backgroundColor = .white
                         self.statusOffline.backgroundColor = .white
                         self.statusOffline.setTitleColor(.black, for: .normal)
                         self.statusOnline.setTitleColor(.black, for: .normal)
                         self.status = "all"
             
             self.silder.setValue(0.0, animated: true)
             self.relationShipLabel.text = "All"
             self.distance = 0
        self.gender = "All"
        self.status = "All"
    }
    @IBAction func relationShipPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectDateVC") as! SelectDateVC
        vc.delegate = self
        vc.type = 4
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func filterPressed(_ sender: Any) {
        self.dismiss(animated: true) {
            self.delegate?.filterFriends(gender: self.gender ?? "", distenace: self.distance ?? 0, status: self.status ?? "" , relationship: self.relationship ?? "")
        }
        
        
    }
    @IBAction func backPressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
       }
       
    @IBAction func Gender(_ sender: UIButton) {
           if (sender.tag == 0){
               self.all.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
               self.all.setTitleColor(.white, for: .normal)
               self.male.backgroundColor = .white
               self.female.backgroundColor = .white
               self.female.setTitleColor(.black, for: .normal)
               self.male.setTitleColor(.black, for: .normal)
               self.gender = "all"
           }
           else if (sender.tag == 1){
               self.all.backgroundColor = .white
               self.male.backgroundColor = .white
               self.female.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
               self.female.setTitleColor(.white, for: .normal)
               self.all.setTitleColor(.black, for: .normal)
               self.male.setTitleColor(.black, for: .normal)
               self.gender = "female"
           }
           else  {
               self.all.backgroundColor = .white
               self.female.backgroundColor = .white
               self.male.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
               self.male.setTitleColor(.white, for: .normal)
               self.all.setTitleColor(.black, for: .normal)
               self.female.setTitleColor(.black, for: .normal)
               self.gender = "male"
           }
       }
    @IBAction func status(_ sender: UIButton) {
             if (sender.tag == 0){
                 self.statusAll.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                 self.statusAll.setTitleColor(.white, for: .normal)
                 self.statusOnline.backgroundColor = .white
                 self.statusOffline.backgroundColor = .white
                 self.statusOffline.setTitleColor(.black, for: .normal)
                 self.statusOnline.setTitleColor(.black, for: .normal)
                 self.status = "all"
             }
             else if (sender.tag == 1){
                 self.statusAll.backgroundColor = .white
                 self.statusOnline.backgroundColor = .white
                 self.statusOffline.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                 self.statusOffline.setTitleColor(.white, for: .normal)
                 self.statusAll.setTitleColor(.black, for: .normal)
                 self.statusOnline.setTitleColor(.black, for: .normal)
                 self.status = "Offline"
             }
             else  {
                 self.statusAll.backgroundColor = .white
                 self.statusOffline.backgroundColor = .white
                 self.statusOnline.backgroundColor = UIColor.hexStringToUIColor(hex: "984243")
                 self.statusOnline.setTitleColor(.white, for: .normal)
                 self.statusAll.setTitleColor(.black, for: .normal)
                 self.statusOffline.setTitleColor(.black, for: .normal)
                 self.status = "Online"
             }
         }
}
extension FilterFriendsVC:SelectYearDelegate{
    func selectYear(year: String, index: Int, type: Int) {
        self.relationShipLabel.text = year
        self.relationship = year
    }
    
    
}
