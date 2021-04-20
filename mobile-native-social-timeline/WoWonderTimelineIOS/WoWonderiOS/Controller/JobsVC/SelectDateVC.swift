//
//  SelectDateVC.swift
//  WoWonderiOS
//
//  Created by Muhammad Haris Butt on 7/22/20.
//  Copyright © 2020 clines329. All rights reserved.
//

import UIKit
protocol SelectYearDelegate {
    func selectYear(year:String,index:Int,type:Int)
}

class SelectDateVC: UIViewController {
    let jobCategories = [
        "Accounting",
        "General Business",
        "Admin & Clerical",
        "General Labor",
        "Pharmaceutical",
        "Automotive",
        "Government",
        "Professional Services",
        "Banking",
        "Grocery",
        "Purchasing",
        "Procurement",
        "Biotech",
        "Health Care",
        "QA",
        "Quality Control",
        "Broadcast",
        "Journalism",
        "Hotel",
        "Hospitality",
        "Real Estate",
        "Business Development",
        "Human Resources",
        "Research",
        "Construction",
        "Information Technology",
        "Restaurant",
        "Food Service",
        "Consultant",
        "Installation",
        "Maint",
        "Repair",
        "Retail",
        "Customer Service",
        "Insurance",
        "Sales",
        "Design",
        "Inventory",
        "Science",
        "Distribution",
        "Shipping",
        "Legal",
        "Skilled Labor",
        "Trades",
        "Education",
        "Teaching",
        "Legal Admin",
        "Strategy",
        "Planning",
        "Engineering",
        "Management",
        "Supply Chain",
        "Entry Level",
        "New Grad",
        "Manufacturing",
        "Telecommunications",
        "Executive",
        "Marketing",
        "Training",
        "Facilities",
        "Media",
        "Journalism",
        "Newspaper",
        "Transportation",
        "Finance",
        "Nonprofit",
        "Social Services",
        "Warehouse",
        "Franchise",
        "Nurse",
        "Other",
    ]
    var movieCategories = [
        "Default",
        "Action",
        "Comedy",
        "Drama","Horro",
        "Mythological",
        "War","Adventure",
        "Family",
        "Sport",
        "Animation",
        "Crime",
        "Fantasy",
        "Musical",
        "Romance",
        "Thriller",
        "History",
        "Documentry",
        "TV Show"
        
    ]
    var relationShipCategories = [
           "All",
           "Single",
           "In a Relationship",
           "Married",
           "Engaged"
           
       ]
    
    
    @IBOutlet weak var dateLabel: UILabel!
    @IBOutlet weak var tableView: UITableView!
    
    var type:Int? = 0
    var delegate:SelectYearDelegate?
    var years = [String]()
    var reversedYears = [String]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.years = (1970...2020).map { String($0) }
        self.reversedYears = self.years.reversed()
        if self.type == 0{
            self.dateLabel.text = NSLocalizedString("From Date", comment: "From Date")
        }else  if self.type == 1{
            self.dateLabel.text = NSLocalizedString("To Date", comment: "To Date")
        }else  if self.type == 2{
            self.dateLabel.text = NSLocalizedString("Position", comment: "Position")
        }else  if self.type == 3{
            self.dateLabel.text = NSLocalizedString("Select A Category", comment: "Select A Category")
        }else  if self.type == 4{
            self.dateLabel.text = NSLocalizedString("Relationship", comment: "Relationship")
        }
    }
    
    @IBAction func closePressed(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}
extension SelectDateVC:UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if self.type == 2{
            return self.jobCategories.count
        }else if self.type == 3{
            return self.movieCategories.count
        }else if self.type == 4{
            return self.relationShipCategories.count
        }else{
            return self.reversedYears.count
        }
        
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "cell") as? UITableViewCell
        if self.type == 2{
            cell?.textLabel!.text = self.jobCategories[indexPath.row]
        }else if self.type == 3{
            cell?.textLabel!.text = self.movieCategories[indexPath.row]
        }else if self.type == 4{
            cell?.textLabel!.text = self.relationShipCategories[indexPath.row]
        }else{
            cell?.textLabel!.text = self.reversedYears[indexPath.row]
        }
        
        return cell!
    }
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        self.dismiss(animated: true) {
            if self.type == 2{
                self.delegate?.selectYear(year: self.jobCategories[indexPath.row], index: indexPath.row, type: self.type ?? 0)
            }else if self.type == 3{
                self.delegate?.selectYear(year: self.movieCategories[indexPath.row], index: indexPath.row, type: self.type ?? 0)
            }else if self.type == 4{
                self.delegate?.selectYear(year: self.relationShipCategories[indexPath.row], index: indexPath.row, type: self.type ?? 0)
            }else{
                self.delegate?.selectYear(year: self.reversedYears[indexPath.row], index: indexPath.row, type: self.type ?? 0)
            }
        }
    }
}
