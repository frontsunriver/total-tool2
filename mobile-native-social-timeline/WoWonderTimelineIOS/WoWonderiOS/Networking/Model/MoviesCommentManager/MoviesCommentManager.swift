//
//  MoviesCommentManager.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/24/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import Foundation
import Alamofire
import WoWonderTimelineSDK


class MoviesCommentManager {
    func fetchComment (movieId : String, offset : String,completionBlock : @escaping (_ Success:FetchCommentModel.fetchComment_SuccessModel?, _ AuthError : FetchCommentModel.fetchComment_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.limit : 20, APIClient.Params.type : "get_comments",APIClient.Params.movie_id :movieId,APIClient.Params.offset : offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Movies_Comment.getMoviesComment + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                print("Res = \(response.result.value)")
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    let result = FetchCommentModel.fetchComment_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(FetchCommentModel.fetchComment_ErrorModel.self, from: data)else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    func likeComment (MovieId:String,commentId: String, type: String,reaction:String, completionBlock : @escaping (_ Success:
             LikeCommentModal.likeComment_SuccessModal?, _ AuthError : LikeCommentModal.likeComment_ErrorModal? , Error?)->()){
             
             let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.commentId:commentId, APIClient.Params.type:type,APIClient.Params.movie_id:MovieId,APIClient.Params.reaction_type:reaction]
             let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
         
          Alamofire.request(APIClient.Movies_Comment.MovieCommentLikeDislike + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                 if response.result.value != nil {
                     guard let res = response.result.value as? [String:Any] else {return}
                     guard let apiStatusCode = res["api_status"] as? Any else {return}
                     if apiStatusCode as? Int == 200{
                        guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                        guard let result = try? JSONDecoder().decode(LikeCommentModal.likeComment_SuccessModal.self, from: data) else {return}
                        completionBlock(result,nil,nil)
                     }
                     else {
                         guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                         guard let result = try? JSONDecoder().decode(LikeCommentModal.likeComment_ErrorModal.self, from: data) else {return}
                         completionBlock(nil,result,nil)
                     }
                 }
                 else {
                       print(response.error?.localizedDescription)
                       completionBlock(nil,nil,response.error)
                 }
             }
         }
    func addComment (MovieId:String, type: String, text:String,completionBlock : @escaping (_ Success:
        CreateCommentModel.createComment_SuccessModel?, _ AuthError : CreateCommentModel.createComment_ErrorModel? , Error?)->()){
               
               let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.type:type,APIClient.Params.movie_id:MovieId,APIClient.Params.text:text]
               let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
           
            Alamofire.request(APIClient.Movies_Comment.CreateMovieComment + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                   if response.result.value != nil {
                       guard let res = response.result.value as? [String:Any] else {return}
                       guard let apiStatusCode = res["api_status"] as? Any else {return}
                       if apiStatusCode as? Int == 200{
                          guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                        let result = CreateCommentModel.createComment_SuccessModel.init(json: res)
                          completionBlock(result,nil,nil)
                       }
                       else {
                           guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                           guard let result = try? JSONDecoder().decode(CreateCommentModel.createComment_ErrorModel.self, from: data) else {return}
                           completionBlock(nil,result,nil)
                       }
                   }
                   else {
                         print(response.error?.localizedDescription)
                         completionBlock(nil,nil,response.error)
                   }
               }
           }
    
    func fetchCommentReply (commentId : String, offset : String,completionBlock : @escaping (_ Success:FetchCommentModel.fetchComment_SuccessModel?, _ AuthError : FetchCommentModel.fetchComment_ErrorModel? , Error?)->()){
        let params = [APIClient.Params.serverKey : APIClient.SERVER_KEY.Server_Key,APIClient.Params.limit : 20, APIClient.Params.type : "reply_fetch",APIClient.Params.commentId :commentId,APIClient.Params.offset : offset] as [String : Any]
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.Movies_Comment.getCommentReplies + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            if response.result.value != nil {
                print("Res = \(response.result.value)")
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"]  as? Any else {return}
                let apiCode = apiStatusCode as? Int
                if apiCode == 200 {
                    let result = FetchCommentModel.fetchComment_SuccessModel.init(json: res)
                    completionBlock(result,nil,nil)
                }
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(FetchCommentModel.fetchComment_ErrorModel.self, from: data)else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
        
    }
    func commentReplyLikeDislike (MovieId:String,commentId: String, type: String,reaction:String, completionBlock : @escaping (_ Success:
                LikeCommentModal.likeComment_SuccessModal?, _ AuthError : LikeCommentModal.likeComment_ErrorModal? , Error?)->()){
                
                let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.commentId:commentId, APIClient.Params.type:type,APIClient.Params.movie_id:MovieId,APIClient.Params.reaction_type:type]
                let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
            
             Alamofire.request(APIClient.Movies_Comment.CommentReplyLikeDislikep + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                    if response.result.value != nil {
                        guard let res = response.result.value as? [String:Any] else {return}
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? Int == 200{
                           guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                           guard let result = try? JSONDecoder().decode(LikeCommentModal.likeComment_SuccessModal.self, from: data) else {return}
                           completionBlock(result,nil,nil)
                        }
                        else {
                            guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                            guard let result = try? JSONDecoder().decode(LikeCommentModal.likeComment_ErrorModal.self, from: data) else {return}
                            completionBlock(nil,result,nil)
                        }
                    }
                    else {
                          print(response.error?.localizedDescription)
                          completionBlock(nil,nil,response.error)
                    }
                }
            }
    func addCommentReply(MovieId:String, commentID:String,type: String, text:String,completionBlock : @escaping (_ Success:
           CreateCommentModel.createComment_SuccessModel?, _ AuthError : CreateCommentModel.createComment_ErrorModel? , Error?)->()){
                  
                  let params = [APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key, APIClient.Params.type:type,APIClient.Params.movie_id:MovieId,APIClient.Params.commentId:commentID,APIClient.Params.text:text]
                  let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
              
               Alamofire.request(APIClient.Movies_Comment.addCommentReply + access_token, method: .post, parameters: params, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
                      if response.result.value != nil {
                          guard let res = response.result.value as? [String:Any] else {return}
                          guard let apiStatusCode = res["api_status"] as? Any else {return}
                          if apiStatusCode as? Int == 200{
                             guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                           let result = CreateCommentModel.createComment_SuccessModel.init(json: res)
                             completionBlock(result,nil,nil)
                          }
                          else {
                              guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                              guard let result = try? JSONDecoder().decode(CreateCommentModel.createComment_ErrorModel.self, from: data) else {return}
                              completionBlock(nil,result,nil)
                          }
                      }
                      else {
                            print(response.error?.localizedDescription)
                            completionBlock(nil,nil,response.error)
                      }
                  }
              }
    
    static let sharedInstance = MoviesCommentManager()
    private init() {}
    
}
