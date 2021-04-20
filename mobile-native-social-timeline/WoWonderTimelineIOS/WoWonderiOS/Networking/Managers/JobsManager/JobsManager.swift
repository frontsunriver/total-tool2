//
//  JobsManager.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/22/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK
class JobsManager{
    func getJobs(Type:String,limit:Int,offset: Int,distance:Int,completionBlock :@escaping (_ Success: jobsModel.jobsSuccessModel?, _ AuthError: jobsModel.jobsErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.type:Type,
            APIClient.Params.lenght:distance,
            APIClient.Params.limit:limit,
            APIClient.Params.offset:offset,
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Jobs.searchJob + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    let result = jobsModel.jobsSuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(jobsModel.jobsErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func ApplyJob(jobID:String,type:String,Name:String,Phone:String,location:String,email: String,position:String,work:String,description:String,fromDate:String,ToDateString:String, currentlywork:String,completionBlock :@escaping (_ Success: ApplyJobModel.ApplyJobSuccessModel?, _ AuthError: ApplyJobModel.ApplyJobErrorModel?, Error?)->()){
        let params = [
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.jobId:jobID,
            APIClient.Params.user_name:Name,
            APIClient.Params.type:type,
            APIClient.Params.phoneNumber:Phone,
            APIClient.Params.location:location,
            APIClient.Params.email:email,
            APIClient.Params.experience_description:description,
            APIClient.Params.experience_start_date:fromDate,
            APIClient.Params.experience_end_date:fromDate,
            APIClient.Params.i_currently_work:currentlywork,
            ] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Jobs.ApplyJob + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? Int == 200{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(ApplyJobModel.ApplyJobSuccessModel.self, from: data) else {return}
                                     completionBlock(result,nil,nil)
                }else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(ApplyJobModel.ApplyJobErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    func nearByBusiness(Type:String,limit:Int,offset: Int,distance:Int,completionBlock :@escaping (_ Success: jobsModel.jobsSuccessModel?, _ AuthError: jobsModel.jobsErrorModel?, Error?)->()){
          let params = [
              APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
              APIClient.Params.type:Type,
              APIClient.Params.distance:distance,
              APIClient.Params.limit:limit,
              APIClient.Params.offset:offset,
              ] as [String : Any]
          let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.NearByBusiness.nearByBusiness + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
              if response.result.value != nil{
                  guard let res = response.result.value as? [String:Any] else {return}
                  guard let apiStatusCode = res["api_status"] as? Any else {return}
                  if apiStatusCode as? Int == 200{
                      let result = jobsModel.jobsSuccessModel.init(json: res)
                      completionBlock(result,nil,nil)
                  }
                  else {
                      guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                      guard let result = try? JSONDecoder().decode(jobsModel.jobsErrorModel.self, from: data) else {return}
                      completionBlock(nil,result,nil)
                  }
              }
              else {
                  print(response.error?.localizedDescription)
                  completionBlock(nil,nil,response.error)
              }
          }
      }
  
    static let sharedInstance = JobsManager()
    private init() {}
    
}

