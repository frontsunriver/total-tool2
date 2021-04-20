
import UIKit
import WoWonderTimelineSDK


class SetUpcells {
    
    static func setupCells(tableView: UITableView){

        tableView.register(UINib(nibName: "NewsFeedCell", bundle: nil), forCellReuseIdentifier: "NewsFeedCell")
        tableView.register(UINib(nibName: "MusicCell", bundle: nil),
                           forCellReuseIdentifier: "musicCell")
        tableView.register(UINib(nibName: "PostWithLinkCell", bundle: nil), forCellReuseIdentifier: "PostLinkCell")

        tableView.register(UINib(nibName: "PostYoutubeCell", bundle: nil), forCellReuseIdentifier: "PostYoutube")
        tableView.register(UINib(nibName: "GifImageCell", bundle: nil), forCellReuseIdentifier: "GifCell")
        tableView.register(UINib(nibName: "BlogCell", bundle: nil), forCellReuseIdentifier: "BlogCell")
        tableView.register(UINib(nibName: "GroupCell", bundle: nil), forCellReuseIdentifier: "GroupCell")
        tableView.register(UINib(nibName: "ProductCell", bundle: nil), forCellReuseIdentifier: "ProductCell")
        tableView.register(UINib(nibName: "NormalPostCell", bundle: nil), forCellReuseIdentifier: "NormalPostCell")
        tableView.register(UINib(nibName: "EventCell", bundle: nil), forCellReuseIdentifier: "EventCell")
        tableView.register(UINib(nibName: "PostwithBg_imageCell", bundle: nil), forCellReuseIdentifier: "postWithBg_image")
        tableView.register(UINib(nibName: "VideoCell", bundle: nil), forCellReuseIdentifier: "VideoCell")
        tableView.register(UINib(nibName: "MultiImage3", bundle: nil), forCellReuseIdentifier: "MultiImage3")
        tableView.register(UINib(nibName: "MultiImage2", bundle: nil), forCellReuseIdentifier: "MultiImage2")
        tableView.register(UINib(nibName: "PhotoAlbumCell", bundle: nil), forCellReuseIdentifier: "PhotoAlbumCell")
        tableView.register(UINib(nibName: "PhotoAlbum2", bundle: nil), forCellReuseIdentifier: "PhotoAlbum2")
        tableView.register(UINib(nibName: "PhotoAlbum3", bundle: nil), forCellReuseIdentifier: "PhotoAlbum3")
        tableView.register(UINib(nibName: "PostOptionCell", bundle: nil), forCellReuseIdentifier: "PostOptions")
        tableView.register(UINib(nibName: "PostPDFCell", bundle: nil), forCellReuseIdentifier: "PostPDFCell")
        tableView.register(UINib(nibName: "PostShareCell", bundle: nil), forCellReuseIdentifier: "PostShare")
        tableView.register(UINib(nibName: "DonationPostCell", bundle: nil), forCellReuseIdentifier: "DonationPost")
        tableView.register(UINib(nibName: "AddPostCell", bundle: nil), forCellReuseIdentifier: "AddPostCells")

    }
    
    
}
