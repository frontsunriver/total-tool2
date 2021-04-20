
import UIKit
import WoWonderTimelineSDK


class SalaryController: UIViewController {

    @IBOutlet weak var salaryDate: UILabel!
    
    @IBOutlet weak var closeBtn: UIButton!
    
    var delegate : JobSalaryDelegate!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.salaryDate.text = NSLocalizedString("Salary Date", comment: "Salary Date")
        self.closeBtn.setTitle(NSLocalizedString("Close", comment: "Close"), for: .normal)
    }
    
    
    @IBAction func SalaryDate(_ sender: UIButton) {
        switch sender.tag{
        case 0:
            self.delegate.salaryData(salaryDate: "Per Hour", salaryType: "per_hour")
            self.dismiss(animated: true, completion: nil)
        case 1:
            self.delegate.salaryData(salaryDate: "Per Day", salaryType: "per_day")
            self.dismiss(animated: true, completion: nil)
        case 2:
            self.delegate.salaryData(salaryDate: "Per Week", salaryType: "per_week")
            self.dismiss(animated: true, completion: nil)
        case 3:
            self.delegate.salaryData(salaryDate: "Per Month", salaryType: "per_month")
            self.dismiss(animated: true, completion: nil)
        case 4:
            self.delegate.salaryData(salaryDate: "Per Year", salaryType: "per_year")
            self.dismiss(animated: true, completion: nil)
        default:
            print("Default")
        }
        
    }
    


    @IBAction func Close(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }
    
}
