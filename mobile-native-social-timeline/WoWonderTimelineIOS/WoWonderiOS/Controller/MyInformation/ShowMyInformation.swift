//
//  ShowMyInformation.swift
//  WoWonderiOS
//
//  Created by Ubaid Javaid on 10/27/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
import WebKit
import ZKProgressHUD


class ShowMyInformation: UIViewController, WKUIDelegate,WKNavigationDelegate {

    @IBOutlet weak var webView: WKWebView!
    
    let status = Reach().connectionStatus()
    var navTitle = ""
    var urlString : String = ""
    
    override func viewDidLoad() {
        super.viewDidLoad()

        NotificationCenter.default.addObserver(self, selector: #selector(self.networkStatusChanged(_:)), name: Notification.Name(rawValue: ReachabilityStatusChangedNotification), object: nil)
        navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
            let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
             navigationController?.navigationBar.titleTextAttributes = textAttributes
                 self.navigationItem.title = navTitle
        self.webView.uiDelegate = self
        self.webView.navigationDelegate = self


    }
    
    override func viewWillAppear(_ animated: Bool) {
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))
        case .online(.wwan),.online(.wiFi):
            ZKProgressHUD.show()
        
            let url = NSURL (string: self.urlString )
//            print("webURL::::::::::\(url!) \n webTitle :::::::: \(webTitle)")
            let requestObj = NSURLRequest(url: url! as URL)
            print("request")

            self.webView.load(requestObj as URLRequest)
            print("webview")
            
        }

    }
    
    ///Network Connectivity.
    @objc func networkStatusChanged(_ notification: Notification) {
        if let userInfo = notification.userInfo {
            let status = userInfo["Status"] as! String
            print(status)
        }
    }
    
    func webView(_ webView: WKWebView, didFinish navigation: WKNavigation!) {
        ZKProgressHUD.dismiss()
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
