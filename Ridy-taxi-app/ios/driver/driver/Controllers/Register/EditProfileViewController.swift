//
//  EditProfileViewController.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import Eureka
import ImageRow
import Kingfisher

class EditProfileViewController: FormViewController {
    lazy var driver = try! Driver(from: UserDefaultsConfig.user!)
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector:#selector(self.refreshScreeen), name: .connectedAfterForeground, object: nil)
        form
            +++ Section(NSLocalizedString("Status", comment: "Profile's status section header"))
            <<< LabelRow() {
                $0.title = "Status"
                $0.disabled = true
                $0.value = driver.status!.rawValue
            }
            <<< LabelRow() {
                $0.title = "Note"
                $0.disabled = true
                $0.value = driver.documentsNote
            }
            +++ Section(NSLocalizedString("Images", comment: "Profile's image section header"))
            <<< ImageRow() {
                $0.tag = "profile_row"
                $0.title = NSLocalizedString("Profile Image", comment: "Profile's image field title")
                $0.clearAction = .no
                if let address = driver.media?.address {
                    let url = URL(string: Config.Backend + address.replacingOccurrences(of: " ", with: "%20"))
                    ImageDownloader.default.downloadImage(with: url!, completionHandler:  { result in
                        switch result {
                        case .success(let value):
                            (self.form.rowBy(tag: "profile_row")! as! ImageRow).value = value.image
                            (self.form.rowBy(tag: "profile_row")! as! ImageRow).reload()
                        case .failure(let error):
                            print(error)
                        }
                    })
                }
            }
            .cellUpdate { cell, row in
                cell.accessoryView?.layer.cornerRadius = 17
                cell.accessoryView?.frame = CGRect(x: 0, y: 0, width: 34, height: 34)
            }.onChange { self.uploadMedia(type: "driver image", dataImage: ($0.value?.jpegData(compressionQuality: 1))!) { } }
            +++ Section(NSLocalizedString("Personal Info", comment: "Profile Personal Info section header"))
            <<< PhoneRow() {
                $0.title = NSLocalizedString("Mobile Number", comment: "Profile Mobile Number field title")
                $0.disabled = true
                $0.value = "\(driver.mobileNumber ?? 0)"
            }
            <<< EmailRow() {
                $0.title = NSLocalizedString("E-Mail", comment: "Profile Email field title")
                $0.value = driver.email
            }.onChange {
                    self.driver.email = $0.value
            }
            <<< TextRow() {
                $0.title = NSLocalizedString("Name", comment: "Profile Name field")
                $0.value = driver.firstName
                $0.placeholder = NSLocalizedString("First Name", comment: "Profile First Name Field")
            }.onChange {
                self.driver.firstName = $0.value
            }
            <<< TextRow() {
                $0.title = " "
                $0.placeholder = NSLocalizedString("Last Name", comment: "Profile Last Name field")
                $0.value = driver.lastName
            }.onChange {
                self.driver.lastName = $0.value
            }
            <<< PushRow<String>() {
                $0.title = NSLocalizedString("Gender", comment: "Profile's gender field title")
                $0.selectorTitle = NSLocalizedString("Select Your Gender", comment: "Profile's gender field selector title")
                $0.options = ["Male","Female","Unspecified"]
                $0.value = driver.gender?.capitalizingFirstLetter()
            }.onChange { self.driver.gender = ($0.value! as String).lowercased() }
            <<< TextRow() {
                $0.title = NSLocalizedString("Certificate Number", comment: "Profile Certificate Number field title")
                $0.value = driver.certificateNumber
            }.onChange() { self.driver.certificateNumber = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Car Color", comment: "Profile Car color field title")
                $0.value = driver.carColor
            }.onChange() { self.driver.carColor = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Car Plate", comment: "Profile Car plate field title")
                $0.value = driver.carPlate
            }.onChange() { self.driver.carPlate = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Bank Name", comment: "Profile Bank name")
                $0.value = driver.bankName
            }.onChange() { self.driver.bankName = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Bank Account Number or IBAN (if applicable)", comment: "Profile Bank account field title")
                $0.value = driver.accountNumber
            }.onChange() { self.driver.accountNumber = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Bank Routing Number", comment: "Profile Bank Routing Number field title")
                $0.value = driver.bankRoutingNumber
            }.onChange() { self.driver.bankRoutingNumber = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Bank SWIFT or BIC code (if applicable)", comment: "Profile Swift field title")
                $0.value = driver.bankSwift
            }.onChange() { self.driver.bankSwift = $0.value }
            <<< TextRow() {
                $0.title = NSLocalizedString("Your Address", comment: "Profile Address field title")
                $0.value = driver.address
            }.onChange { self.driver.address = $0.value }
            
