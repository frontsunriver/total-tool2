
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GetPageReviewManager{
    
    func getPageReview(pageid: String,offset: String,completionBlock : @escaping (_ Success: GetPageReviewModal.getPageReview_SuccessModal?, _ AuthError :GetPageReviewModal.getPageReview_ErrorModal?, Error?)->()){
        
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.pageId:pageid, APIClient.Params.off_set:offset]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.GetPageReview.getPageReviewApi + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = GetPageReviewModal.getPageReview_SuccessModal.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else{return}
                    guard let result = try? JSONDecoder().decode(GetPageReviewModal.getPageReview_ErrorModal.self, from: data) else{return}
                    completionBlock(nil,result,nil)
                }
            }
            else{
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    static let sahredInstance = GetPageReviewManager()
    private init() {}
    
}
