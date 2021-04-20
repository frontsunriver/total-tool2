//
//  CreateBlogController.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/21/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import WebKit

class CreateBlogController: UIViewController {
    
    
    @IBOutlet weak var webView: WKWebView!
    
    let status = Reach().connectionStatus()

    override func viewDidLoad() {
        super.viewDidLoad()
    NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
    navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
         navigationController?.navigationBar.titleTextAttributes = textAttributes
             self.navigationItem.title = NSLocalizedString("Create Article", comment: "Create Article")
    }
    
    override func viewWillAppear(_ animated: Bool) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            let url = "\("demo.wowonder.com")\("/create-blog?c_id=")\(UserData.getAccess_Token() ?? "")\("&user_id=")\(UserData.getUSER_ID() ?? "")\("&application=phone")"
            self.webView.load(URLRequest(url: URL(string: url)!))
        }

    }
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
        }
    }
    

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destination.
        // Pass the selected object to the new view controller.
    }
    */

}
