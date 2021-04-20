
import UIKit

@IBDesignable class RoundTextView: UITextView {

     @IBInspectable var padding: CGFloat = 0

      func textRect(forBounds bounds: CGRect) -> CGRect {
         return bounds.insetBy(dx: padding, dy: padding)
     }

     func editingRect(forBounds bounds: CGRect) -> CGRect {
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
    
}
