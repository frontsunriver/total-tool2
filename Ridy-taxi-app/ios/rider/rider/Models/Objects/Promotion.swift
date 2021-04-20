//
//  Promotion.swift
//
//  Created by Minimalistic Apps on 11/15/18
//  Copyright (c) . All rights reserved.
//

import Foundation

public final class Promotion: Codable {

  // MARK: Properties
  public var id: Int?
  public var description: String?
  public var title: String?
  public var startTimestamp: Double?
  public var expirationTimestamp: Double?
}
