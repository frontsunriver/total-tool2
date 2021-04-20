//
//  SplashViewController.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import SPAlert
import Firebase
import FirebaseUI
import FirebaseMessaging

class SplashViewController: UIViewController {
    @IBOutlet weak var indicatorLoading: UIActivityIndicatorView!
    @IBOutlet weak var textLoading: UILabel!
    @IBOutlet weak var buttonLogin: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.showMainPage), name: .connectedAfterForeground, object: self)
        NotificationCenter.default.addObserver(self, selector: #selector(self.onConError), name: .connectionError, object: self)
        if let token = UserDefaultsConfig.jwtToken {
            connectSocket(token: token)
        } else {
            self.loginState()
        }
    }
    
    func loginState() {
        indicatorLoading.isHidden = true
        textLoading.isHidden = true
        buttonLogin.isHidden = false
        buttonLogin.isEnabled = true
    }
    
    func loadingState() {
        indicatorLoading.isHidden = false
        textLoading.isHidden = false
        buttonLogin.isHidden = true
        buttonLogin.isEnabled = false
    }
    
    @objc func onConError(_ notification: Notification) {
        let err = notification.object as! ConnectionError
        connectionError(error: err)
    }
    
    func connectionError(error: ConnectionError) {
        switch error {
        case .NotFound:
            let title = NSLocalizedString("Message", comment: "Message Default Title")
            let dialog = UIAlertController(title: title, message: NSLocalizedString("User Info not found. Do you want to register again?", comment: ""), preferredStyle: .alert)
            dialog.addAction(UIAlertAction(title: NSLocalizedString("Register", comment: ""), style: .default) { action in
                self.onLoginClicked(self.buttonLogin)
            })
            dialog.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: ""), style: .cancel, handler: nil))
            self.present(dialog, animated: true, completion: nil)
            
        case .RegistrationIncomplete:
            self.showRegisterForm(jwtToken: UserDefaultsConfig.jwtToken!)
            
        default:
            SPAlert.present(title: NSLocalizedString("Error", comment: ""), message: error.rawValue, preset: .error)
            self.loginState()
        }
    }
    
    func connectSocket(token:String) {
        Messaging.messaging().token() { fcmId, error in
            SocketNetworkDispatcher.instance.connect(namespace: .Driver, token: token, notificationId: fcmId ?? "") { result in
                switch result {
                case .success(_):
                    self.performSegue(withIdentifier: "segueShowHost", sender: nil)
                    
                case .failure(let error):
                    self.loginState()
                    self.connectionError(error: error)
                    
                }
            }
        }
    }
    
    @IBAction func onLoginClicked(_ sender: UIButton) {
        let auth = FUIAuth.defaultAuthUI()
        auth?.delegate = self
        let phoneAuth = FUIPhoneAuth(authUI: auth!)
        auth?.providers = [phoneAuth]
        phoneAuth.signIn(withPresenting: self, phoneNumber: nil)
    }
    
    @objc func showMainPage() {
        NotificationCenter.default.removeObserver(self)
        self.performSegue(withIdentifier: "segueShowHost", sender: nil)
    }
    
    func tryLogin(firebaseToken: String) {
        self.loadingState()
        Login(firebaseToken: firebaseToken).execute() { result in
            switch result {
            case .success(let response):
                UserDefaultsConfig.jwtToken = response.token
                UserDefaultsConfig.user = try! response.user.asDictionary()
                if(response.user.status == Driver.Status.Offline || response.user.status == Driver.Status.Online) {
                    self.connectSocket(token: response.token)
                } else {
                    self.showRegisterForm(jwtToken: response.token)
                }
                break
                
            case .failure(let error):
                self.loginState()
                error.showAlert()
            }
        }
    }
    
    func showRegisterForm(jwtToken: String) {
        GetRegisterInfo(jwtToken: jwtToken).execute() { result in
            switch result {
            case .success(let response):
                do {
                    UserDefaultsConfig.user = try response.driver.asDictionary()
                    UserDefaultsConfig.services = try response.services.map() { return try $0.asDictionary() }
                    self.buttonLogin.isHidden = true
                    self.textLoading.isHidden = true
                    self.indicatorLoading.isHidden = true
                    self.performSegue(withIdentifier: "showRegister", sender: nil)
                } catch {
                    SPAlert.present(title: NSLocalizedString("Error", comment: ""), message: NSLocalizedString("Can't decode", comment: ""), preset: .error)
                }
                
            case .failure(let error):
                error.showAlert()
                self.loginState()
            }
        }
    }
}

extension SplashViewController: FUIAuthDelegate {
    func authUI(_ authUI: FUIAuth, didSignInWith user: User?, error: Error?) {
        if user == nil {
            return
        }
        user?.getIDTokenForcingRefresh(true) { idToken, error in
            if let error = error {
                print(error)
                return;
            }
            
            self.tryLogin(firebaseToken: idToken!)
        }
    }
}
