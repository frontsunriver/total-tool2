import WoWonderTimelineSDK
import UIKit
import FittedSheets
import ZKProgressHUD
import MediaPlayer
import ActionSheetPicker_3_0
class AddPostVC: UIViewController {
    
    @IBOutlet weak var tableView: UITableView!
    
    var postPrivacy:Int? = 0
    var PostColor:String? =  "0"
    var postText:String? = ""
    var type:String? = ""
    var musicImageType:String? = ""
    var pathString:String = ""
    var musicData:Data?
    var fileData:Data?
    var fileExtension:String? = ""
    var feelingType:String? = ""
    var feelingName:String? = ""
    var postType:String? = ""
    var pageid:String? = ""
    var groupId:String? = ""
    var eventId:String? = ""
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        
        self.navigationController?.navigationBar.topItem?.backBarButtonItem = UIBarButtonItem(title: "", style: .plain, target: self, action: nil)
        
        let textAttributes = [NSAttributedString.Key.foregroundColor:UIColor.white]
        navigationController?.navigationBar.titleTextAttributes = textAttributes
        self.navigationItem.largeTitleDisplayMode = .never
        self.navigationItem.title = NSLocalizedString("Add Post", comment: "Add Post")
        
        self.setupUI()
        
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        self.tabBarController?.tabBar.isHidden = true
        AppInstance.instance.isAlbumVisible = false
        AppInstance.instance.musicSelected = false
    }
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        self.tabBarController?.tabBar.isHidden = false
    }
    
    @IBAction func feelingPressed(_ sender: Any) {
        let vc =  UIStoryboard(name: "AddPost", bundle: nil).instantiateViewController(withIdentifier: "PostTypeVC") as? PostTypeVC
              vc?.delegate = self
              
              let controller = SheetViewController(controller:vc!)
              
              controller.blurBottomSafeArea = false
              self.present(controller, animated: false, completion: nil)
    }
    @IBAction func personClicked(_ sender: Any) {
        let vc =  UIStoryboard(name: "AddPost", bundle: nil).instantiateViewController(withIdentifier: "PostTypeVC") as? PostTypeVC
              vc?.delegate = self
              
              let controller = SheetViewController(controller:vc!)
              
              controller.blurBottomSafeArea = false
              self.present(controller, animated: false, completion: nil)
    }
    @IBAction func picturePressed(_ sender: Any) {
        let vc =  UIStoryboard(name: "AddPost", bundle: nil).instantiateViewController(withIdentifier: "PostTypeVC") as? PostTypeVC
              vc?.delegate = self
              
              let controller = SheetViewController(controller:vc!)
              
              controller.blurBottomSafeArea = false
              self.present(controller, animated: false, completion: nil)
    }
    @IBAction func postTypePressed(_ sender: Any) {
        
        let vc =  UIStoryboard(name: "AddPost", bundle: nil).instantiateViewController(withIdentifier: "PostTypeVC") as? PostTypeVC
        vc?.delegate = self
        
        let controller = SheetViewController(controller:vc!)
        
        controller.blurBottomSafeArea = false
        self.present(controller, animated: false, completion: nil)
    }
    private func setupUI(){
        let save = UIBarButtonItem(title: NSLocalizedString("Save", comment: "Save"), style: .done, target: self, action: #selector(Save))
        self.navigationItem.rightBarButtonItem  = save
        self.tableView.separatorStyle = .none
        tableView.register(UINib(nibName: "AddPostSectionOneTableItem", bundle: nil), forCellReuseIdentifier: "AddPostSectionOneTableItem")
        tableView.register(UINib(nibName: "AddPostSectionTwoTableItem", bundle: nil), forCellReuseIdentifier: "AddPostSectionTwoTableItem")
        tableView.register(UINib(nibName: "AddPostSectionThreeTableItem", bundle: nil), forCellReuseIdentifier: "AddPostSectionThreeTableItem")
        tableView.register(UINib(nibName: "AddPostSectionFourTableItem", bundle: nil), forCellReuseIdentifier: "AddPostSectionFourTableItem")
        
//        print("settings adata = \(AppInstance.instance.siteSettings?.config?.postColors)")
        
        
    }
    @objc func Save(){
        let indexpathforTextView = IndexPath(row: 0, section: 1)
        let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionTwoTableItem
        self.postText  = cell.textView.text ?? ""
        if self.type == "IMAGE"{
            if AppInstance.instance.musicSelected{
                let indexpathforTextView = IndexPath(row: 0, section: 3)
                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
                var imagesDataArray  = [Data]()
                cell.imageArray.forEach { (it) in
                    let data = it.jpegData(compressionQuality: 0.1)
                    imagesDataArray.append(data!)
                    self.uploadImages(imageArray: imagesDataArray)
                }
            }else{
                let indexpathforTextView = IndexPath(row: 0, section: 2)
                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
                var imagesDataArray  = [Data]()
                cell.imageArray.forEach { (it) in
                    let data = it.jpegData(compressionQuality: 0.1)
                    imagesDataArray.append(data!)
                    self.uploadImages(imageArray: imagesDataArray)
                }
            }
            
        }else if self.type == "VIDEO"{
            if AppInstance.instance.musicSelected{
                let indexpathforTextView = IndexPath(row: 0, section: 3)
                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
                self.uploadVideo(videoData: cell.VideoData!)
                
            }else{
                let indexpathforTextView = IndexPath(row: 0, section: 2)
                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
                self.uploadVideo(videoData: cell.VideoData!)
            }
            
        }else if self.type == "GIF"{
            if AppInstance.instance.musicSelected{
                let indexpathforTextView = IndexPath(row: 0, section: 3)
                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
                self.postGIF(GIFURL: cell.gifURLString ?? "")
                
            }else{
                let indexpathforTextView = IndexPath(row: 0, section: 2)
                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
                self.postGIF(GIFURL: cell.gifURLString ?? "")
            }
            
        }else if self.type == "MUSIC"{
            
            self.uploadMusic(musicData: self.musicData ?? Data())
            
        }else if self.type == "FILE"{
            
            self.uploadFile(fileData: self.fileData ?? Data(), extension1: self.fileExtension ?? "")
            
        }else if self.type == "FEELING"{
            
            self.updateFeeling(feelingType: self.feelingType ?? "", feelingName: self.feelingName ?? "",postText:self.postText ?? "",postPrivacy:self.postPrivacy ?? 0 ,postColor:self.PostColor ?? "")
            
        }else{
            if self.postText == "" {
                self.view.makeToast("You cannot post empty!")
            }else if self.PostColor != "0" && self.postText == ""{
                self.view.makeToast("You cannot post empty!")
            }else{
                self.updatePost(postText: self.postText ?? "", postPrivacy: self.postPrivacy ?? 0, postColor: self.PostColor ?? "0")
            }
        }
        print("test = \(self.postText ?? "")")
        print("post Privacy = \(self.postPrivacy ?? 0)")
        
        print("test = \(self.postText ?? "")")
    }
    private func openCamera(){
        self.type = "IMAGE"
        let imagePickerController = UIImagePickerController()
                                                  
                                                  imagePickerController.delegate = self
                                                  
                                                  imagePickerController.allowsEditing = true
                                                  imagePickerController.sourceType = .camera
                                                  self.present(imagePickerController, animated: true, completion: nil)
    }
    private func openMusicLibrary(){
        let pickerController = MPMediaPickerController(mediaTypes: .music)
        pickerController.delegate = self
        present(pickerController, animated: true)
        
    }
    func openiCloudDocuments(){
        let importMenu = UIDocumentPickerViewController(documentTypes: [String("public.data")], in: .import)
        importMenu.delegate = self
        importMenu.modalPresentationStyle = .formSheet
        self.present(importMenu, animated: true, completion: nil)
    }
    private func showGif(){
        let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
        let vc = storyboard.instantiateViewController(withIdentifier: "SelectGIFVC") as! SelectGIFVC
        vc.delegate = self
    //    self.navigationController?.pushViewController(vc, animated: true)
        self.present(vc, animated: true, completion: nil)
    }
    func openImageController(){
        self.type = "IMAGE"
        ActionSheetStringPicker.show(withTitle: NSLocalizedString("Source", comment: ""),
                                     rows: [NSLocalizedString("Gallery", comment: ""),
                                            NSLocalizedString("Camera", comment: "")
            ],
                                     initialSelection: 0,
                                     doneBlock: { (picker, value, index) in
                                        
                                        if value == 0 {
                                            let imagePickerController = UIImagePickerController()
                                            
                                            imagePickerController.delegate = self
                                            
                                            imagePickerController.allowsEditing = true
                                            imagePickerController.sourceType = .photoLibrary
                                            self.present(imagePickerController, animated: true, completion: nil)
                                            
                                            
                                        }else if value == 1 {
                                            let imagePickerController = UIImagePickerController()
                                            
                                            imagePickerController.delegate = self
                                            
                                            imagePickerController.allowsEditing = true
                                            imagePickerController.sourceType = .camera
                                            self.present(imagePickerController, animated: true, completion: nil)
                                        }
                                        return
                                        
        }, cancel:  { ActionStringCancelBlock in return }, origin:self.view)
        
    }
    private func openVideoController(){
        self.type = "VIDEO"
        let imagePickerController = UIImagePickerController()
        imagePickerController.sourceType = .photoLibrary
        imagePickerController.mediaTypes = ["public.movie"]
        imagePickerController.delegate = self
        self.present(imagePickerController, animated: true, completion: nil)
    }
    private func updatePost(postText:String,postPrivacy:Int,postColor:String){
        ZKProgressHUD.show()
        let userID = UserData.getUSER_ID() ?? ""
        performUIUpdatesOnMain {
            AddPostManager.instance.addPostText( userID:userID,postText: postText, postColor: postColor, postPrivacy: postPrivacy, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
                if success != nil {
//                    print(success?.postData)
                    AppInstance.instance.commingBackFromAddPost = true
                    ZKProgressHUD.dismiss()
                    var postID = ""
                    if let post_data = success?.post_data["post_id"] as? String{
                        postID = post_data
                    }
                    let userInfo = ["data" : ["post_id":postID]]
                    NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                    self.navigationController?.popViewController(animated: true)
                    AppInstance.instance.commingBackFromAddPost = true
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors?.errorText)
                    self.showAlert(title: "", message: (authError?.errors?.errorText)!)
                }
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
        }
    }
    private func uploadImages(imageArray:[Data]){
        ZKProgressHUD.show()
        AddPostManager.instance.addImages(userID : UserData.getUSER_ID() ?? "", postText: self.postText ?? "", postColor: self.PostColor ?? "", postPrivacy: self.postPrivacy ?? 0, imageDataArray: imageArray, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
            if success != nil {
                AppInstance.instance.commingBackFromAddPost = true
                var postID = ""
                if let post_data = success?.post_data["post_id"] as? String{
                    postID = post_data
                }
                let userInfo = ["data" : ["post_id":postID]]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                ZKProgressHUD.dismiss()
                self.navigationController?.popViewController(animated: true)

                
            }
            else if authError != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors?.errorText)
                self.showAlert(title: "", message: (authError?.errors?.errorText)!)
            }
            else if error  != nil {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                
            }
        }
        
    }
    private func uploadVideo(videoData:Data){
        ZKProgressHUD.show()
        
        AddPostManager.instance.addVideo(userID: UserData.getUSER_ID() ?? "", postText: self.postText ?? "", postColor: self.PostColor ?? "", postPrivacy: self.postPrivacy ?? 0, videoData: videoData, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
            if success != nil {
                AppInstance.instance.commingBackFromAddPost = true
                var postID = ""
                if let post_data = success?.post_data["post_id"] as? String{
                    postID = post_data
                }
                let userInfo = ["data" : ["post_id":postID]]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                ZKProgressHUD.dismiss()
                self.navigationController?.popViewController(animated: true)

            }
            else if authError != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors?.errorText)
                self.showAlert(title: "", message: (authError?.errors?.errorText)!)
            }
            else if error  != nil {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                
            }
        }
        
    }
    
    private func postGIF(GIFURL:String){
        ZKProgressHUD.show()
        
        AddPostManager.instance.postGiF(userID: UserData.getUSER_ID() ?? "", postText: self.postText ?? "", postColor: self.PostColor ?? "", postPrivacy: self.postPrivacy ?? 0, GIFUrl: GIFURL, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
            if success != nil {
                AppInstance.instance.commingBackFromAddPost = true
                ZKProgressHUD.dismiss()
                var postID = ""
                if let post_data = success?.post_data["post_id"] as? String{
                    postID = post_data
                }
                let userInfo = ["data" : ["post_id":postID]]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                self.navigationController?.popViewController(animated: true)

            }
            else if authError != nil {
                
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors?.errorText)
                self.showAlert(title: "", message: (authError?.errors?.errorText)!)
            }
            else if error  != nil {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                
            }
        }
        
    }
    private func uploadMusic(musicData:Data){
        ZKProgressHUD.show()
        
        AddPostManager.instance.postMusic(userID: UserData.getUSER_ID() ?? "", postText: self.postText ?? "", postColor: self.PostColor ?? "", postPrivacy: self.postPrivacy ?? 0, musicData: musicData, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
            if success != nil {
                AppInstance.instance.commingBackFromAddPost = true
                var postID = ""
                if let post_data = success?.post_data["post_id"] as? String{
                    postID = post_data
                }
                let userInfo = ["data" : ["post_id":postID]]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                ZKProgressHUD.dismiss()
                self.navigationController?.popViewController(animated: true)

            }
            else if authError != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors?.errorText)
                self.showAlert(title: "", message: (authError?.errors?.errorText)!)
            }
            else if error  != nil {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                
            }
        }
        
    }
    private func uploadFile(fileData:Data,extension1:String){
        ZKProgressHUD.show()
        
        AddPostManager.instance.postFIle(userID: UserData.getUSER_ID() ?? "", postText: self.postText ?? "", postColor: self.PostColor ?? "", postPrivacy: self.postPrivacy ?? 0, fileData: fileData,extension1:extension1, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
            if success != nil {
                AppInstance.instance.commingBackFromAddPost = true
                var postID = ""
                if let post_data = success?.post_data["post_id"] as? String{
                    postID = post_data
                }
                let userInfo = ["data" : ["post_id":postID]]
                NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                ZKProgressHUD.dismiss()
                self.navigationController?.popViewController(animated: true)

            }
            else if authError != nil {
                ZKProgressHUD.dismiss()
                self.view.makeToast(authError?.errors?.errorText)
                self.showAlert(title: "", message: (authError?.errors?.errorText)!)
            }
            else if error  != nil {
                ZKProgressHUD.dismiss()
                print(error?.localizedDescription)
                
            }
        }
        
    }
    private func updateFeeling(feelingType:String,feelingName:String,postText:String,postPrivacy:Int,postColor:String){
        ZKProgressHUD.show()
        let userID = UserData.getUSER_ID() ?? ""
        performUIUpdatesOnMain {
            
            AddPostManager.instance.addFeeling( userID:userID,postText: postText, postColor: postColor, postPrivacy: postPrivacy, feelingName: feelingName, feelingType: feelingType, pageID: self.pageid ?? "", groupID: self.groupId ?? "", eventID: self.eventId ?? "", postType:self.postType ?? "" ) { (success, authError, error) in
                if success != nil {
                    AppInstance.instance.commingBackFromAddPost = true
                      ZKProgressHUD.dismiss()
                    var postID = ""
                    if let post_data = success?.post_data["post_id"] as? String{
                        postID = post_data
                    }
                    let userInfo = ["data" : ["post_id":postID]]
                    NotificationCenter.default.post(name: NSNotification.Name(rawValue: "load"), object: nil, userInfo: userInfo)
                    self.navigationController?.popViewController(animated: true)

                    
                }
                else if authError != nil {
                    ZKProgressHUD.dismiss()
                    self.view.makeToast(authError?.errors?.errorText)
                    self.showAlert(title: "", message: (authError?.errors?.errorText)!)
                }
                else if error  != nil {
                    ZKProgressHUD.dismiss()
                    print(error?.localizedDescription)
                    
                }
            }
        }
    }
}

