import WoWonderTimelineSDK
import Foundation
import AVFoundation
import UIKit
extension FileManager{
    
    
    
    static func deleteFile(_ filePath:URL) {
        guard FileManager.default.fileExists(atPath: filePath.path) else{
            return
        }
        do {
            try FileManager.default.removeItem(atPath: filePath.path)
            print("Remove image success!!")
        }catch{
            fatalError("Unable to delete file: \(error) : \(#function).")
        }
    }
    
    
    
    
    
    func savePostImage(image:UIImage)->String {
        let documentDirectory: NSString = NSSearchPathForDirectoriesInDomains(.documentDirectory, .userDomainMask, true).first! as NSString
        
        // Set static name, so everytime image is cloned, it will be named "temp", thus rewrite the last "temp" image.
        // *Don't worry it won't be shown in Photos app.
        let imageName = String.randomStringWithLength(length: 10) + ".jpg"
        
        let imagePath = documentDirectory.appendingPathComponent(imageName)
        
        print("File Exist : \(self.fileExists(atPath: imagePath))")
        print(imagePath)
        
//        if self.fileExists(atPath: imagePath) {
//            try! self.removeItem(atPath: imagePath)
//            print("File removed")
//        }
        
        // Encode this image into JPEG. *You can add conditional based on filetype, to encode into JPEG or PNG
        if let data = image.jpegData(compressionQuality: 0.6) {
            // Save cloned image into document directory
            try! data.write(to: URL(fileURLWithPath: imagePath), options: Data.WritingOptions.atomic)
            print("Save image successfully.")
        }
        
        return imagePath
    }
    
    func saveThumbNailImage(image:UIImage)->String{
        
        let documentDirectory: NSString = NSSearchPathForDirectoriesInDomains(.documentDirectory, .userDomainMask, true).first! as NSString
        
        // Set static name, so everytime image is cloned, it will be named "temp", thus rewrite the last "temp" image.
        // *Don't worry it won't be shown in Photos app.
        let imageName = "thumbNail.jpg"
        
        let imagePath = documentDirectory.appendingPathComponent(imageName)
        
        print("File Exist : \(self.fileExists(atPath: imagePath))")
        print(imagePath)
        
        if self.fileExists(atPath: imagePath) {
            try! self.removeItem(atPath: imagePath)
            print("File removed")
        }
        
        // Encode this image into JPEG. *You can add conditional based on filetype, to encode into JPEG or PNG
        if let data = image.jpegData(compressionQuality: 0.6) {
            // Save cloned image into document directory
            try! data.write(to: URL(fileURLWithPath: imagePath), options: Data.WritingOptions.atomic)
            print("Save image successfully.")
        }
        
        return imagePath
    }
    
    func saveImage(image:UIImage)->String{
        
        let documentDirectory: NSString = NSSearchPathForDirectoriesInDomains(.documentDirectory, .userDomainMask, true).first! as NSString
        
        // Set static name, so everytime image is cloned, it will be named "temp", thus rewrite the last "temp" image.
        // *Don't worry it won't be shown in Photos app.
        let imageName = "profileTemp.jpg"
        
        let imagePath = documentDirectory.appendingPathComponent(imageName)
        
        print("File Exist : \(self.fileExists(atPath: imagePath))")
        print(imagePath)
        
        if self.fileExists(atPath: imagePath) {
            try! self.removeItem(atPath: imagePath)
            print("File removed")
        }
        
        // Encode this image into JPEG. *You can add conditional based on filetype, to encode into JPEG or PNG
        if let data = image.jpegData(compressionQuality: 0.6) {
            // Save cloned image into document directory
            try! data.write(to: URL(fileURLWithPath: imagePath), options: Data.WritingOptions.atomic)
            print("Save image successfully.")
        }
        
        return imagePath
    }
    
    func saveImage(image:UIImage){
        let documentDirectory: NSString = NSSearchPathForDirectoriesInDomains(.documentDirectory, .userDomainMask, true).first! as NSString
        
        // Set static name, so everytime image is cloned, it will be named "temp", thus rewrite the last "temp" image.
        // *Don't worry it won't be shown in Photos app.
        let imageName = "profileTemp.jpg"
        let imagePath = documentDirectory.appendingPathComponent(imageName)
        
        print("File Exist : \(self.fileExists(atPath: imagePath))")
        print(imagePath)
        
        if self.fileExists(atPath: imagePath) {
            do{
                try! self.removeItem(atPath: imagePath)
                print("File removed")
            }catch{
                print("File does't exist")
            }
        }
        
        // Encode this image into JPEG. *You can add conditional based on filetype, to encode into JPEG or PNG
        if let data = image.jpegData(compressionQuality: 0.6) {
            // Save cloned image into document directory
            do {
                try! data.write(to: URL(fileURLWithPath: imagePath), options: Data.WritingOptions.atomic)
                print("Save image successfully.")
            }catch{
                print("Save image failed.")
            }
        }
    }
}

