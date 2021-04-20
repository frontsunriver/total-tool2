

import UIKit
import WoWonderTimelineSDK

class SearchLocationController: UIViewController,UISearchBarDelegate{

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var searchBar: UISearchBar!
    
    var delegate: SearchDelegate!
    
    var searching = false
    
    var countryList = ["All","United States","Canda","Afghanistan","Albania","Algeria","American Samoa","Andorra","Angola","Anguilla","Antractica","Antigua and/ or Barbuda","Argentina","Armenia","Aruba","Australia","Austria","Azerbaijan","Bahamas","Bahrian","Bangladesh","Barbados","Belarus","Belgium","Belize","Benin","Brmuda","Bhutan","Bolivia","Bosnia and Herzergovina","Botsawana","Bouvet Island","brazil","Brunei Darussalam","Bulgaria","Burkina Faso","Burundi","Cambodia","Cameroon","Cape Verde","Cayman Islands", "Centeral African Republic","Chad","Chile","China","Christmas Island","Cocos (Keeling) Island","Colombia","Comoros","Congo","Cook Island","Costa Rica","Croatia (Hrvatska)","Cuba","Cyprus","Czech REpublic", "Denmark","Djibouti","Dominica","Dominican Republic","East Timor","Ecuador","Egypt","EL Salvador","Equatorial Guinea","Eritrea","Estonia","Ethiopia","Falkland Island (Malvinas)","Faroe Island","Fiji","Finland","France","France Metropolitan","French Guiana","French Polynesia","French Southeren Territories","GAbon","Gambia","Georgia","Germany","Ghana","Gibraltar","Greece","Greenland","Grenada","Guadeloupe","Guam","Guatemala","Guinea","Guinea-Bissau","Guyana","Haiti","Heard and Mc Donald Island","Honduras","Hong Kong","Hungary","Icland","India","Indonesia","Iran (Islamic REpublic of)","Iraq","Ireland","Israel","Italy","Ivory Coast","Jamaica","Japan","Jordan","Kazakhstan","Kenya","Kiribati","Korea, Republic of","Kosovo","Kuwait","Kyrgyzstan","Lao Peoplet's Democratic Republic","Latvia","Lebanon","Lesotho","Liberia","Libyan Arab Jamahiriya","Liechtenstein","Lithuainia","Macau","Macedonia","Madagascar","Malawi","Malaysia","Maldives","Mali","Malta","Marshall Island","Martinique","Mauritania","Mauritius","MAyotte","Mexico","Micronesia, Federated State of","Moldova, Republic of","Monaco","Mongolia","Montenergro","Montenegra","Montserrat","Morocco","Mozambique","Mayanmar","Namibia","Nauru","Nepal","Netherlands","Netherlands Antilles"," New Caledonia","New Zealand","Nicaragua","Niger","Nigeria","Niue","Norfork Island","Northern Mariana ISlands","Norway","Oman","Pakistan","Palau","Panama","Papua New Guinea","Paraguay","Peru","Philippines","Pitcairn","Poland","Portugal","Puerto Rico","Qatar","Reunoin","Romania","Russian Fedration","Rwanda","Saint Kitts and Nevis","Saint Lucia","Saint Vincent and the Grenadines","Samoa","San Marino","Sao Tome and Principe","Saudi Arabia","Senegal","Serbia","Seychelles","Sierra Leone","Singapore","Slovakia","Slovenia","Solomon Island","Somalia","South Africa","South Gerogia South Sandwich Island","Spain","Srilanka","St. Helena","St. Pierre and Miquelon","Sudan","Suriname","Svalbarn and Jan Mayen Islands","Swaziland","Swedan","Switzerland","Syrian Arab Republic","Tiawan","Tajikistan","Tanzania, United Republic of","Thailand","Togo","Tokelau","Tonga","Trinidad and Tobago","Tunisia","Turkey","Turkmenistan","Turks and Caicos Islands","Tuvalu","Uganda","Ukraine","United Arab Emirates","United Kingdom","United States minor outlying islands","Uruguay","Uzbekistan","Vanuatu","Vatican City State","Venezuela","Vietnam","Yemen","Yugoslavia","Zaire","Zambia","Zimbabwe"]
    
    var searchList = [String]()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.searchBar.delegate = self
        self.setUPSearchField()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.tableView.reloadData()
    }
    
    
    
    @IBAction func CAncel(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
    private func setUPSearchField(){
        if let textfield = self.searchBar.value(forKey: "searchField") as? UITextField {
            textfield.clearButtonMode = .never
            textfield.backgroundColor = .clear
            //                UIColor.hexStringToUIColor(hex: "#984243")
            textfield.attributedPlaceholder = NSAttributedString(string:" Search...", attributes:[NSAttributedString.Key.foregroundColor: UIColor.yellow])
            textfield.textColor = .white
            if let leftView = textfield.leftView as? UIImageView {
                leftView.image = leftView.image?.withRenderingMode(.alwaysTemplate)
                leftView.tintColor = UIColor.white
            }
        }
    }
    
    
}
extension SearchLocationController : UITableViewDataSource,UITableViewDelegate{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if searching{
            return self.searchList.count
        }
        else {
            return self.countryList.count
        }
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = UITableViewCell()
        cell.textLabel?.textColor = UIColor.black
        cell.backgroundColor = UIColor.white
        cell.textLabel?.numberOfLines = 0
        if self.searching  {
            cell.textLabel?.text = self.searchList[indexPath.row]
            return cell
        }
        else {
            cell.textLabel?.text = self.countryList[indexPath.row]
            return cell
        }
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        if searching{
            self.delegate.locationSearch(location: self.searchList[indexPath.row], countryId: "\(indexPath.row)")
            self.dismiss(animated: true, completion: nil)
        }
        else {
            self.delegate.locationSearch(location: self.countryList[indexPath.row], countryId: "\(indexPath.row)")
        self.dismiss(animated: true, completion: nil)
        }
    }
    
        func searchBar(_ searchBar: UISearchBar, textDidChange searchText: String) {
            self.searchList = self.countryList.filter({$0.prefix(searchText.count) == searchText})
            print(self.searchList)
            self.searching = true
            self.tableView.reloadData()
        }
        
        func searchBarSearchButtonClicked(_ searchBar: UISearchBar) {
            self.searchBar.resignFirstResponder()
        }
}
