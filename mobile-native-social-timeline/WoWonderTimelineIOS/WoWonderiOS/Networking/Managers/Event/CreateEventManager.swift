
import Foundation
import Alamofire
import WoWonderTimelineSDK


class CreateEventManager{
    
    func createEvent(eventName :String,eventLocation :String,eventDescription :String,startDate :String,endDate :String,startTime :String,endTime :String,data :Data?, completionBlock : @escaping (_ Success :CreateEventModal.createEvent_SuccessModal?, _ AuthError : CreateEventModal.createEvent_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.eventName:eventName, APIClient.Params.eventLocation:eventLocation, APIClient.Params.eventDescription:eventDescription, APIClient.Params.eventStartDate:startDate, APIClient.Params.eventEndDate:endDate, APIClient.Params.eventStarttime:startTime, APIClient.Params.eventEndtime:endTime]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in params {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
           if let data = data{
            multipartFormData.append(data, withName: "event_cover", fileName: "file.jpg", mimeType: "image/png")
            }
        },usingThreshold: UInt64.init(), to: APIClient.Events.createEventApi + access_token,method: .post,headers: nil){
            (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { (response) in
                    print(response.value)
                    if response.result.value != nil {
                        guard let res = response.result.value as? [String:Any] else {return}
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? Int == 200{
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(CreateEventModal.createEvent_SuccessModal.self, from: data) else {return}
                            completionBlock(result,nil,nil)
                        }
                        else {
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(CreateEventModal.createEvent_ErrorModal.self, from: data) else {return}
                            completionBlock(nil,result,nil)
                        }
                    }
                    else {
                        print(response.error?.localizedDescription)
                        completionBlock(nil,nil,response.error)
                    }
                }
            case .failure(let error):
                print("Error in upload: \(error.localizedDescription)")
                completionBlock(nil,nil,error)
            }
        }
    }
    
    static let sharedInstance = CreateEventManager()
    private init() {}
}
