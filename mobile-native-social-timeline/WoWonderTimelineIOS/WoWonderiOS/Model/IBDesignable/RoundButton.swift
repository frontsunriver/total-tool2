

import UIKit
@IBDesignable
class RoundButton: UIButton {

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
