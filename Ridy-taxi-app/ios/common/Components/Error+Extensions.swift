//
//  Error+Extensions.swift
//  Shared
//
//  Created by Manly Man on 11/23/19.
//  Copyright © 2019 Innomalist. All rights reserved.
//

import Foundation
import SPAlert

public extension HTTPStatusCode {
    func showAlert() {
        SPAlert.present(title: NSLocalizedString("Error", comment: ""), message:  self.localizedDescription, preset: .error)
    }
}
