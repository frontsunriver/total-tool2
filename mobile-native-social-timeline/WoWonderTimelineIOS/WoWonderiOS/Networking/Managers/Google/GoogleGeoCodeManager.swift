

import Foundation
import Alamofire
import WoWonderTimelineSDK


class GoogleGeoCodeManager {
    
    func geoCode(address : String, completionBlock : @escaping (_ Success: GoogleGeoCodeModal.GoogleGeoCode_SuccessModal?, _ AuthError : GoogleGeoCodeModal.GoogleGeoCodeErrorModel? , Error?)->()){
        
        let params = ["address" : address, APIClient.Params.googleMapKey :  APIClient.Google_Key.google_Key] as [String : Any]
        
        Alamofire.request(APIClient.GoogleMap.googleMapApi, method: .get, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["status"]  as? Any else {return}
                let apiCodeString = apiStatusCode as? String
                
                if apiCodeString == "OK" {
                    guard let alldata = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    //                    print(response.value)
                    guard let result = try? JSONDecoder().decode(GoogleGeoCodeModal.GoogleGeoCode_SuccessModal.self, from: alldata) else {return}
                    //                    print(result)
                    completionBlock(result,nil,nil)
                    
                }
                    
                else if apiCodeString == "INVALID_REQUEST" {
                    let alldata = try? JSONSerialization.data(withJSONObject: response.value, options: [])
                    let result = try? JSONDecoder().decode(GoogleGeoCodeModal.GoogleGeoCodeErrorModel.self, from: alldata!)
                    completionBlock(nil,result,nil)
                    print(result)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sharedInstance = GoogleGeoCodeManager()
    private init() {}
    
}
