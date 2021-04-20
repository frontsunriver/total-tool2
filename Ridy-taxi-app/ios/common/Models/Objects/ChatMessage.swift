
// This file was generated from JSON Schema using quicktype, do not modify it directly.
// To parse the JSON, add this file to your project and do:
//
//   let chatMessage = try? newJSONDecoder().decode(ChatMessage.self, from: jsonData)

import Foundation
import UIKit
import MessageKit

// MARK: - ChatMessageElement
public class ChatMessage: Codable, MessageType {
    let id: Int?
    let sentAt: Double?
    let content: String?
    let sentBy: ClientType?
    let state: String?
    
    public var sender: SenderType { get {
        if(self.sentBy == .Driver) {
            return Request.shared.driver!
        } else {
            return Request.shared.rider!
        }
        }
    }
    
    public var messageId: String { get {
        return String(self.id!)
        }
    }
    
    public var sentDate: Date { get {
        return Date(timeIntervalSince1970: sentAt! / 1000)
        }}
    
    public var kind: MessageKind { get {
        return MessageKind.text(self.content!)
        }
    }
}
public enum ClientType: String, Codable {
    case Driver = "d"
    case Rider = "r"
}
typealias ChatMessages = [ChatMessage]
