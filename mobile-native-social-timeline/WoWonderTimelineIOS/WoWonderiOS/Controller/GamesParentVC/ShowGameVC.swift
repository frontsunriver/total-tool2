//
//  ShowGameVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/30/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import WebKit
class ShowGameVC: UIViewController {
    
    var gameLink:String? = ""
    
    @IBOutlet weak var webView: WKWebView!
    override func viewDidLoad() {
        super.viewDidLoad()
        let url = URL(string: gameLink!);
        let request = URLRequest(url: url!)
        webView.load(request);

    }
    

}
