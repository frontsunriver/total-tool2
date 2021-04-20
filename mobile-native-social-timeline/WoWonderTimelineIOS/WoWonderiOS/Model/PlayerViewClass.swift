

import Foundation
import AVFoundation
import AVKit

class PlayerViewClass : UIView{
    
    override static var layerClass : AnyClass{
        
        return AVPlayerLayer.self
    }
    
    var playerLayer : AVPlayerLayer{
        return layer as! AVPlayerLayer
    }
    
    var player:AVPlayer?{
        
        get{
            return playerLayer.player
        }
        
        set {
            playerLayer.player = newValue
        }
        
    }
    
}
