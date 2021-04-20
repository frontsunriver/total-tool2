

import UIKit
import Toast_Swift
import Kingfisher
import WoWonderTimelineSDK
class BlogController: UIViewController,BlogCategoryDelegate,FilterBlockUser{

    @IBOutlet weak var tableView: UITableView!
    @IBOutlet weak var noArticles: UIView!
    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var filterBtn: RoundButton!
    
    
    let Storyboard = UIStoryboard(name: "Poke-MyVideos-Albums", bundle: nil)
    let status = Reach().connectionStatus()
    let spinner = UIActivityIndicatorView(style: .gray)
    let pulltoRefresh = UIRefreshControl()

    var blogs = [[String:Any]]()
    var categoryId = 0
    var offset: String? = nil
    
    override func viewDidLoad() {
        super.viewDidLoad()
    navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        self.navigationItem.largeTitleDisplayMode = .never
        self.noArticles.isHidden = true
        self.filterBtn.isHidden = true
        self.activityIndicator.startAnimating()
     NotificationCenter.default.post(name: Notification.Name(ReachabilityStatusChangedNotification), object: nil)
    let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
    navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.title = NSLocalizedString("Explore Article", comment: "Explore Article")
        self.navigationItem.rightBarButtonItem = UIBarButtonItem(title: NSLocalizedString("Create", comment: "Create"), style: .done, target: self, action: #selector(self.Create(sender:)))
        self.navigationItem.largeTitleDisplayMode = .never
        self.tableView.tableFooterView = UIView()
        self.pulltoRefresh.tintColor = UIColor.hexStringToUIColor(hex: "#984243")
        self.pulltoRefresh.addTarget(self, action: #selector(self.refresh), for: .valueChanged)
        self.tableView.addSubview(pulltoRefresh)
        self.getBlogs(categoryId: self.categoryId)
    }
    
    //Pull To Refresh
    @objc func refresh(){
        self.offset = "0"
        self.spinner.stopAnimating()
        self.blogs.removeAll()
        self.tableView.reloadData()
        self.getBlogs(categoryId: self.categoryId)
    }
    
    @IBAction func Create(sender:UIBarButtonItem){
        let vc = Storyboard.instantiateViewController(withIdentifier: "CreateBlogVC") as! CreateBlogController
        self.navigationController?.pushViewController(vc, animated: true)
    }

    
    
    @IBAction func Filter(_ sender: Any) {
    let vc = Storyboard.instantiateViewController(withIdentifier: "BlogCategoryVC") as! BlogCategoryController
        vc.modalPresentationStyle = .overCurrentContext
        vc.modalTransitionStyle = .crossDissolve
        vc.delegate = self
        self.present(vc, animated: true, completion: nil)
    }
    
    private func getBlogs(categoryId : Int){
        switch status {
        case .unknown, .offline:
            self.view.makeToast(NSLocalizedString("Internet Connection Failed", comment: "Internet Connection Failed"))

        case .online(.wwan),.online(.wiFi):
            GetBlogManager.sharedInstance.getBlog(categoryId: categoryId, offset: self.offset ?? "") { (success, authError, error) in
                if success != nil {
                    for i in success!.articles{
                        self.blogs.append(i)
                    }
                    self.offset = self.blogs.last?["id"] as? String ?? "0"
                    print(self.offset)
                    self.activityIndicator.stopAnimating()
                    self.pulltoRefresh.endRefreshing()
                    self.tableView.reloadData()
                    if self.blogs.count == 0 {
                        self.noArticles.isHidden = false
                    }
                    else {
                        self.noArticles.isHidden = true
                        self.filterBtn.isHidden = false
                    }
                }
                else if authError != nil {
                    self.activityIndicator.stopAnimating()
                    self.view.makeToast(authError?.errors.errorText)
                }
                else if error != nil {
                    print(error?.localizedDescription)
                }
            }
        }
        
    }
    
}
extension BlogController :UITableViewDelegate,UITableViewDataSource{
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return blogs.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell =  tableView.dequeueReusableCell(withIdentifier: "Blog") as! ArticleCell
        let index = self.blogs[indexPath.row]
        cell.gotoProfile.tag = indexPath.row
        cell.gotoProfile.addTarget(self, action: #selector(self.GotoProfile(sender:)), for: .touchUpInside)
        if let thumbnail = index["thumbnail"] as? String{
            let url = URL(string: thumbnail)
            cell.thumbnailImage.kf.setImage(with: url)
        }
        if let title = index["title"] as? String{
            cell.BlogTitle.text! = title
        }
        if let descrip = index["description"] as? String{
            cell.articleDescrip.text! = descrip
        }
        if let time = index["posted"] as? String{
            cell.postTime.text! = time
        }
        if let category = index["category"] as? String{
            cell.categoryLabel.text = "\(category)\("  ")"
        }
        if let author = index["author"] as? [String:Any]{
            if let profileIcon = author["avatar"] as? String {
                let url = URL(string: profileIcon)
                cell.profileIcon.kf.setImage(with: url)
            }
            if let name = author["name"] as? String{
                cell.userName.text! = name
            }
        }
        
        return cell
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return 143.0
    }
    
    func tableView(_ tableView: UITableView, willDisplay cell: UITableViewCell, forRowAt indexPath: IndexPath) {
        if self.blogs.count >= 10 {
            let count = self.blogs.count
            let lastElement = count - 1
            
            if indexPath.row == lastElement {
                spinner.startAnimating()
                spinner.frame = CGRect(x: CGFloat(0), y: CGFloat(0), width: tableView.bounds.width, height: CGFloat(44))
                self.tableView.tableFooterView = spinner
                self.tableView.tableFooterView?.isHidden = false
                self.getBlogs(categoryId: self.categoryId)
            }
        }
    }

    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
    let vc = Storyboard.instantiateViewController(withIdentifier: "BlogDetailsVC") as! BlogDetails
    let index = self.blogs[indexPath.row]
    vc.blogDetails.append(index)
        vc.modalPresentationStyle = .fullScreen
        vc.modalTransitionStyle = .coverVertical
       self.present(vc, animated: true, completion: nil)
//    self.navigationController?.pushViewController(vc, animated: true)
    }
    
    func blogCategory(categoryName: String, categoryId: Int) {
        self.blogs.removeAll()
        self.tableView.reloadData()
        self.filterBtn.isHidden = true
        self.activityIndicator.startAnimating()
        self.getBlogs(categoryId: categoryId)
    }
    
    func filterBlockUser(userId: String) {
        self.blogs = self.blogs.filter({$0["user_id"] as? String != userId})
        self.tableView.reloadData()
     }
     
    
    @IBAction func GotoProfile(sender :UIButton){
//    var user_name = "",profile_Image = "",follower = "0",following = "0",likes = "0",group = "0",user_id = "0",coverImage = "",followPrivacy = "",messagePrivacy = "", friendPrivacy = "",post_privacy = "",points = ""
//
//        let index = self.blogs[sender.tag]
//        if let author = index["author"]  as? [String:Any]{
//            if let userName = author["name"] as? String{
//                   user_name = userName
//            }
//            if let userId = author["user_id"] as? String{
//                user_id = userId
//            }
//            if let profileImage = author["avatar"] as? String{
//                profile_Image = profileImage
//            }
//            if let cover = author["cover"] as? String{
//                coverImage = cover
//            }
//            if let point = author["points"] as? String{
//                points = point
//            }
//            if let details = author["details"] as? [String:Any] {
//                if let followersCount = details["followers_count"] as? String {
//                    follower = followersCount
//                }
//                if let followingCount = details["following_count"] as? String {
//                    following = followingCount
//                }
//                if let likesCount = details["likes_count"] as? String {
//                    likes = likesCount
//                }
//                if let groupCount = details["groups_count"] as? String{
//                    group = groupCount
//                }
//
//              }
//
//            }
//        let profileData = UserProfileData(username: user_name, profileImage: profile_Image, follower: follower, following: following, likes: likes, group: group, user_id: user_id, coverImage: coverImage, followPrivacy: followPrivacy, messagePrivacy: messagePrivacy, friendPrivacy: friendPrivacy, post_privacy: post_privacy, points: points)
        let storyBoard = UIStoryboard(name: "Main", bundle: nil)
        let vc = storyBoard.instantiateViewController(withIdentifier: "UserProfile") as! GetUserDataController
        if let userData = self.blogs[sender.tag]["author"] as? [String:Any]{
            vc.userData = userData
        }
        vc.delegate = self
        self.navigationController?.pushViewController(vc, animated: true)
        }
    }
    
    
    

