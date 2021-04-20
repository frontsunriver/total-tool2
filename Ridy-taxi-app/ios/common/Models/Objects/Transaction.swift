//
//  Transaction.swift
//
//  Created by Minimalistic Apps on 11/15/18
//  Copyright (c) . All rights reserved.
//

import Foundation

public final class Transaction: Codable {

  public var documentNumber: String?
  public var id: Int?
  public var transactionTime: Double?
  public var amount: Double?
  public var currency: String?
  public var transactionType: String?
}