extension AddPostVC: UITableViewDelegate {
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        if AppInstance.instance.musicSelected{
            switch indexPath.section {
            case 0,2:
                return UITableView.automaticDimension
            case 1:
                if AppInstance.instance.isBackGroundSelected{
                    return 350.0
                }else{
                    return 250.0
                }
            case 3: return 500.0
                
            default:
                return UITableView.automaticDimension
            }
        }else{
            switch indexPath.section {
            case 0:
                return UITableView.automaticDimension
            case 1:
                if AppInstance.instance.isBackGroundSelected{
                    return 350.0
                }else{
                    return 250.0
                }
            case 2: return 500.0
                
            default:
                return UITableView.automaticDimension
            }
        }
        
    }
}

extension AddPostVC: UITableViewDataSource {
    
    func numberOfSections(in tableView: UITableView) -> Int {
        if AppInstance.instance.musicSelected{
            return 4
        }else{
            return 3
        }
        
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if AppInstance.instance.musicSelected{
            
            switch section {
            case 0: return 1
            case 1: return 1
            case 2: return 1
            case 3: return 1
            default: return 0
            }
            
        }else{
            switch section {
            case 0: return 1
            case 1: return 1
            case 2: return 1
            default: return 0
            }
        }
        
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        if AppInstance.instance.musicSelected{
            switch indexPath.section {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionOneTableItem") as! AddPostSectionOneTableItem
                cell.vc = self
                
                cell.bind()
                return cell
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionTwoTableItem") as! AddPostSectionTwoTableItem
                cell.vc = self
                cell.bind()
                return cell
            case 2:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionFourTableItem") as! AddPostSectionFourTableItem
                cell.bind(imageType: self.musicImageType ?? "", URLString: self.pathString ?? "")
                return cell
            case 3:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionThreeTableItem") as! AddPostSectionThreeTableItem
                cell.vc = self
                return cell
            default:
                
                return UITableViewCell()
            }
            
        }else{
            switch indexPath.section {
            case 0:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionOneTableItem") as! AddPostSectionOneTableItem
                cell.vc = self
                
                cell.bind()
                return cell
            case 1:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionTwoTableItem") as! AddPostSectionTwoTableItem
                cell.vc = self
                cell.bind()
                return cell
            case 2:
                let cell = tableView.dequeueReusableCell(withIdentifier: "AddPostSectionThreeTableItem") as! AddPostSectionThreeTableItem
                cell.vc = self
                return cell
            default:
                
                return UITableViewCell()
            }
        }
        
    }
}

