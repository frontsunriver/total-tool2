import WoWonderTimelineSDK
import Foundation
import UIKit

class UserData {
    
    static let preference = UserDefaults.standard
    static fileprivate let USER_NAME: String = "USER_NAME"
    static fileprivate let USER_EMAIL: String = "USER_EMAIL"
    static fileprivate let USER_ID: String = "USER_ID"
    static fileprivate let USER_LoginType = "USER_LoginType"
    static fileprivate let Access_token: String = "Access_token"
    static fileprivate let Wallet: String = "Wallet"
    static fileprivate let Image: String = "Image"
    static fileprivate let cover: String = "Cover"
    static fileprivate let points: String = "Points"
    static fileprivate let followers: String = "followers"
    static fileprivate let following: String = "following"
    static fileprivate let  likes: String = "Likes"
    static fileprivate let isPro: String = "isPro"

   
    static func setUSER_ID(_ userId: String?){
        preference.setValue(userId, forKey: USER_ID)
        preference.synchronize()
    }
    
    static func setaccess_token(_ token: String?){
           preference.setValue(token, forKey: Access_token)
           preference.synchronize()
       }
    
    
    static func setUSER_NAME(_ userName: String?){
        preference.setValue(userName, forKey: USER_NAME)
        preference.synchronize()
    }
    static func setUser_Email(_ userEmail: String?){
        preference.setValue(userEmail, forKey: USER_EMAIL)
        preference.synchronize()
    }
    
    static func setUSER_LoginType(_ userLoginType : String?){
    preference.setValue(userLoginType, forKey: USER_LoginType)
           preference.synchronize()
    }
    static func setWallet (_ wallet: String?){
        preference.set(wallet, forKey: Wallet)
    }
    static func SetImage(_ image: String?){
        preference.set(image, forKey: Image)
    }
    static func SetisPro(_ is_Pro: String?){
        preference.set(is_Pro, forKey: isPro)
    }
    
    static func getUSER_NAME() -> String?{
        return preference.string(forKey: USER_NAME)
    }
    static func getUSER_EMAIL() -> String?{
        return preference.string(forKey: USER_EMAIL)
    }
    static func getUSER_ID() -> String?{
        return preference.string(forKey: USER_ID)
    }
    static func getAccess_Token() -> String?{
           return preference.string(forKey: Access_token)
       }
    static func getUSER_LoginType() -> String?{
        return preference.string(forKey:USER_LoginType)
    }
    static func getWallet() -> String?{
        return preference.string(forKey: Wallet)
    }
    
    static func getImage() -> String?{
        return preference.string(forKey: Image)
    }
    static func getIsPro() -> String?{
        return preference.string(forKey: isPro)
    }
    
    
    static func clear(){
        let appDomain = Bundle.main.bundleIdentifier!
        UserDefaults.standard.removePersistentDomain(forName: appDomain)
        preference.synchronize()
        
    }
    
    
    
    
    
    
}
