//
//  FavoriteAddressDialogViewController.swift
//  rider
//
//  Copyright © 1397 Minimalistic Apps. All rights reserved.
//

import UIKit
import MapKit

class FavoriteAddressDialogViewController: UIViewController {
    @IBOutlet weak var textTitle: UITextField!
    @IBOutlet weak var textAddress: UITextField!
    var address: Address?
    @IBOutlet weak var map: MKMapView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        if address != nil {
            textTitle.text = address?.title
            textAddress.text = address?.address
            map.setCenter((address?.location)!, animated: true)
        } else {
            let locationManager = CLLocationManager()
            if let location = locationManager.location {
                map.setCenter(location.coordinate, animated: true)
            }
        }

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */
    
    

}
