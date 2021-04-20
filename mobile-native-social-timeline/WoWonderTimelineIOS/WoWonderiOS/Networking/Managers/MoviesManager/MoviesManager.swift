//
//  MoviesManager.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/23/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK
class MoviesManager{
    
    static let sharedInstance = MoviesManager()
     private init() {}
    
    func getMovies(limit:Int,offset: Int,genre:String,completionBlock :@escaping (_ Success: MoviesModel.MoviesSuccessModel?, _ AuthError: MoviesModel.MoviesErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.limit:limit,
                        APIClient.Params.genre:genre,
            APIClient.Params.offset:offset,
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Movies.getMovies + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = MoviesModel.MoviesSuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(MoviesModel.MoviesErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }

}
