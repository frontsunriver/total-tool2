

import UIKit
import WebKit
import ZKProgressHUD
import WoWonderTimelineSDK
class HelpWebViewVC: UIViewController {

    @IBOutlet weak var webView: WKWebView!
    var URLString:String? = ""
    var Title:String = ""
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.largeTitleDisplayMode = .never
        self.title = Title ?? ""
        self.loadURL(URLString ?? "")
    }
    
    private func loadURL(_ Url:String){
        
        let link = URL(string:Url)!
        let request = URLRequest(url: link)
        webView.navigationDelegate = self
        webView.load(request)
    }

}
extension HelpWebViewVC: WKNavigationDelegate{
    func webView(_ webView: WKWebView, didStartProvisionalNavigation navigation: WKNavigation!) {
        ZKProgressHUD.show()
    }
    func webView(_ webView: WKWebView, didFinish navigation: WKNavigation!) {
        
        ZKProgressHUD.hide()
    }
}
