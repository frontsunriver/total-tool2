//
//  ChatViewController.swift
//  rider
//
//  Created by Manly Man on 7/21/19.
//  Copyright © 2019 minimal. All rights reserved.
//

import UIKit
import MessageKit
import Kingfisher
import InputBarAccessoryView

class ChatViewController: MessagesViewController {
    @objc func messageReceived(notification: Notification) {
        messages.append(notification.object as! ChatMessage)
        self.messagesCollectionView.reloadData()
        self.messagesCollectionView.scrollToBottom(animated: true)
    }
    
    var messages: [ChatMessage] = []
    var sender: SenderType!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        NotificationCenter.default.addObserver(self, selector: #selector(self.messageReceived), name: .messageReceived, object: nil)
        self.title = NSLocalizedString("Chat", comment: "Title of chat screen")
        messagesCollectionView.messagesDataSource = self
        messagesCollectionView.messagesLayoutDelegate = self
        messagesCollectionView.messagesDisplayDelegate = self
        self.messageInputBar.delegate = self
        if #available(iOS 13.0, *) {
            self.messagesCollectionView.backgroundColor = .systemBackground
        }
        if #available(iOS 13.0, *) {
            messageInputBar.inputTextView.textColor = .label
            messageInputBar.inputTextView.placeholderLabel.textColor = .secondaryLabel
            messageInputBar.backgroundView.backgroundColor = .systemBackground
        } else {
            messageInputBar.inputTextView.textColor = .black
            messageInputBar.inputTextView.backgroundColor = .white
        }
        GetMessages().execute() { result in
            switch result {
            case .success(let response):
                self.messages = response
                self.messagesCollectionView.reloadData()
                break;
                
            case .failure(let error):
                error.showAlert()
            }
            
        }
    }
    
    func insertMessage(_ message: ChatMessage) {
        messages.append(message)
        self.messagesCollectionView.reloadData()
        self.messagesCollectionView.scrollToBottom(animated: true)
    }
    
    func isNextMessageSameSender(at indexPath: IndexPath) -> Bool {
        guard indexPath.section + 1 < messages.count else { return false }
        return messages[indexPath.section].sentBy == messages[indexPath.section + 1].sentBy
    }
}

extension ChatViewController: MessagesDataSource {

    func currentSender() -> SenderType {
        return sender
    }
    
    func numberOfSections(in messagesCollectionView: MessagesCollectionView) -> Int {
        return messages.count
    }
    
    func messageForItem(at indexPath: IndexPath, in messagesCollectionView: MessagesCollectionView) -> MessageType {
        return messages[indexPath.section]
    }
    
    func configureAvatarView(_ avatarView: AvatarView, for message: MessageType, at indexPath: IndexPath, in messagesCollectionView: MessagesCollectionView) {
        let msg = message as! ChatMessage
        let url: URL
        if msg.sentBy == .Driver {
            let driver = msg.sender as! Driver
            url = URL(string: Config.Backend + (driver.media?.address?.replacingOccurrences(of: " ", with: "%20") ?? ""))!
            avatarView.kf.setImage(with: url, placeholder: UIImage(named: "nobody"))
            avatarView.isHidden = isNextMessageSameSender(at: indexPath)
        } else {
            //avatarView.image = UIImage(named: "nobody")
            //avatarView.isHidden = true
        }
        
        
    }
    
    
}

extension ChatViewController: InputBarAccessoryViewDelegate {
    
    func inputBar(_ inputBar: InputBarAccessoryView, didPressSendButtonWith text: String) {
        
        // Here we can parse for which substrings were autocompleted
        let attributedText = messageInputBar.inputTextView.attributedText!
        let range = NSRange(location: 0, length: attributedText.length)
        attributedText.enumerateAttribute(.autocompleted, in: range, options: []) { (_, range, _) in
            let substring = attributedText.attributedSubstring(from: range)
            let context = substring.attribute(.autocompletedContext, at: 0, effectiveRange: nil)
            print("Autocompleted: `", substring, "` with context: ", context ?? [])
        }
        messageInputBar.inputTextView.text = String()
        messageInputBar.invalidatePlugins()
        
        // Send button activity animation
        messageInputBar.sendButton.startAnimating()
        messageInputBar.inputTextView.placeholder = "Sending..."
        SendMessage(content: text).execute() { result in
            switch result {
            case .success(let response):
                self.messageInputBar.sendButton.stopAnimating()
                self.messageInputBar.inputTextView.placeholder = "Aa"
                self.insertMessage(response)
                self.messagesCollectionView.scrollToBottom(animated: true)
                
            case .failure(let error):
                self.messageInputBar.sendButton.stopAnimating()
                error.showAlert()
            }
        }
    }
    
    private func insertMessages(_ messages: [ChatMessage]) {
        for message in messages {
            insertMessage(message)
        }
    }
    
    
}


extension ChatViewController: MessagesDisplayDelegate, MessagesLayoutDelegate {}
