

import UIKit
import WoWonderTimelineSDK
import OneSignal
class BaseVC: UIViewController {
    
    var oneSignalID:String? = ""


    override func viewDidLoad() {
        super.viewDidLoad()

        oneSignalID = OneSignal.getPermissionSubscriptionState().subscriptionStatus.userId
                    print("Current playerId \(oneSignalID)")
                    UserDefaults.standard.setDeviceId(value: oneSignalID ?? "", ForKey: "deviceID")
        
    }
    
    func convertTime(miliseconds: Int) -> String {

        var seconds: Int = 0
        var minutes: Int = 0
        var hours: Int = 0
        var days: Int = 0
        var secondsTemp: Int = 0
        var minutesTemp: Int = 0
        var hoursTemp: Int = 0

        if miliseconds < 1000 {
            return ""
        } else if miliseconds < 1000 * 60 {
            seconds = miliseconds / 1000
            return "\(seconds) seconds"
        } else if miliseconds < 1000 * 60 * 60 {
            secondsTemp = miliseconds / 1000
            minutes = secondsTemp / 60
            seconds = (miliseconds - minutes * 60 * 1000) / 1000
            return "\(minutes) minutes, \(seconds) seconds"
        } else if miliseconds < 1000 * 60 * 60 * 24 {
            minutesTemp = miliseconds / 1000 / 60
            hours = minutesTemp / 60
            minutes = (miliseconds - hours * 60 * 60 * 1000) / 1000 / 60
            seconds = (miliseconds - hours * 60 * 60 * 1000 - minutes * 60 * 1000) / 1000
            return "\(hours) hours, \(minutes) minutes, \(seconds) seconds"
        } else {
            hoursTemp = miliseconds / 1000 / 60 / 60
            days = hoursTemp / 24
            hours = (miliseconds - days * 24 * 60 * 60 * 1000) / 1000 / 60 / 60
            minutes = (miliseconds - days * 24 * 60 * 60 * 1000 - hours * 60 * 60 * 1000) / 1000 / 60
            seconds = (miliseconds - days * 24 * 60 * 60 * 1000 - hours * 60 * 60 * 1000 - minutes * 60 * 1000) / 1000
            return "\(days) days, \(hours) hours, \(minutes) minutes, \(seconds) seconds"
        }
    }

}
