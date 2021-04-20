//
//  ApplyJobVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/22/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import ZKProgressHUD
class ApplyJobVC: UIViewController {
    
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var checkBoxImage: UIImageView!
    @IBOutlet weak var currentlyWorkView: UIView!
    @IBOutlet weak var toDateBtn: UIButton!
    @IBOutlet weak var fromBtn: UIButton!
    @IBOutlet weak var emailTextField: RoundTextField!
    @IBOutlet weak var locationTextField: UITextView!
    @IBOutlet weak var phoneTextField: RoundTextField!
    @IBOutlet weak var DescriptionTextView: UITextView!
    @IBOutlet weak var WhereWorkTextFIeld: RoundTextField!
    @IBOutlet weak var positionBtn: UIButton!
    @IBOutlet weak var nameLabel: RoundTextField!
    
    
    var status:Bool? = false
    var position:String? = ""
    var startYear:String? = ""
    var endYear:String? = ""
    var currentlywork:String? = "off"
        var jobId:String? = ""
    var titleString:String? = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.titleLabel.text = self.titleString ?? ""
        self.nameLabel.text = AppInstance.instance.profile?.userData?.name ?? ""
        self.emailTextField.text = AppInstance.instance.profile?.userData?.email ?? ""
        let tap = UITapGestureRecognizer(target: self, action: #selector(self.handleTap(_:)))
        currentlyWorkView.addGestureRecognizer(tap)
    }
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.navigationController?.isNavigationBarHidden = true
    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        self.navigationController?.isNavigationBarHidden = false
    }
    @objc func handleTap(_ sender: UITapGestureRecognizer? = nil) {
        self.status = !self.status!
        if self.status ?? false{
            self.currentlywork = "on"
            self.checkBoxImage.image = UIImage(named: "check-sign-in-a-square")
            self.toDateBtn.isHidden = true
            self.endYear = ""
        }else{
            self.currentlywork = "off"
            self.checkBoxImage.image = UIImage(named: "check-box-empty")
            self.toDateBtn.isHidden = false

            
        }
        
    }
    @IBAction func Back(_ sender: Any) {
        self.navigationController?.popViewController(animated: true)
    }
    @IBAction func sendPressed(_ sender: Any) {
        if self.nameLabel.text!.isEmpty || self.phoneTextField.text!.isEmpty || self.locationTextField.text!.isEmpty || self.emailTextField.text!.isEmpty || self.position == "" || self.WhereWorkTextFIeld.text!.isEmpty || self.DescriptionTextView.text!.isEmpty || self.startYear == "" || self.endYear == ""  {
            self.view.makeToast("Please enter your data")
        }else{
            self.applyJob()
        }
    }
    
    @IBAction func positionPressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectDateVC") as! SelectDateVC
        vc.delegate = self
        vc.type = 2
        self.present(vc, animated: true, completion: nil)
    }
    
    @IBAction func toDatePressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectDateVC") as! SelectDateVC
        vc.delegate = self
        vc.type = 1
        self.present(vc, animated: true, completion: nil)
    }
    
    
    @IBAction func fromDatePressed(_ sender: Any) {
        let storyboard = UIStoryboard(name: "MoreSection", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectDateVC") as! SelectDateVC
        vc.delegate = self
        vc.type = 0
        self.present(vc, animated: true, completion: nil)
    }
    
    private func applyJob(){
        ZKProgressHUD.show()
        let name = self.nameLabel.text ?? ""
        let phonenumber = self.phoneTextField.text ?? ""
        let location = self.locationTextField.text ?? ""
        let email = self.emailTextField.text ?? ""
        let work = self.WhereWorkTextFIeld.text ?? ""
        let expDes = self.DescriptionTextView.text ?? ""
        let position = self.position ?? ""
        let startYear = self.startYear ?? ""
        let endYear = self.endYear ?? ""
        let id = self.jobId ?? ""
        
        performUIUpdatesOnMain {
            JobsManager.sharedInstance.ApplyJob(jobID:id,type:"apply",Name: name, Phone: phonenumber,location:location, email: email, position: position, work: work, description: expDes, fromDate: startYear, ToDateString: endYear, currentlywork: self.currentlywork ?? "") { (success, sessionError, error) in
                if success != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(success?.messageData ?? "")
                    
                    self.navigationController?.popViewController(animated: true)
                }
                else if sessionError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(sessionError?.errors?.errorText)
                }
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
        }
        
    }
    
}


extension ApplyJobVC:SelectYearDelegate{
    func selectYear(year: String, index: Int, type: Int) {
        if type == 0{
            self.fromBtn.setTitle(year, for: .normal)
            self.startYear = year
        } else if type == 1{
            self.toDateBtn.setTitle(year, for: .normal)
            self.endYear = year
        }else if type == 2{
            self.position = year
            self.positionBtn.setTitle(year, for: .normal)
            
        }
    }
    
    
}
