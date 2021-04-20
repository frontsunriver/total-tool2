
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetNewsFeedManagers{
    
    
    
    func get_News_Feed(access_token : String, limit : Int , off_set : String ,completionBlock : @escaping (_ Success:Get_News_FeedModel.get_News_Feed_SuccessModel?, _ AuthError : Get_News_FeedModel.get_News_Feed_ErrorModel? , Error?)->()){

        let body = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key, APIClient.Params.type : "get_news_feed", APIClient.Params.limit : limit, APIClient.Params.offset : off_set] as [String : Any]
        print("Params",body)
        print("URL",APIClient.Get_News_Feed.get_News_Feed_Posts + access_token)
        Alamofire.request(APIClient.Get_News_Feed.get_News_Feed_Posts + access_token, method: .post, parameters: body, encoding: URLEncoding.default, headers:
            nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                let result = Get_News_FeedModel.get_News_Feed_SuccessModel(json: res) 
                    print(result)
                    
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let alldata = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(Get_News_FeedModel.get_News_Feed_ErrorModel.self, from: alldata) else {return}
                    completionBlock(nil,result,nil)
                    
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static var sharedInstance = GetNewsFeedManagers()
    private init () {}
    
}
  