extension AddPostVC : UIImagePickerControllerDelegate , UINavigationControllerDelegate {
    
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [UIImagePickerController.InfoKey : Any]) {
        if AppInstance.instance.musicSelected{
            let indexpathforTextView = IndexPath(row: 0, section: 3)
            let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
            
            if self.type == "IMAGE" {
                
                cell.isEmptyMedia = false
                cell.videoArray.removeAll()
                cell.GifsArray.removeAll()
                let img = info[UIImagePickerController.InfoKey.originalImage] as? UIImage
                cell.imageArray.append(img!)
                
                cell.collectionView.reloadData()
            } else if self.type == "VIDEO" {
                print("\(self.type ?? "")")
                cell.isEmptyMedia = false
                
                cell.videoArray.removeAll()
                let vidURL = info[UIImagePickerController.InfoKey.mediaURL] as! URL
                cell.videoArray.append(vidURL.absoluteString)
                let ThumbnailIamge =  self.generateThumbnail(path: vidURL)
                cell.thumbnailiamgeArray.append(ThumbnailIamge!)
                cell.collectionView.reloadData()
                let videoData = try! Data(contentsOf: vidURL)
                
                cell.VideoData = videoData
                cell.thumbData = ThumbnailIamge?.jpegData(compressionQuality: 0.1)
                print(videoData)
            }
        }else{
            let indexpathforTextView = IndexPath(row: 0, section: 2)
            let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
            
            if self.type == "IMAGE" {
                
                cell.isEmptyMedia = false
                cell.videoArray.removeAll()
                cell.GifsArray.removeAll()
                let img = info[UIImagePickerController.InfoKey.originalImage] as? UIImage
               cell.imageArray.append(img!)
                  AppInstance.instance.isAlbumVisible = true
                  self.tableView.reloadData()
                  cell.collectionView.reloadData()
                
            } else if self.type == "VIDEO" {
                print("\(self.type ?? "")")
                cell.isEmptyMedia = false
                
                cell.videoArray.removeAll()
                let vidURL = info[UIImagePickerController.InfoKey.mediaURL] as! URL
                let siteSettings = AppInstance.instance.siteSettings
                var boolValue = false
                if let extensions = siteSettings["allowedExtenstion"] as? String {
                let toExtension = extensions.split(separator: ",")
                 for it in toExtension{
                     if (vidURL.absoluteString.contains(String(it))){
                         boolValue = true
                         break;
                     }else{
                        boolValue = false
                     }
                 }
                  
                }
//                let extensions = AppInstance.instance.siteSettings.config?.allowedExtenstion!.split(separator: ",")
                if boolValue{
                    print("Accepted Extension ")
                    cell.videoArray.append(vidURL.absoluteString)
                    let ThumbnailIamge =  self.generateThumbnail(path: vidURL)
                    cell.thumbnailiamgeArray.append(ThumbnailIamge!)
                    cell.collectionView.reloadData()
                    let videoData = try! Data(contentsOf: vidURL)
                    
                    cell.VideoData = videoData
                    cell.thumbData = ThumbnailIamge?.jpegData(compressionQuality: 0.1)
                    print(videoData)
                }else{
                    self.view.makeToast("This extension is not allowed to upload on server!!")
                }
                
            }
        }
        
        
        self.dismiss(animated: true, completion: nil)
    }
    
    public func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
        picker.dismiss(animated: true)
    }
    func generateThumbnail(path: URL) -> UIImage? {
        do {
            let asset = AVURLAsset(url: path, options: nil)
            let imgGenerator = AVAssetImageGenerator(asset: asset)
            imgGenerator.appliesPreferredTrackTransform = true
            let cgImage = try imgGenerator.copyCGImage(at: CMTimeMake(value: 0, timescale: 1), actualTime: nil)
            let thumbnail = UIImage(cgImage: cgImage)
            return thumbnail
        } catch let error {
            print("*** Error generating thumbnail: \(error.localizedDescription)")
            return nil
        }
    }
}
extension AddPostVC:didSelectPostType{
    
