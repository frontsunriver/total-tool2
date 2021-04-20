//
//  ColoredButton.swift
//  rider
//
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit

@IBDesignable
public class ColoredButton: UIButton {
    
    // MARK: Overrides
    
    override public func prepareForInterfaceBuilder() {
        super.prepareForInterfaceBuilder()
        redraw()
    }
    
    override public func layoutSubviews() {
        super.layoutSubviews()
        redraw()
    }
    
    override public var isEnabled: Bool {
        didSet {
            layoutSubviews()
        }
    }
    
    // MARK: Private
    
    private func redraw() {
        self.layer.cornerRadius = 6
        self.setTitleColor(UIColor.white, for: .normal)
        self.contentEdgeInsets = UIEdgeInsets(top: 10.0, left: 0, bottom: 10.0, right: 0)
        let color = UIApplication.shared.keyWindow?.tintColor ?? self.tintColor
        if self.isEnabled {
            self.backgroundColor = color
        } else {
            self.backgroundColor = UIColor.gray
        }
    }
}
