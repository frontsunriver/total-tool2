//
//  NavigatorViewController.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit


class NavigatorViewController: UINavigationController, SideMenuItemContent {
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(NavigatorViewController.onMenuItemClicked), name: .menuClicked, object: nil)
    }
    @objc func onMenuItemClicked() {
        showSideMenu()
    }
}
