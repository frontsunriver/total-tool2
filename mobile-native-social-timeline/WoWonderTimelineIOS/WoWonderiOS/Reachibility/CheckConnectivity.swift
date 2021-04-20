import WoWonderTimelineSDK
import Foundation
import UIKit

class CheckConnectivity : UIViewController{
    @objc func networkStatusChanged(_ notification: Notification) {
         if let userInfo = notification.userInfo {
             let status = userInfo["Status"] as! String
             print("Status",status)
         }
     }
}
