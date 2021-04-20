
import UIKit

public class LoadingOverlay {
    
    var overlayView = UIView()
    var activityIndicator = UIActivityIndicatorView()
    var labelLoading = UILabel(frame: CGRect(x: UIScreen.main.bounds.midX, y: UIScreen.main.bounds.midY, width: 100, height: 50))
    var isVisible = false
    
    public class var shared: LoadingOverlay {
        struct Static {
            static let instance: LoadingOverlay = LoadingOverlay()
        }
        return Static.instance
    }
    
    public func showOverlay(view: UIView!) {
        if isVisible {
            return
        }
        isVisible = true
        overlayView = UIView(frame: UIScreen.main.bounds)
        overlayView.backgroundColor = UIColor(red: 0, green: 0, blue: 0, alpha: 0.2)
        let blurEffect = UIBlurEffect(style: .regular)
        let blurredEffectView = UIVisualEffectView(effect: blurEffect)
        blurredEffectView.frame = CGRect(x: UIScreen.main.bounds.midX - 125, y: UIScreen.main.bounds.midY - 40, width: 250, height: 80)
        blurredEffectView.layer.cornerRadius = 15
        blurredEffectView.layer.masksToBounds = true
        overlayView.addSubview(blurredEffectView)
        if #available(iOS 13.0, *) {
            activityIndicator = UIActivityIndicatorView(style: UIActivityIndicatorView.Style.medium)
        } else {
            activityIndicator = UIActivityIndicatorView(style: UIActivityIndicatorView.Style.white)
        }
        activityIndicator.center = CGPoint.init(x: overlayView.center.x - 50, y: overlayView.center.y)
        overlayView.addSubview(activityIndicator)
        activityIndicator.startAnimating()
        labelLoading.text = "Loading..."
        labelLoading.font = UIFont.boldSystemFont(ofSize: 16.0)
        labelLoading.center = CGPoint.init(x: overlayView.center.x + 25, y: overlayView.center.y)
        overlayView.addSubview(labelLoading)
        
        //self.overlayView.backgroundColor.alpha = 0
        view.addSubview(overlayView)
        /*UIView.animate(withDuration: 1) {
            self.overlayView.alpha = 0.75
        }*/
    }
    
    public func hideOverlayView() {
        isVisible = false
        self.activityIndicator.stopAnimating()
        self.overlayView.removeFromSuperview()
    }
}
