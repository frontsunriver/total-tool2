//
//  ShotsUITests.swift
//  ShotsUITests
//
//  Created by Manly Man on 4/2/20.
//  Copyright © 2020 minimal. All rights reserved.
//

import XCTest

class ShotsUITests {
    let app = XCUIApplication()
    
    /*override func setUpWithError() throws {
        // Put setup code here. This method is called before the invocation of each test method in the class.
        
        app.launch()
        // In UI tests it is usually best to stop immediately when a failure occurs.
        continueAfterFailure = false

        // In UI tests it’s important to set the initial state - such as interface orientation - required for your tests before they run. The setUp method is a good place to do this.
    }

    override func tearDownWithError() throws {
        // Put teardown code here. This method is called after the invocation of each test method in the class.
    }*/
    
    /*func testShot() throws {
        wait(for: [expectation(for: NSPredicate(format: "enabled == TRUE"), evaluatedWith: app.buttons["Confirm Location"], handler: nil)], timeout: 15)
        app.buttons["Confirm Location"].tap()
        wait(for: [expectation(for: NSPredicate(format: "enabled == TRUE"), evaluatedWith: app.buttons["Final Destination"], handler: nil)], timeout: 15)
        app.buttons["Final Destination"].tap()
        let element = app.collectionViews.children(matching: .cell).element(boundBy: 0).children(matching: .other).element.children(matching: .other).element
        let _ = element.waitForExistence(timeout: 15)
        element.tap()
        app.children(matching: .window).element(boundBy: 0).children(matching: .other).element.children(matching: .other).element.children(matching: .other).element.children(matching: .other).element.children(matching: .other).element.children(matching: .other).element.children(matching: .other).element(boundBy: 0).children(matching: .other).element(boundBy: 1).children(matching: .map).element.tap()
        snapshot("01MainScreen")
    }*/
}
