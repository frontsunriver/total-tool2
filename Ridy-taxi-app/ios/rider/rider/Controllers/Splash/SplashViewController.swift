//
//  SplashViewController.swift
//  Rider
//
//  Copyright © 2018 Minimalistic Apps. All rights reserved.
//

import UIKit
import Firebase
import SPAlert
import FirebaseMessaging
import FirebaseUI

class SplashViewController: UIViewController {
    let defaults:UserDefaults = UserDefaults.standard
    @IBOutlet weak var indicatorLoading: UIActivityIndicatorView!
    @IBOutlet weak var textLoading: UILabel!
    @IBOutlet weak var buttonLogin: UIButton!
    override func viewDidLoad() {
        
    }
    func connectSocket(token:String) {
        Messaging.messaging().token() { (fcmToken, error) in
            if let error = error {
                print("Error fetching remote instance ID: \(error)")
                return
            }
            SocketNetworkDispatcher.instance.connect(namespace: .Rider, token: token, notificationId: fcmToken ?? "") { result in
                switch result {
                case .success(_):
                    self.performSegue(withIdentifier: "segueShowHost", sender: nil)
                    
                case .failure(let error):
                    switch error {
                    case .NotFound:
                        let title = NSLocalizedString("Message", comment: "Message Default Title")
                        let dialog = UIAlertController(title: title, message: "User Info not found. Do you want to register again?", preferredStyle: .alert)
                        dialog.addAction(UIAlertAction(title: "Register", style: .default) { action in
                            self.onLoginClicked(self.buttonLogin)
                        })
                        dialog.addAction(UIAlertAction(title: "Cancel", style: .cancel, handler: nil))
                        self.present(dialog, animated: true, completion: nil)
                        
                    default:
                        SPAlert.present(title: "Error", message: error.rawValue, preset: .error)
                        self.indicatorLoading.isHidden = true
                        self.textLoading.isHidden = true
                        self.buttonLogin.isHidden = false
                    }
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
    
    func tryLogin(firebaseToken: String) {
        Login(firebaseToken: firebaseToken).execute() { result in
            switch result {
            case .success(let response):
                UserDefaultsConfig.jwtToken = response.token
                UserDefaultsConfig.user = try! response.user.asDictionary()
                self.connectSocket(token: response.token)
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    override func viewDidAppear(_ animated: Bool) {
        if let token = UserDefaultsConfig.jwtToken {
            connectSocket(token: token)
        } else {
            indicatorLoading.isHidden = true
            textLoading.isHidden = true
            buttonLogin.isHidden = false
        }
    }
}
extension SplashViewController: FUIAuthDelegate {
    func authUI(_ authUI: FUIAuth, didSignInWith user: User?, error: Error?) {
        if(user == nil) {
            return
        }
        indicatorLoading.isHidden = false
        textLoading.isHidden = false
        buttonLogin.isHidden = true
        user?.getIDTokenForcingRefresh(true) { idToken, error in
            if let error = error {
                print(error)
                return;
            }
            
            self.tryLogin(firebaseToken: idToken!)
        }
    }
}
