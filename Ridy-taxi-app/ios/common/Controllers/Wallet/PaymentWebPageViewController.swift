//
//  PaymentWebPageViewController.swift
//  rider
//
//  Created by Manly Man on 9/10/20.
//  Copyright © 2020 minimal. All rights reserved.
//

import UIKit
import WebKit

public class PaymentWebPageViewController: UIViewController, WKNavigationDelegate {
    private let webView = WKWebView(frame: .zero)
    var url: String = ""
    weak var delegate: WebPaymentResultDelegate?
    
    override public func viewDidLoad() {
        super.viewDidLoad()
        webView.translatesAutoresizingMaskIntoConstraints = false
        self.view.addSubview(self.webView)
        // You can set constant space for Left, Right, Top and Bottom Anchors
        NSLayoutConstraint.activate([
            self.webView.leftAnchor.constraint(equalTo: self.view.leftAnchor),
            self.webView.bottomAnchor.constraint(equalTo: self.view.bottomAnchor),
            self.webView.rightAnchor.constraint(equalTo: self.view.rightAnchor),
            self.webView.topAnchor.constraint(equalTo: self.view.topAnchor),
        ])
        // For constant height use the below constraint and set your height constant and remove either top or bottom constraint
        //self.webView.heightAnchor.constraint(equalToConstant: 200.0),
        
        self.view.setNeedsLayout()
        self.webView.navigationDelegate = self
        let request = URLRequest(url: URL.init(string: url)!)
        self.webView.load(request)
    }
    
    public func webView(_ webView: WKWebView, decidePolicyFor
           navigationAction: WKNavigationAction,
           decisionHandler: @escaping (WKNavigationActionPolicy) -> Swift.Void) {

        let url = navigationAction.request.url!.absoluteString
        if isUrlVerify(url: url) || isUrlCancel(url: url) {
            if let del = delegate {
                if isUrlVerify(url: url) {
                    del.paid()
                } else {
                    del.canceled()
                }
            }
            decisionHandler(.cancel)
            dismiss(animated: true, completion: nil)
            return
        }
        decisionHandler(.allow)
    }
    
    func isUrlVerify(url: String) -> Bool {
        return url.contains("ridyverifiedpayment")
    }
    
    func isUrlCancel(url: String) -> Bool {
        return url.contains("ridycancelpayment")
        
    }
}

protocol WebPaymentResultDelegate: class {
    func paid()
    func canceled()

}
