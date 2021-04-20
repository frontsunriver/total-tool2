//
//  AuthIntroController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 8/5/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit

class AuthIntroController: UIViewController {
    
    
    @IBOutlet weak var startLabel: UILabel!
    @IBOutlet weak var loginBtn: RoundButton!
    @IBOutlet weak var registerBtn: RoundButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.startLabel.text = NSLocalizedString("Let's get started!", comment: "Let's get started!")
        self.loginBtn.setTitle(NSLocalizedString("Login", comment: "Login"), for: .normal)
        self.registerBtn.setTitle(NSLocalizedString("Register", comment: "Register"), for: .normal)

    }
    



}
