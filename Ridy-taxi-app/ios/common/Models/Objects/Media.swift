//
//  Media.swift
//
//  Copyright (c) Minimalistic apps. All rights reserved.
//

import Foundation

public final class Media: Codable {

    public enum PathType: String, Codable {
        case absolute = "absolute"
        case relative = "relative"
    }
    public enum PrivacyLevel: String, Codable {
        case low = "low"
        case medium = "medium"
        case high = "high"
    }
  // MARK: Declaration for string constants to be used to decode and also serialize.
  enum CodingKeys: String, CodingKey {
    case id = "id"
    case address = "address"
    case privacyLevel = "privacy_level"
    case title = "title"
    case type = "type"
    case pathType = "path_type"
  }

  // MARK: Properties
  public var id: Int?
  public var address: String?
  public var privacyLevel: PrivacyLevel?
  public var title: String?
  public var type: String?
  public var pathType: PathType?
}
