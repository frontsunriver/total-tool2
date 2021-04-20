//
//  TravelTableViewController.swift
//  Rider
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import SPAlert

class TripHistoryCollectionViewController: UICollectionViewController, UICollectionViewDelegateFlowLayout {
    //MARK: Properties
    let cellIdentifier = "TripHistoryCollectionViewCell"
    
    var travels = [Request]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        let nibCell = UINib(nibName: cellIdentifier, bundle: nil)
        collectionView?.register(nibCell, forCellWithReuseIdentifier: cellIdentifier)
        self.refreshList(self)
    }

    @IBAction func refreshList(_ sender: Any) {
        GetRequestHistory().execute() { result in
            switch result {
            case .success(let response):
                self.travels = response
                self.collectionView?.reloadData()
                
            case .failure(let error):
                error.showAlert()
            }
        }
    }
    
    func collectionView(_ collectionView: UICollectionView, layout collectionViewLayout: UICollectionViewLayout, sizeForItemAt indexPath: IndexPath) -> CGSize {
        let kWhateverHeightYouWant = 150
        return CGSize(width: collectionView.bounds.size.width - 16, height: CGFloat(kWhateverHeightYouWant))
    }
    
    @available(iOS 13.0, *)
    override func collectionView(_ collectionView: UICollectionView, contextMenuConfigurationForItemAt indexPath: IndexPath, point: CGPoint) -> UIContextMenuConfiguration? {
        return UIContextMenuConfiguration(identifier: nil, previewProvider: nil, actionProvider: { suggestedActions in
            let complaint = UIAction(title: NSLocalizedString("Write Complaint", comment: ""), image: UIImage(systemName: "square.and.pencil")) { action in
                let title = NSLocalizedString("Complaint", comment: "")
                let message = NSLocalizedString("You can write a complaint on service provided and it will be reviewed As soon as possible.", comment: "")
                let dialog = UIAlertController(title: title, message: message, preferredStyle: .alert)
                dialog.addTextField() { textField in
                    textField.placeholder = NSLocalizedString("Title...", comment: "")
                }
                dialog.addTextField() { textField in
                    textField.placeholder = NSLocalizedString("Content...", comment: "")
                }
                dialog.addAction(UIAlertAction(title: NSLocalizedString("OK", comment: ""), style: .default) { action in
                    WriteComplaint(requestId: self.travels[indexPath.row].id!, subject: dialog.textFields![0].text!, content: dialog.textFields![1].text!).execute() { result in
                        switch result {
                        case .success(_):
                            SPAlert.present(title: NSLocalizedString("Complaint Sent", comment: ""), preset: .done)
                            
                        case .failure(let error):
                            error.showAlert()
                        }
                    }
                })
                dialog.addAction(UIAlertAction(title: NSLocalizedString("Cancel", comment: ""), style: .cancel))
                self.present(dialog, animated: true)
            }

            // Here we specify the "destructive" attribute to show that it’s destructive in nature
            let hide = UIAction(title: NSLocalizedString("Hide Item", comment: ""), image: UIImage(systemName: "eye.slash"), attributes: .destructive) { action in
                HideHistoryItem(requestId: self.travels[indexPath.row].id!).execute() { result in
                    switch result {
                    case .success(_):
                        self.refreshList(self)
                        
                    case .failure(let error):
                        error.showAlert()
                    }
                }
            }
            var buttons = [complaint]
            if SocketNetworkDispatcher.instance.userType == .Rider {
                buttons.append(hide)
            }
            // The "title" will show up as an action for opening this menu
            //let edit = UIMenu(title: "Write Complaint", children: [complaint])

            // Create our menu with both the edit menu and the share action
            return UIMenu(title: NSLocalizedString("Options", comment: ""), children: buttons)
        })
    }
    
    override func numberOfSections(in tableView: UICollectionView) -> Int {
        return 1
    }

    override func collectionView(_ collectionView: UICollectionView, numberOfItemsInSection section: Int) -> Int {
        return travels.count
    }
    
    override func collectionView(_ collectionView: UICollectionView, cellForItemAt indexPath: IndexPath) -> UICollectionViewCell {
        guard let cell = self.collectionView?.dequeueReusableCell(withReuseIdentifier: cellIdentifier, for: indexPath) as? TripHistoryCollectionViewCell  else {
            fatalError("The dequeued cell is not an instance of TripHistoryTableCell.")
        }
        let travel = travels[indexPath.row]
        let dateFormatter = DateFormatter()
        dateFormatter.dateStyle = .medium
        dateFormatter.timeStyle = .medium
        cell.pickupLabel.text = travel.addresses[0]
        if travel.addresses.count > 1 {
            cell.destinationLabel.text = travel.addresses.last
        }
        if let startTimestamp = travel.startTimestamp {
            cell.startTimeLabel.text = dateFormatter.string(from: Date(timeIntervalSince1970: TimeInterval(startTimestamp / 1000)))
        }
        if let finishTimestamp = travel.finishTimestamp {
            cell.finishTimeLabel.text = dateFormatter.string(from: Date(timeIntervalSince1970: TimeInterval(finishTimestamp / 1000)))
        }
        cell.textCost.text = MyLocale.formattedCurrency(amount: travel.costAfterCoupon ?? 0, currency: travel.currency!)
        
        cell.textStatus.text = travel.status!.rawValue.splitBefore(separator: { $0.isUppercase }).map{String($0)}.joined(separator: " ")
        return cell
    }
}

extension Character {
    var isUpperCase: Bool { return String(self) == String(self).uppercased() }
}

extension Sequence {
    func splitBefore(
        separator isSeparator: (Iterator.Element) throws -> Bool
    ) rethrows -> [AnySequence<Iterator.Element>] {
        var result: [AnySequence<Iterator.Element>] = []
        var subSequence: [Iterator.Element] = []

        var iterator = self.makeIterator()
        while let element = iterator.next() {
            if try isSeparator(element) {
                if !subSequence.isEmpty {
                    result.append(AnySequence(subSequence))
                }
                subSequence = [element]
            }
            else {
                subSequence.append(element)
            }
        }
        result.append(AnySequence(subSequence))
        return result
    }
}