    func didselectPostType(type: String,feelingType:String?,FeelingTypeString:String?) {
        if type == "IMAGE"{
            self.openImageController()
            self.type = "IMAGE"
        }else if type == "VIDEO" {
            self.openVideoController()
            self.type == "VIDEO"
        }else if type == "GIF"{
            self.showGif()
            self.type = "GIF"
        }else if type == "MENTIONCONTACT"{
            let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
            let vc = storyboard.instantiateViewController(withIdentifier: "MentionUserVC") as! MentionUserVC
            vc.delegate = self
            self.navigationController?.pushViewController(vc, animated: true)
            self.type = "MENTIONCONTACT"
        }else if type == "FEELING"{
            if feelingType == "Feeling"{
                let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "FeelingSelectVC") as! FeelingSelectVC
                vc.delegate = self
                            vc.feelingTypeString = feelingType ?? ""
                self.navigationController?.pushViewController(vc, animated: true)
            }else{
                let storyboard = UIStoryboard(name: "AddPost", bundle: nil)
                let vc = storyboard.instantiateViewController(withIdentifier: "FeelingTypeVC") as! FeelingTypeVC
                vc.delegate = self
                vc.feelingTypeString = feelingType ?? ""
                self.present(vc, animated: true, completion: nil)
            }
        }else if type == "MUSIC"{
              self.type = "MUSIC"
            self.openMusicLibrary()
          
            
        }else if type == "FILE"{
              self.type = "FILE"
            self.openiCloudDocuments()
        }else if type == "CAMERA"{
            self.openCamera()
        }
        
        
        
    }
}
extension AddPostVC:didSelectGIFDelegate{
    func didSelectGIF(GIFUrl: String,id: String) {
        let indexpathforTextView = IndexPath(row: 0, section: 2)
        let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionThreeTableItem
        cell.isEmptyMedia = false
        cell.GifsArray.removeAll()
        cell.collectionView.reloadData()
        cell.GifsArray.append(GIFUrl)
        cell.gifURLString = GIFUrl
        self.type = "GIF"
        cell.collectionView.reloadData()
    }
}
extension AddPostVC:didSelectUserDelegate{
    func didSelectUser(userID: String, username: String, index: Int) {
        print("username = @\(username)")
        //        self.userId = userID
        //        self.addUserBtn.setTitle(username, for: .normal)
    }
    
    
}
extension AddPostVC:didSelectingFeelingTypeDelegate{
    func didselectFeelingType(type: String, feelingString: String) {
          print("Type =\(type ?? "") feelingString = \(feelingString ?? "")")
        let indexpathforTextView = IndexPath(row: 0, section: 1)
                                let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionTwoTableItem
        
      
       
           
//            cell.textView.text = "Feeling \(feelingString)"
             cell.textView.text = "\(type) \(feelingString)"
        self.feelingType = type
        self.feelingName = feelingString
        self.type = "FEELING"
        
    }
}
extension AddPostVC:MPMediaPickerControllerDelegate{
    func mediaPicker(_ mediaPicker: MPMediaPickerController, didPickMediaItems mediaItemCollection: MPMediaItemCollection) {
        print("You selected \(mediaItemCollection)")
        
        
        let item: MPMediaItem = mediaItemCollection.items[0]
        let pathURL: NSURL? = item.value(forProperty: MPMediaItemPropertyAssetURL) as? NSURL
        print("Path = \(pathURL?.absoluteString)")
        let siteSettings = AppInstance.instance.siteSettings
        var boolValue = false
        if let extensions = siteSettings["allowedExtenstion"] as? String {
        let toExtension = extensions.split(separator: ",")
         for it in toExtension{
            if ((pathURL?.absoluteString?.contains(String(it))) != nil){
                 boolValue = true
                 break;
             }else{
                boolValue = false
             }
         }
          
        }
        /////////////////
//        let extensions = AppInstance.instance.siteSettings?.config?.allowedExtenstion!.split(separator: ",")
//        var boolValue = false
//        for it in extensions!{
//            if (pathURL?.absoluteString?.contains(String(it)))!{
//                boolValue = true
//                break;
//            }else{
//                boolValue = false
//            }
//        }
        if boolValue{
            print("Accepted Extension ")
            AppInstance.instance.musicSelected = true
            self.musicImageType = self.type ?? ""
            self.pathString = "\(pathURL?.absoluteString?.fileName() ?? "").\(pathURL?.absoluteString?.fileExtension() ?? "")"
            self.type = "MUSIC"
            let musicDATA = try? Data(contentsOf: (pathURL?.absoluteURL)!)
            self.musicData = musicDATA
            self.tableView.reloadData()
            
        }else{
            self.view.makeToast("This extension is not allowed to upload on server!!")
        }
        
        if pathURL == nil {
            self.view.makeToast("Unable to read DRM protected file.")
            
            return
        }
        mediaPicker.dismiss(animated: true, completion: nil)
    }
    func mediaPickerDidCancel(_ mediaPicker: MPMediaPickerController) {
        mediaPicker.dismiss(animated: true, completion: nil)
    }
}

