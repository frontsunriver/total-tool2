//
//  NavigatorViewController.swift
//  Rider
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit


class NavigatorViewController: UINavigationController, SideMenuItemContent {
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(NavigatorViewController.onMenuItemClicked), name: .menuClicked, object: nil)
        if #available(iOS 13.0, *) {
            let navBarAppearance = UINavigationBarAppearance()
            navBarAppearance.configureWithDefaultBackground()
            navigationBar.standardAppearance = navBarAppearance
            navigationBar.scrollEdgeAppearance = navBarAppearance
        }
    }
    @objc func onMenuItemClicked() {
        showSideMenu()
    }
}
