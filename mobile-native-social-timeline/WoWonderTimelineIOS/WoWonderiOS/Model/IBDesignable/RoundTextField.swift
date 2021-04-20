

import UIKit
@IBDesignable
class RoundTextField: UITextField {

  
    @IBInspectable var padding: CGFloat = 0

    override func textRect(forBounds bounds: CGRect) -> CGRect {
        return bounds.insetBy(dx: padding, dy: padding)
    }

    override func editingRect(forBounds bounds: CGRect) -> CGRect {
        return textRect(forBounds: bounds)
    }

   @IBInspectable var cornerRadius:CGFloat = 0 {
             didSet{
                 layer.cornerRadius = cornerRadius
                 layer.masksToBounds = cornerRadius > 0
             }
             
         }
      
      @IBInspectable var borderWidth:CGFloat = 0 {
          didSet{
              layer.borderWidth = borderWidth
              
          }
          
      }

      @IBInspectable var borderColor:UIColor = .white {
          didSet{
              layer.borderColor = borderColor.cgColor
              
          }
        
      }
    
    @IBInspectable var placeHolderColor: UIColor? {
           get {
               return self.placeHolderColor
           }
           set {
               self.attributedPlaceholder = NSAttributedString(string:self.placeholder != nil ? self.placeholder! : "", attributes:[NSAttributedString.Key.foregroundColor: newValue!])
           }
       }
    

}
