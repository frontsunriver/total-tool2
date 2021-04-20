//
//  ImageSliderController.swift
//  News_Feed
//
//  Created by clines329 on 11/10/19.
//  Copyright © 2019 clines329. All rights reserved.
//

import UIKit

class ImageSliderController: UIViewController,UIScrollViewDelegate {
    
    
    
    
    @IBOutlet weak var pageController: UIPageControl!
    
    
    @IBOutlet weak var nextImageBtn: UIButton!
    
    
    @IBOutlet weak var skipBtn: UIButton!
    @IBOutlet weak var scrollView: UIScrollView!{
        
        didSet  {
            scrollView.delegate = self
            
            
            
        }
        
    }
    
    
    var imageSlides:[IntroImageSlider] = [];
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.scrollView.alwaysBounceHorizontal = false
        self.scrollView.alwaysBounceVertical = false
        self.scrollView.scrollsToTop = false
        navigationController?.interactivePopGestureRecognizer?.isEnabled = false
        
        
        
        imageSlides = createSlides()
        setupSlideScrollView(slides: imageSlides)
        self.scrollView.bounces = false
        self.scrollView.bouncesZoom = false
        pageController.numberOfPages = imageSlides.count
        pageController.currentPage = 0
        view.bringSubviewToFront(pageController)
        
        
    }
    
    override func viewWillAppear(_ animated: Bool) {
        self.navigationController?.navigationBar.isHidden = true
    }
    
    
    
    func createSlides() -> [IntroImageSlider] {
        
        let slide1:IntroImageSlider = Bundle.main.loadNibNamed("IntroImageSlider", owner: self, options: nil)?.first as! IntroImageSlider
        slide1.sliderImage.image = UIImage(named: "ic_rocket")
        slide1.textLabel.text = "New Version"
        slide1.descriptionLabel.text = "Fast so we can take you to our space"
        slide1.backgroundColor = UIColor.hexStringToUIColor(hex: "2C4154")
        let slide2:IntroImageSlider = Bundle.main.loadNibNamed("IntroImageSlider", owner: self, options: nil)?.first as! IntroImageSlider
        slide2.sliderImage.image = UIImage(named: "ic_magnifying_glass")
        slide2.textLabel.text = "Search Globally"
        slide2.descriptionLabel.text = "Find new friends and contacts"
        slide2.backgroundColor = UIColor.hexStringToUIColor(hex: "FCB741")
        
        
        let slide3:IntroImageSlider = Bundle.main.loadNibNamed("IntroImageSlider", owner: self, options: nil)?.first as! IntroImageSlider
        slide3.sliderImage.image = UIImage(named: "ic_paper_plane")
        slide3.textLabel.text = "New Features"
        slide3.descriptionLabel.text = "Send & Recieve all kind of messages"
        slide3.backgroundColor = UIColor.hexStringToUIColor(hex: "2385C2")
        
        let slide4:IntroImageSlider = Bundle.main.loadNibNamed("IntroImageSlider", owner: self, options: nil)?.first as! IntroImageSlider
        slide4.sliderImage.image = UIImage(named: "ic_chat_violet")
        slide4.textLabel.text = "Stay Sync"
        slide4.descriptionLabel.text = "Keep you conversation going from all devices"
        slide4.backgroundColor = UIColor.hexStringToUIColor(hex: "8E43AC")
        
        
        return [slide1, slide2, slide3, slide4]
    }
    
    
    
    func setupSlideScrollView(slides : [IntroImageSlider]) {
        scrollView.frame = CGRect(x: 0, y: 0, width: view.frame.width, height: view.frame.height)
        scrollView.contentSize = CGSize(width: view.frame.width * CGFloat(slides.count), height: view.frame.height)
        scrollView.isPagingEnabled = true
        
        for i in 0 ..< slides.count {
            slides[i].frame = CGRect(x: view.frame.width * CGFloat(i), y: 0, width: view.frame.width, height: view.frame.height)
            scrollView.addSubview(slides[i])
        }
    }
    
    func scrollViewDidScroll(_ scrollView: UIScrollView) {
        let pageIndex = round(scrollView.contentOffset.x/view.frame.width)
        pageController.currentPage = Int(pageIndex)
        if pageController.currentPage == 3 {
            self.skipBtn.isHidden = true
            self.nextImageBtn.setImage(UIImage(named: "verified"), for: .normal)
            
        }else {
            self.skipBtn.isHidden = false
            self.nextImageBtn.setImage(UIImage(named: "arrow-pointing"), for: .normal)
            self.nextImageBtn.setTitle(nil, for: .normal)
        }
        
    }
    
    
    
    func scrollToNextSlide(){
        let cellSize = CGSize(width: self.view.frame.width, height: self.view.frame.height)
        let contentOffset = scrollView.contentOffset;
        
        scrollView.scrollRectToVisible(CGRect(x: contentOffset.x + cellSize.width, y: contentOffset.y, width: cellSize.width, height: cellSize.height), animated: true);
        
    }
    
    
    
    func scrollView(_ scrollView: UIScrollView, didScrollToPercentageOffset percentageHorizontalOffset: CGFloat) {
        if(pageController.currentPage == 0) {
            //Change background color to toRed: 103/255, fromGreen: 58/255, fromBlue: 183/255, fromAlpha: 1
            //Change pageControl selected color to toRed: 103/255, toGreen: 58/255, toBlue: 183/255, fromAlpha: 0.2
            //Change pageControl unselected color to toRed: 255/255, toGreen: 255/255, toBlue: 255/255, fromAlpha: 1
            
            let pageUnselectedColor: UIColor = fade(fromRed: 255/255, fromGreen: 255/255, fromBlue: 255/255, fromAlpha: 1, toRed: 103/255, toGreen: 58/255, toBlue: 183/255, toAlpha: 1, withPercentage: percentageHorizontalOffset * 3)
            pageController.pageIndicatorTintColor = pageUnselectedColor
            
            
            let bgColor: UIColor = fade(fromRed: 103/255, fromGreen: 58/255, fromBlue: 183/255, fromAlpha: 1, toRed: 255/255, toGreen: 255/255, toBlue: 255/255, toAlpha: 1, withPercentage: percentageHorizontalOffset * 3)
            imageSlides[pageController.currentPage].backgroundColor = bgColor
            
            let pageSelectedColor: UIColor = fade(fromRed: 81/255, fromGreen: 36/255, fromBlue: 152/255, fromAlpha: 1, toRed: 103/255, toGreen: 58/255, toBlue: 183/255, toAlpha: 1, withPercentage: percentageHorizontalOffset * 3)
            pageController.currentPageIndicatorTintColor = pageSelectedColor
        }
    }
    
    func fade(fromRed: CGFloat,fromGreen: CGFloat,fromBlue: CGFloat,fromAlpha: CGFloat,toRed: CGFloat,
              toGreen: CGFloat,toBlue: CGFloat,toAlpha: CGFloat,withPercentage percentage: CGFloat) -> UIColor {
        
        let red: CGFloat = (toRed - fromRed) * percentage + fromRed
        let green: CGFloat = (toGreen - fromGreen) * percentage + fromGreen
        let blue: CGFloat = (toBlue - fromBlue) * percentage + fromBlue
        let alpha: CGFloat = (toAlpha - fromAlpha) * percentage + fromAlpha
        
        // return the fade colour
        return UIColor(red: red, green: green, blue: blue, alpha: alpha)
        
        
    }
     
    
    @IBAction func Next(_ sender: Any) {
        if pageController.currentPage == 3 {
            let storyboard = UIStoryboard(name: "Main", bundle: nil)
            let myVC = storyboard.instantiateViewController(withIdentifier: "News_FeedVC") as! UINavigationController
            self.present(myVC, animated: true, completion: nil)
        }
        else {
            self.scrollToNextSlide()
        }
    }
    
    
    @IBAction func Skip(_ sender: Any) {
        let storyboard = UIStoryboard(name: "Main", bundle: nil)
        
        let myVC = storyboard.instantiateViewController(withIdentifier: "News_FeedVC") as! UINavigationController
        self.present(myVC, animated: true, completion: nil)
    }
    
    
    
}
