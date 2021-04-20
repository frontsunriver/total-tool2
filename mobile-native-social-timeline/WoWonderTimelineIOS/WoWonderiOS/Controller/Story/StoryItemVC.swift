

import UIKit
import WoWonderTimelineSDK


var PPStoriesItemsViewControllerVC = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "StoryItemVC") as! StoryItemVC

class StoryItemVC: UIViewController {
    
    var pageViewController : UIPageViewController?
    var pages: [GetStoriesModel.UserDataElement] = []
    var currentIndex : Int = 0
    
    var refreshStories: (() -> Void)?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        // Do any additional setup after loading the view.
        
        pageViewController = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "PageViewController") as? UIPageViewController
        pageViewController!.dataSource = self
        pageViewController!.delegate = self
        
        
        let startingViewController: PreStoriesItemVC = viewControllerAtIndex(index: currentIndex)!
        let viewControllers = [startingViewController]
        pageViewController!.setViewControllers(viewControllers , direction: .forward, animated: false, completion: nil)
        pageViewController!.view.frame = view.bounds
        
        startingViewController.refreshStories = {
            self.refreshStories!()
        }
        
        addChild(pageViewController!)
        view.addSubview(pageViewController!.view)
        view.sendSubviewToBack(pageViewController!.view)
        pageViewController!.didMove(toParent: self)
        
        print(self.pages)
    }
    
    func viewControllerAtIndex(index: Int) -> PreStoriesItemVC? {
        
        if pages.count == 0 || index >= pages.count {
            return nil
        }
        
        // Create a new view controller and pass suitable data.
        let vc = UIStoryboard(name: "Stories", bundle: nil).instantiateViewController(withIdentifier: "PreStoriesItemVC") as! PreStoriesItemVC
        vc.refreshStories =  {
            self.refreshStories!()
        }
        vc.pageIndex = index
        vc.items = self.pages
        currentIndex = index
        
        //vc.view.transform = CGAffineTransform(scaleX: 1.4, y: 1.4)
        return vc
    }
    
    func goNextPage(fowardTo position: Int) {
        print("Position : \(position)")
        print("Item Count : \(self.pages.count)")
        let startingViewController: PreStoriesItemVC = viewControllerAtIndex(index: position)!
        let viewControllers = [startingViewController]
        pageViewController!.setViewControllers(viewControllers , direction: .forward, animated: true, completion: nil)
    }
    
    
}


extension StoryItemVC : UIPageViewControllerDataSource, UIPageViewControllerDelegate  {
    
    func pageViewController(_ pageViewController: UIPageViewController, viewControllerBefore viewController: UIViewController) -> UIViewController? {
        var index = (viewController as! PreStoriesItemVC).pageIndex
        if (index == 0) || (index == NSNotFound) {
            return nil
        }
        index -= 1
        return viewControllerAtIndex(index: index)
    }
    
    func pageViewController(_ pageViewController: UIPageViewController, viewControllerAfter viewController: UIViewController) -> UIViewController? {
        var index = (viewController as! PreStoriesItemVC).pageIndex
        if index == NSNotFound {
            return nil
        }
        index += 1
        if (index == pages.count) {
            return nil
        }
        return viewControllerAtIndex(index: index)
    }
    
    
}