extension AddPostVC:UIDocumentPickerDelegate{
    func documentPickerWasCancelled(_ controller: UIDocumentPickerViewController) {
        controller.dismiss(animated: true, completion: nil)
    }
    func documentPicker(_ controller: UIDocumentPickerViewController, didPickDocumentsAt urls: [URL]) {
        let URL = urls[0]
        var boolValue = false
        let siteSettings = AppInstance.instance.siteSettings
        if let extensions = siteSettings["allowedExtenstion"] as? String {
        let toExtension = extensions.split(separator: ",")
         for it in toExtension{
            if ((URL.absoluteString.contains(String(it))) != nil){
                 boolValue = true
                 break;
             }else{
                boolValue = false
             }
         }
          
        }

        ////////////////////////
//        let extensions = AppInstance.instance.siteSettings?.config?.allowedExtenstion!.split(separator: ",")
//        var boolValue = false
//        for it in extensions!{
//            if (URL.absoluteString.contains(String(it))){
//                boolValue = true
//                break;
//            }else{
//                                boolValue = false
//
//            }
//        }
        if boolValue{
            
            print("Accepted Extension =\(URL.absoluteString.fileExtension())")
            AppInstance.instance.musicSelected = true
            self.musicImageType = self.type ?? ""
            self.pathString = "\(URL.absoluteString.fileName() ?? "").\(URL.absoluteString.fileExtension() ?? "")"
            self.type = "FILE"
            let fileDATA = try? Data(contentsOf: (URL))
            self.fileData = fileDATA
            self.fileExtension = URL.absoluteString.fileExtension()
            self.tableView.reloadData()
            
        }else{
            self.view.makeToast("This extension is not allowed to upload on server!!")
        }
        print("URL =\(URL.absoluteString)")
    }
}
extension AddPostVC:didSelectMentionUsernameDelegate{
    func didSelectMentionUserName(usernameArray: [String]) {
        let stringConverted = usernameArray.joined(separator: ",")
        
        let indexpathforTextView = IndexPath(row: 0, section: 1)
        let cell = tableView.cellForRow(at: indexpathforTextView)! as! AddPostSectionTwoTableItem
        cell.textView.text = stringConverted
        self.tableView.reloadData()
    }
    
    
}
