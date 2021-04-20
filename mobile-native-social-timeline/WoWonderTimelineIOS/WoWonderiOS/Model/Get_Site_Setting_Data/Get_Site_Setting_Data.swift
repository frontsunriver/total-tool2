

import Foundation
import CoreData
import UIKit

class Get_Site_Setting_Data {
    
    let appDelegate = UIApplication.shared.delegate as! AppDelegate
    
    
    var array = [String: Get_Site_SettingModel.PostColor]()
    
    func load_Site_Setting_Data() {
        
        Get_Site_Setting_Manager.sharedInstance.get_Site_Setting { (success, authError, error) in
            if success != nil {
                let values = (success?.config!.postColors?.values)
                print(values)
                let managedContext = self.appDelegate.persistentContainer.viewContext
                
//                let data = PostColor(context: managedContext)
                
//                for i in values!{
//
//                    data.id = Int32(i.id)
//                    data.color_1 = i.color1
//                    data.color_2 = i.color2
//                    data.text_color = i.textColor
//                    data.time = i.time
//                    data.image = i.image
//
//                }
                
                let photoEntity = NSEntityDescription.insertNewObject(forEntityName: "PostColor", into: managedContext) as? PostColor
                for i in values! {

                    photoEntity?.setValue(i.id, forKey: "id")
                    photoEntity?.setValue(i.color1, forKey: "color_1")
                    photoEntity?.setValue(i.color2, forKey: "color_2")
                    photoEntity?.setValue(i.image, forKey: "image")
                    photoEntity?.setValue(i.textColor, forKey: "text_color")
                    photoEntity?.setValue(i.textColor, forKey: "time")


                }
                do {
                    try managedContext.save()
                }
                catch let error as NSError {
                    print("could not save\(error),\(error.userInfo)")
                }
                
                
                self.fetchData ()
                
            }
            else if authError != nil {
                print("serverError")
            }
            else if error != nil {
                
                print("InternalError")
            }
        }
        
        
    }
    
    
    func  fetchData () {
//        let companyFetchReq:NSFetchRequest<Company> = Company.fetchRequest()

//        let fetchRequest = NSFetchRequest<NSFetchRequestResult>(entityName: "PostColor")
        let fetchRequest : NSFetchRequest<PostColor> = PostColor.fetchRequest()
        let managedContext = self.appDelegate.persistentContainer.viewContext
       
        do {
            
            let result  = try? managedContext.fetch(fetchRequest)
            for i in result!{
                print(i.id)
            }
            
            
            
        }
        catch{
            print("failed Load Data")
        }
        
    }
    
    
    
    static let sharedInstance = Get_Site_Setting_Data()
    private init() {}
    
}
