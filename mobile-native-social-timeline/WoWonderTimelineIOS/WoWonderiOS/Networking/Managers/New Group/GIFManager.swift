
import Foundation
import Alamofire
import WoWonderTimelineSDK


class GIFManager{
    
    static let instance = GIFManager()
    
    func getGIFS(limit:Int,offset:Int, completionBlock: @escaping (_ Success:GIFModel.GIFSuccessModel?, Error?) ->()){
        
        let params = [
            //            API.PARAMS.limit: limit,
            APIClient.Params.api_key: "b9427ca5441b4f599efa901f195c9f58",
            APIClient.Params.q: limit
            
            ] as [String : Any]
        
        Alamofire.request(APIClient.GIFs.GIFsApi, method: .get, parameters: params, encoding:URLEncoding.default, headers: nil).responseJSON { (response) in
            
            if (response.result.value != nil){
         
                let data = try! JSONSerialization.data(withJSONObject: response.value!, options: [])
                let result = try! JSONDecoder().decode(GIFModel.GIFSuccessModel.self, from: response.data!)
                completionBlock(result,nil)

            }else{
                print("error = \(response.error?.localizedDescription ?? "")")
                completionBlock(nil,response.error)
            }
        }
    }
}
