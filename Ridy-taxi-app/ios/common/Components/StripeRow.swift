import UIKit
import Eureka
import Stripe

public final class StripeCell: Cell<String>, CellType, STPPaymentCardTextFieldDelegate {
    
    public var paymentField = STPPaymentCardTextField()
    
    required init(style: UITableViewCell.CellStyle, reuseIdentifier: String?) {
        super.init(style: style, reuseIdentifier: reuseIdentifier)
        paymentField = STPPaymentCardTextField()
        paymentField.backgroundColor = UIColor.clear
        paymentField.textColor  = UIColor.black
        self.contentView.backgroundColor = UIColor.clear
        self.backgroundColor = UIColor.white
        paymentField.frame = contentView.bounds
        self.contentView.addSubview(paymentField)
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override public func setup() {
        super.setup()
        
        selectionStyle = .none
        
        paymentField.backgroundColor = .white
        paymentField.cornerRadius = 0.0
        paymentField.borderColor = .clear
        
        //paymentField.delegate = self
        
        height = { return 44 }
    }
    
    override public func update() {
        super.update()
        textLabel?.text = nil
    }
    
    public func paymentCardTextFieldDidChange(_ textField: STPPaymentCardTextField) {
        formViewController()?.beginEditing(of: self)
    }
}

/// An Eureka Row for Stripe's STPPaymentCardTextField.
public final class StripeRow: Row<StripeCell>, RowType {
    required init(tag: String?) {
        super.init(tag: tag)
        cellProvider = CellProvider<StripeCell>()
    }
}
