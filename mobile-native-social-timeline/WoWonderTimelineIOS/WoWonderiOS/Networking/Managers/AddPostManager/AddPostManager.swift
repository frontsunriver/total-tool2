

import Foundation
import Alamofire
import WoWonderTimelineSDK

class AddPostManager{
    
    static let instance = AddPostManager()
    
    func addPostText(userID:String,postText:String, postColor:String,postPrivacy:Int,pageID:String?,groupID:String?,eventID:String?,postType:String,completionBlock :@escaping (_ Success: AddPostModel.AddPostSuccessModel?, _ AuthError: AddPostModel.AddPostErrorModel?, Error?)->()){
        var param =  [String : Any]()
        if postType == "page"{
             param = [
            
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.userId:userID,
            APIClient.Params.s:UserData.getAccess_Token() ?? "",
            APIClient.Params.postText:postText,
            APIClient.Params.post_color:postColor,
            APIClient.Params.postPrivacy:postPrivacy,
              APIClient.Params.page_id:pageID ?? "",
            ]
        }else if postType == "group"{
            param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                        APIClient.Params.group_id:groupID ?? "",
                      ]
            
        }else if postType == "event"{
            param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                        APIClient.Params.event_id:eventID ?? "",
                      ]
        }else{
            param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                      ]
        }
        
        print("PARAMS= \(param)")
        print("URL",APIClient.AddPost.AddPostApi)
        let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
        Alamofire.request(APIClient.AddPost.AddPostApi, method: .post, parameters: param, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
            print(response.result.value)
            if response.result.value != nil{
                guard let res = response.result.value as? [String:Any] else {return}
                guard let apiStatusCode = res["api_status"] as? Any else {return}
                if apiStatusCode as? String == "200"{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    let result = AddPostModel.AddPostSuccessModel.init(json: res)
//                    try? JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data)
                    print("result",result)
                    completionBlock(result,nil,nil)
                }
                    
                else {
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    guard let result = try? JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data) else {return}
                    completionBlock(nil,result,nil)
                }
            }
            else {
                print(response.error?.localizedDescription)
                completionBlock(nil,nil,response.error)
            }
        }
    }
    
    func addImages(userID:String,postText:String, postColor:String,postPrivacy:Int,imageDataArray:[Data]?,pageID:String?,groupID:String?,eventID:String?,postType:String, completionBlock: @escaping (_ Success:AddPostModel.AddPostSuccessModel?,_ AuthError:AddPostModel.AddPostErrorModel?, Error?) ->()){
        
       var param =  [String : Any]()
        if postType == "page"{
             param = [
            
            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
            APIClient.Params.userId:userID,
            APIClient.Params.s:UserData.getAccess_Token() ?? "",
            APIClient.Params.postText:postText,
            APIClient.Params.post_color:postColor,
            APIClient.Params.postPrivacy:postPrivacy,
              APIClient.Params.page_id:pageID ?? "",
            ]
        }else if postType == "group"{
            param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                        APIClient.Params.group_id:groupID ?? "",
                      ]
            
        }else if postType == "event"{
            param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                        APIClient.Params.event_id:eventID ?? "",
                      ]
        }else{
            param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                      
                      
                      ]
        }
        
        let jsonData = try! JSONSerialization.data(withJSONObject: param, options: [])
        let decoded = String(data: jsonData, encoding: .utf8)!
        print("Decoded String = \(decoded)")
        let headers: HTTPHeaders = [
            "Content-type": "multipart/form-data"
        ]
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in param {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
            if imageDataArray!.count > 1{
                for data in imageDataArray!{
                    multipartFormData.append(data, withName: "postPhotos[]", fileName: "file.jpg", mimeType: "image/png")
                }
            }else{
                let data1 =  imageDataArray?[0]
                if let data = data1{
                    multipartFormData.append(data, withName: "postPhotos", fileName: "file.jpg", mimeType: "image/png")
                }
            }
            print("API =\(APIClient.AddPost.AddPostApi)")
        }, usingThreshold: UInt64.init(), to: APIClient.AddPost.AddPostApi, method: .post, headers: headers) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { response in
                    print("Succesfully uploaded")
                    print("response = \(response.result.value)")
                    if (response.result.value != nil){
                        guard let res = response.result.value as? [String:Any] else {return}
                        print("Response = \(res)")
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? String == "200"{
                            print("apiStatus Int = \(apiStatusCode)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = AddPostModel.AddPostSuccessModel.init(json: res)

//                            let result = try! JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data)
//                            print("Success = \(result.apiText ?? "")")
                            completionBlock(result,nil,nil)
                        }else{
                            print("apiStatus String = \(apiStatusCode)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = try! JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data)
                            print("AuthError = \(result.errors!.errorText)")
                            completionBlock(nil,result,nil)
                            
                        }
                        
                    }else{
                        print("error = \(response.error?.localizedDescription)")
                        completionBlock(nil,nil,response.error)
                    }
                }
            case .failure(let error):
                print("Error in upload: \(error.localizedDescription)")
                completionBlock(nil,nil,error)
                
            }
        }
    }
    func addVideo(userID:String,postText:String, postColor:String,postPrivacy:Int,videoData:Data?,pageID:String?,groupID:String?,eventID:String?,postType:String, completionBlock: @escaping (_ Success:AddPostModel.AddPostSuccessModel?,_ AuthError:AddPostModel.AddPostErrorModel?, Error?) ->()){
           
         var param =  [String : Any]()
                  if postType == "page"{
                       param = [
                      
                      APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                      APIClient.Params.userId:userID,
                      APIClient.Params.s:UserData.getAccess_Token() ?? "",
                      APIClient.Params.postText:postText,
                      APIClient.Params.post_color:postColor,
                      APIClient.Params.postPrivacy:postPrivacy,
                        APIClient.Params.page_id:pageID ?? "",
                      ]
                  }else if postType == "group"{
                      param = [
                                
                                APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                APIClient.Params.userId:userID,
                                APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                APIClient.Params.postText:postText,
                                APIClient.Params.post_color:postColor,
                                APIClient.Params.postPrivacy:postPrivacy,
                                  APIClient.Params.group_id:groupID ?? "",
                                ]
                      
                  }else if postType == "event"{
                      param = [
                                
                                APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                APIClient.Params.userId:userID,
                                APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                APIClient.Params.postText:postText,
                                APIClient.Params.post_color:postColor,
                                APIClient.Params.postPrivacy:postPrivacy,
                                  APIClient.Params.event_id:eventID ?? "",
                                ]
                  }else{
                      param = [
                                
                                APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                APIClient.Params.userId:userID,
                                APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                APIClient.Params.postText:postText,
                                APIClient.Params.post_color:postColor,
                                APIClient.Params.postPrivacy:postPrivacy,
                                
                                
                                ]
                  }
           
           let jsonData = try! JSONSerialization.data(withJSONObject: param, options: [])
           let decoded = String(data: jsonData, encoding: .utf8)!
           print("Decoded String = \(decoded)")
           let headers: HTTPHeaders = [
               "Content-type": "multipart/form-data"
           ]
           
           Alamofire.upload(multipartFormData: { (multipartFormData) in
               for (key, value) in param {
                   multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
               }
              if let data = videoData{
                            multipartFormData.append(data, withName: "postVideo", fileName: "video.mp4", mimeType: "video/mp4")
                         }
                        
           }, usingThreshold: UInt64.init(), to: APIClient.AddPost.AddPostApi, method: .post, headers: headers) { (result) in
               switch result{
               case .success(let upload, _, _):
                   upload.responseJSON { response in
                       print("Succesfully uploaded")
                       print("response = \(response.result.value)")
                       if (response.result.value != nil){
                           guard let res = response.result.value as? [String:Any] else {return}
                           print("Response = \(res)")
                           guard let apiStatusCode = res["api_status"] as? Any else {return}
                           if apiStatusCode as? String == "200"{
                               print("apiStatus Int = \(apiStatusCode)")
                               let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = AddPostModel.AddPostSuccessModel.init(json: res)

//                               let result = try! JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data)
//                               print("Success = \(result.apiText ?? "")")
                               completionBlock(result,nil,nil)
                           }else{
                               print("apiStatus String = \(apiStatusCode)")
                               let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                               let result = try! JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data)
                               print("AuthError = \(result.errors!.errorText)")
                               completionBlock(nil,result,nil)
                               
                           }
                           
                       }else{
                           print("error = \(response.error?.localizedDescription)")
                           completionBlock(nil,nil,response.error)
                       }
                   }
               case .failure(let error):
                   print("Error in upload: \(error.localizedDescription)")
                   completionBlock(nil,nil,error)
                   
               }
           }
       }
    func postGiF(userID:String,postText:String, postColor:String,postPrivacy:Int,GIFUrl:String,pageID:String?,groupID:String?,eventID:String?,postType:String,completionBlock :@escaping (_ Success: AddPostModel.AddPostSuccessModel?, _ AuthError: AddPostModel.AddPostErrorModel?, Error?)->()){
        var param =  [String : Any]()
                         if postType == "page"{
                              param = [
                             
                             APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                             APIClient.Params.userId:userID,
                             APIClient.Params.s:UserData.getAccess_Token() ?? "",
                             APIClient.Params.postText:postText,
                             APIClient.Params.post_color:postColor,
                             APIClient.Params.postPrivacy:postPrivacy,
                               APIClient.Params.page_id:pageID ?? "",
                               APIClient.Params.postSticker:GIFUrl,

                             ]
                         }else if postType == "group"{
                             param = [
                                       
                                       APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                       APIClient.Params.userId:userID,
                                       APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                       APIClient.Params.postText:postText,
                                       APIClient.Params.post_color:postColor,
                                       APIClient.Params.postPrivacy:postPrivacy,
                                         APIClient.Params.group_id:groupID ?? "",
                                         APIClient.Params.postSticker:GIFUrl,

                                       ]
                             
                         }else if postType == "event"{
                             param = [
                                       
                                       APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                       APIClient.Params.userId:userID,
                                       APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                       APIClient.Params.postText:postText,
                                       APIClient.Params.post_color:postColor,
                                       APIClient.Params.postPrivacy:postPrivacy,
                                         APIClient.Params.event_id:eventID ?? "",
                                         APIClient.Params.postSticker:GIFUrl,

                                       ]
                         }else{
                             param = [
                                       
                                       APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                       APIClient.Params.userId:userID,
                                       APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                       APIClient.Params.postText:postText,
                                       APIClient.Params.post_color:postColor,
                                       APIClient.Params.postPrivacy:postPrivacy,
                                       APIClient.Params.postSticker:GIFUrl,

                                       
                                       ]
                         }
        
          
          print("PARAMS= \(param)")
          let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
          Alamofire.request(APIClient.AddPost.AddPostApi, method: .post, parameters: param, encoding: JSONEncoding.default, headers: nil).responseJSON { (response) in
              if response.result.value != nil{
                  guard let res = response.result.value as? [String:Any] else {return}
                  guard let apiStatusCode = res["api_status"] as? Any else {return}
                  if apiStatusCode as? String == "200"{
                    guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    let result = AddPostModel.AddPostSuccessModel.init(json: res)

//                       let result = try? JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data)
                      completionBlock(result,nil,nil)
                  }
                      
                  else {
                      guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                      guard let result = try? JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data) else {return}
                      completionBlock(nil,result,nil)
                  }
              }
              else {
                  print(response.error?.localizedDescription)
                  completionBlock(nil,nil,response.error)
              }
          }
      }
    func postMusic(userID:String,postText:String, postColor:String,postPrivacy:Int,musicData:Data?,pageID:String?,groupID:String?,eventID:String?,postType:String, completionBlock: @escaping (_ Success:AddPostModel.AddPostSuccessModel?,_ AuthError:AddPostModel.AddPostErrorModel?, Error?) ->()){
              
               var param =  [String : Any]()
                              if postType == "page"{
                                   param = [
                                  
                                  APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                  APIClient.Params.userId:userID,
                                  APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                  APIClient.Params.postText:postText,
                                  APIClient.Params.post_color:postColor,
                                  APIClient.Params.postPrivacy:postPrivacy,
                                    APIClient.Params.page_id:pageID ?? "",
                                  ]
                              }else if postType == "group"{
                                  param = [
                                            
                                            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                            APIClient.Params.userId:userID,
                                            APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                            APIClient.Params.postText:postText,
                                            APIClient.Params.post_color:postColor,
                                            APIClient.Params.postPrivacy:postPrivacy,
                                              APIClient.Params.group_id:groupID ?? "",
                                            ]
                                  
                              }else if postType == "event"{
                                  param = [
                                            
                                            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                            APIClient.Params.userId:userID,
                                            APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                            APIClient.Params.postText:postText,
                                            APIClient.Params.post_color:postColor,
                                            APIClient.Params.postPrivacy:postPrivacy,
                                              APIClient.Params.event_id:eventID ?? "",
                                            ]
                              }else{
                                  param = [
                                            
                                            APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                            APIClient.Params.userId:userID,
                                            APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                            APIClient.Params.postText:postText,
                                            APIClient.Params.post_color:postColor,
                                            APIClient.Params.postPrivacy:postPrivacy,
                                            
                                            
                                            ]
                              }
              
              let jsonData = try! JSONSerialization.data(withJSONObject: param, options: [])
              let decoded = String(data: jsonData, encoding: .utf8)!
              print("Decoded String = \(decoded)")
              let headers: HTTPHeaders = [
                  "Content-type": "multipart/form-data"
              ]
              
              Alamofire.upload(multipartFormData: { (multipartFormData) in
                  for (key, value) in param {
                      multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
                  }
                 if let data = musicData{
                               multipartFormData.append(data, withName: "postMusic", fileName: "music.mp3", mimeType: "audio/mp3")
                            }
                           
              }, usingThreshold: UInt64.init(), to: APIClient.AddPost.AddPostApi, method: .post, headers: headers) { (result) in
                  switch result{
                  case .success(let upload, _, _):
                      upload.responseJSON { response in
                          print("Succesfully uploaded")
                          print("response = \(response.result.value)")
                          if (response.result.value != nil){
                              guard let res = response.result.value as? [String:Any] else {return}
                              print("Response = \(res)")
                              guard let apiStatusCode = res["api_status"] as? Any else {return}
                              if apiStatusCode as? String == "200"{
                                  print("apiStatus Int = \(apiStatusCode)")
                                  let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                                let result = AddPostModel.AddPostSuccessModel.init(json: res)

//                                  let result = try! JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data)
//                                  print("Success = \(result.apiText ?? "")")
                                  completionBlock(result,nil,nil)
                              }else{
                                  print("apiStatus String = \(apiStatusCode)")
                                  let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                                  let result = try! JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data)
                                  print("AuthError = \(result.errors!.errorText)")
                                  completionBlock(nil,result,nil)
                                  
                              }
                              
                          }else{
                              print("error = \(response.error?.localizedDescription)")
                              completionBlock(nil,nil,response.error)
                          }
                      }
                  case .failure(let error):
                      print("Error in upload: \(error.localizedDescription)")
                      completionBlock(nil,nil,error)
                      
                  }
              }
          }
    func postFIle(userID:String,postText:String, postColor:String,postPrivacy:Int,fileData:Data?,extension1:String,pageID:String?,groupID:String?,eventID:String?,postType:String, completionBlock: @escaping (_ Success:AddPostModel.AddPostSuccessModel?,_ AuthError:AddPostModel.AddPostErrorModel?, Error?) ->()){
        
          var param =  [String : Any]()
                                    if postType == "page"{
                                         param = [
                                        
                                        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                        APIClient.Params.userId:userID,
                                        APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                        APIClient.Params.postText:postText,
                                        APIClient.Params.post_color:postColor,
                                        APIClient.Params.postPrivacy:postPrivacy,
                                          APIClient.Params.page_id:pageID ?? "",
                                        ]
                                    }else if postType == "group"{
                                        param = [
                                                  
                                                  APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                                  APIClient.Params.userId:userID,
                                                  APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                                  APIClient.Params.postText:postText,
                                                  APIClient.Params.post_color:postColor,
                                                  APIClient.Params.postPrivacy:postPrivacy,
                                                    APIClient.Params.group_id:groupID ?? "",
                                                  ]
                                        
                                    }else if postType == "event"{
                                        param = [
                                                  
                                                  APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                                  APIClient.Params.userId:userID,
                                                  APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                                  APIClient.Params.postText:postText,
                                                  APIClient.Params.post_color:postColor,
                                                  APIClient.Params.postPrivacy:postPrivacy,
                                                    APIClient.Params.event_id:eventID ?? "",
                                                  ]
                                    }else{
                                        param = [
                                                  
                                                  APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                                  APIClient.Params.userId:userID,
                                                  APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                                  APIClient.Params.postText:postText,
                                                  APIClient.Params.post_color:postColor,
                                                  APIClient.Params.postPrivacy:postPrivacy,
                                                  
                                                  
                                                  ]
                                    }
        
        let jsonData = try! JSONSerialization.data(withJSONObject: param, options: [])
        let decoded = String(data: jsonData, encoding: .utf8)!
        print("Decoded String = \(decoded)")
        let headers: HTTPHeaders = [
            "Content-type": "multipart/form-data"
        ]
        
        Alamofire.upload(multipartFormData: { (multipartFormData) in
            for (key, value) in param {
                multipartFormData.append("\(value)".data(using: String.Encoding.utf8)!, withName: key as String)
            }
           if let data = fileData{
                         multipartFormData.append(data, withName: "postFile", fileName: "file.\(extension1)", mimeType: "file/\(extension1)")
                      }
                     
        }, usingThreshold: UInt64.init(), to: APIClient.AddPost.AddPostApi, method: .post, headers: headers) { (result) in
            switch result{
            case .success(let upload, _, _):
                upload.responseJSON { response in
                    print("Succesfully uploaded")
                    print("response = \(response.result.value)")
                    if (response.result.value != nil){
                        guard let res = response.result.value as? [String:Any] else {return}
                        print("Response = \(res)")
                        guard let apiStatusCode = res["api_status"] as? Any else {return}
                        if apiStatusCode as? String == "200"{
                            print("apiStatus Int = \(apiStatusCode)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = AddPostModel.AddPostSuccessModel.init(json: res)

//                            let result = try! JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data)
//                            print("Success = \(result.apiText ?? "")")
                            completionBlock(result,nil,nil)
                        }else{
                            print("apiStatus String = \(apiStatusCode)")
                            let data = try! JSONSerialization.data(withJSONObject: response.value, options: [])
                            let result = try! JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data)
                            print("AuthError = \(result.errors!.errorText)")
                            completionBlock(nil,result,nil)
                            
                        }
                        
                    }else{
                        print("error = \(response.error?.localizedDescription)")
                        completionBlock(nil,nil,response.error)
                    }
                }
            case .failure(let error):
                print("Error in upload: \(error.localizedDescription)")
                completionBlock(nil,nil,error)
                
            }
        }
    }
    func addFeeling(userID:String,postText:String, postColor:String,postPrivacy:Int,feelingName:String,feelingType:String,pageID:String?,groupID:String?,eventID:String?,postType:String,completionBlock :@escaping (_ Success: AddPostModel.AddPostSuccessModel?, _ AuthError: AddPostModel.AddPostErrorModel?, Error?)->()){
        var param =  [String : Any]()
                                          if postType == "page"{
                                               param = [
                                              
                                              APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                              APIClient.Params.userId:userID,
                                              APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                              APIClient.Params.postText:postText,
                                              APIClient.Params.post_color:postColor,
                                              APIClient.Params.postPrivacy:postPrivacy,
                                                APIClient.Params.page_id:pageID ?? "",
                                                APIClient.Params.feeling:feelingName,
                                                APIClient.Params.feeling_type:feelingType,
                                              ]
                                          }else if postType == "group"{
                                              param = [
                                                        
                                                        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                                        APIClient.Params.userId:userID,
                                                        APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                                        APIClient.Params.postText:postText,
                                                        APIClient.Params.post_color:postColor,
                                                        APIClient.Params.postPrivacy:postPrivacy,
                                                          APIClient.Params.group_id:groupID ?? "",
                                                          APIClient.Params.feeling:feelingName,
                                                          APIClient.Params.feeling_type:feelingType,
                                                        ]
                                              
                                          }else if postType == "event"{
                                              param = [
                                                        
                                                        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                                        APIClient.Params.userId:userID,
                                                        APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                                        APIClient.Params.postText:postText,
                                                        APIClient.Params.post_color:postColor,
                                                        APIClient.Params.postPrivacy:postPrivacy,
                                                          APIClient.Params.event_id:eventID ?? "",
                                                          APIClient.Params.feeling:feelingName,
                                                          APIClient.Params.feeling_type:feelingType,
                                                        ]
                                          }else{
                                              param = [
                                                        
                                                        APIClient.Params.serverKey:APIClient.SERVER_KEY.Server_Key,
                                                        APIClient.Params.userId:userID,
                                                        APIClient.Params.s:UserData.getAccess_Token() ?? "",
                                                        APIClient.Params.postText:postText,
                                                        APIClient.Params.post_color:postColor,
                                                        APIClient.Params.postPrivacy:postPrivacy,
                                                        APIClient.Params.feeling:feelingName,
                                                        APIClient.Params.feeling_type:feelingType,
                                                        
                                                        
                                                        ]
                                          }
          
          
          print("PARAMS= \(param)")
          let access_token = "\("?")\("access_token")\("=")\(UserData.getAccess_Token()!)"
          Alamofire.request(APIClient.AddPost.AddPostApi, method: .post, parameters: param, encoding: URLEncoding.default, headers: nil).responseJSON { (response) in
              if response.result.value != nil{
                  guard let res = response.result.value as? [String:Any] else {return}
                  guard let apiStatusCode = res["api_status"] as? Any else {return}
                  if apiStatusCode as? String == "200"{
                      guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                    let result = AddPostModel.AddPostSuccessModel.init(json: res)

//                      guard let result = try? JSONDecoder().decode(AddPostModel.AddPostSuccessModel.self, from: data) else {return}
                      completionBlock(result,nil,nil)
                  }
                      
                  else {
                      guard let data = try? JSONSerialization.data(withJSONObject: response.value, options: []) else {return}
                      guard let result = try? JSONDecoder().decode(AddPostModel.AddPostErrorModel.self, from: data) else {return}
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
