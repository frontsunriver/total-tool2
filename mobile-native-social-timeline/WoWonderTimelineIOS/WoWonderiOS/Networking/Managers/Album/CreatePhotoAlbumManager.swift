

import Foundation
import Alamofire
import WoWonderTimelineSDK

class CreatePhotoAlbumManager {
    
    func createAlbum(data : [Data]?, albumName: String,completionBlock : @escaping (_ Success:CreateAlbumModel.createAlbum_SuccessModel?, _ AuthError : CreateAlbumModel.createAlbum_ErrorModel?, Error?)->()) {
        let headers = [APIClient.Params.headerKey:"application/x-www-form-urlencoded"]
        let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.type:"create", APIClient.Params.albumName:albumName] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"

        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in params {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
            for (index,data) in (data?.enumerated())!{
                multipartFormData.append(data, withName: "\(APIClient.Params.postPhotos)[]", fileName: "file.jpg", mimeType: "image/png")
            }
        },usingThreshold: UInt64.init(), to: APIClient.Album.albumApi + access_token,method: .post,headers: headers) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { (response) in
                    print(response.value)
                    if response.result.value != nil {
                        guard let res = response.result.value as? [String:Any] else {return}
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? Int == 200{
                            let result = CreateAlbumModel.createAlbum_SuccessModel.init(json: res)
                            completionBlock(result,nil,nil)
                        }
                        else {
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(CreateAlbumModel.createAlbum_ErrorModel.self, from: data) else {return}
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
    
    static var sharedInstance = CreatePhotoAlbumManager()
    private init() {}
    
}