            +++ Section(header: NSLocalizedString("Services", comment: "Profile's Services Info section"), footer: NSLocalizedString("You can select services you can provide. Provider might change those and select other services for you accordingly.", comment: "Profile's Services Footer"))
            <<< MultipleSelectorRow<Service>() {
                $0.title = "Services"
                $0.options = try! [Service](from: UserDefaultsConfig.services!)
            }.onChange { self.driver.services = Array($0.value!) }
            +++ Section(header: NSLocalizedString("Documents", comment: "Profile's Documents Info section"), footer: NSLocalizedString("After approval all your documents will be removed.", comment: "Profile's Documents Footer"))
            <<< ImageRow() {
                $0.title = "ID"
            }.onChange { self.uploadMedia(type: "document", dataImage: ($0.value?.jpegData(compressionQuality: 1))!) { } }
            <<< ImageRow() {
                $0.title = "Driver License"
            }.onChange { self.uploadMedia(type: "document", dataImage: ($0.value?.jpegData(compressionQuality: 1))!) { } }
            <<< ImageRow() {
                $0.title = "Picture of Vehicle"
            }.onChange { self.uploadMedia(type: "document", dataImage: ($0.value?.jpegData(compressionQuality: 1))!) { } }
    }
    
    @IBAction func onSaveButtonClicked(_ sender: UIBarButtonItem) {
        Register(jwtToken: UserDefaultsConfig.jwtToken!, driver: driver).execute() { result in
            switch result {
            case .success(_):
                let title = NSLocalizedString("Message", comment: "")
                let message = NSLocalizedString("Registration was done successfully. You can exit the app now and check back later to see your approval status.", comment: "Registration successful")
                let dialog = UIAlertController(title: title, message: message, preferredStyle: .alert)
                dialog.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: "Message OK button"), style: .default))
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    func uploadMedia(type: String, dataImage: Data, completionHandler: @escaping (() -> Void)) {
        let url = URL(string: "\(Config.Backend)driver/upload")!
        var request = URLRequest(url: url)
        request.setValue(driver.mobileNumber!.description, forHTTPHeaderField: "number")
        request.setValue(type, forHTTPHeaderField: "type")
        request.httpMethod = "POST"
        let boundary:String = "Boundary-\(UUID().uuidString)"
        request.timeoutInterval = 60
        request.allHTTPHeaderFields = ["Content-Type": "multipart/form-data; boundary=----\(boundary)"]
        var data: Data = Data()
        data.append("------\(boundary)\r\n")
        //Here you have to change the Content-Type
        data.append("Content-Disposition: form-data; name=\"file\"; filename=\"d.png\"\r\n")
        data.append("Content-Type: application/YourType\r\n\r\n")
        data.append(dataImage)
        data.append("\r\n")
        data.append("------\(boundary)--")
        
        request.httpBody = data
        DispatchQueue.global(qos: DispatchQoS.QoSClass.userInitiated).sync {
            URLSession.shared.dataTask(with: request, completionHandler: { data, response, error in
                DispatchQueue.main.async {
                    completionHandler()
                }
            }).resume()
        }
    }
    
    @objc func refreshScreeen() {
        GetRegisterInfo(jwtToken: UserDefaultsConfig.jwtToken!).execute() { result in
            switch result {
            case .success(let response):
                UserDefaultsConfig.user = try! response.driver.asDictionary()
                self.dismiss(animated: true, completion: nil)
                
            case .failure(let error):
                print(error)
            }
        }
    }
    
}
extension Data{
    mutating func append(_ string: String, using encoding: String.Encoding = .utf8) {
        if let data = string.data(using: encoding) {
            append(data)
        }
    }
}
