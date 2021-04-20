//
//  MapsUtil.swift
//  Driver
//
//  Copyright © 2018 minimalistic apps. All rights reserved.
//

import UIKit
import CoreLocation

public class MapsUtil {
    public static func encode(items: [CLLocationCoordinate2D]) -> String {
        var lastLat: Int = 0
        var lastLng: Int = 0
        var result = "";
        for item in items {
            let lat = Int((item.latitude * 1e5).rounded())
            let lng = Int((item.longitude * 1e5).rounded())
            let dLat = lat - lastLat
            let dLng = lng - lastLng
            result.append(encodeValue(value: dLat))
            result.append(encodeValue(value: dLng))
            lastLat = lat
            lastLng = lng
        }
        return result
    }
    
    static func encodeValue(value:Int) -> String {
        var result: String = ""
        var v = value < 0 ? ~(value << 1) : value << 1
        while(v>=0x20) {
            result.append(Character(UnicodeScalar(((0x20 | (v & 0x1f)) + 63))!))
            v >>= 5
        }
        result.append(Character(UnicodeScalar(v + 63)!))
        return result
    }
    
    static func distanceToLine(p:CLLocationCoordinate2D, start:CLLocationCoordinate2D, end:CLLocationCoordinate2D) -> Double {
        if start.latitude == end.latitude && start.longitude == end.longitude {
            return CLLocation.distance(from: start, to: end)
        }
        let s0lat = p.latitude.degreesToRadians
        let s0lng = p.longitude.degreesToRadians
        let s1lat = start.latitude.degreesToRadians
        let s1lng = start.longitude.degreesToRadians
        let s2lat = end.latitude.degreesToRadians
        let s2lng = end.longitude.degreesToRadians
        
        let s2s1lat = s2lat - s1lat
        let s2s1lng = s2lng - s1lng
        let u = ((s0lat - s1lat) * s2s1lat + (s0lng - s1lng) * s2s1lng) / (s2s1lat * s2s1lat + s2s1lng * s2s1lng)
        if u <= 0 {
            return CLLocation.distance(from: p, to: start)
        }
        if u >= 1 {
            return CLLocation.distance(from: p, to: end)
        }
        let sa = CLLocationCoordinate2D(latitude: p.latitude - start.latitude, longitude: p.longitude - start.longitude)
        let sb = CLLocationCoordinate2D(latitude: u * (end.latitude - start.latitude), longitude: u * (end.longitude - start.longitude))
        return CLLocation.distance(from: sa, to: sb)
    }
    
    public static func simplify(items:[CLLocationCoordinate2D],tolerance:Double) -> [CLLocationCoordinate2D]{
        var result = items
        let closedPolygon = isClosedPolygen(items: result)
        var lastPoint : CLLocationCoordinate2D = CLLocationCoordinate2D(latitude: 0, longitude: 0)
        if closedPolygon {
            let OFFSET = 0.00000000001
            lastPoint = result.last!
            result.removeLast()
            result.append(CLLocationCoordinate2D(latitude: lastPoint.longitude + OFFSET, longitude: lastPoint.longitude + OFFSET))
        }
        let n = result.count
        var idx: Int
        var maxIdx = 0
        var stack = Stack<[Int]>()
        var dists = [Double?](repeating: nil, count: n)
        dists[0] = 1
        dists[n - 1] = 1
        var maxDist: Double
        var dist : Double = 0.0
        var current: [Int]
        
        if n > 2 {
            let stackVal: [Int] = [0,(n - 1)]
            stack.push(stackVal)
            while stack.count > 0 {
                current = stack.pop()!
                maxDist = 0
                for idx in current[0]..<current[1] {
                    dist = distanceToLine(p: result[idx], start: result[current[0]], end: result[current[1]])
                    if dist > maxDist {
                        maxDist = dist
                        maxIdx = idx
                    }
                }
                if maxDist > tolerance {
                    dists[maxIdx] = maxDist
                    let stackValCurMax = [current[0], maxIdx]
                    stack.push(stackValCurMax)
                    let stackValMaxCur = [maxIdx, current[1]]
                    stack.push(stackValMaxCur)
                }
            }
        }
        
        if(closedPolygon) {
            result.removeLast()
            result.append(lastPoint)
        }
        var finalResult : [CLLocationCoordinate2D] = []
        idx = 0
        for item in result {
            if dists[idx] != 0 {
                finalResult.append(item)
            }
            idx = idx + 1
        }
        return finalResult
        
    }
    
    static func isClosedPolygen(items:[CLLocationCoordinate2D]) -> Bool {
        return (items.first?.latitude == items.last?.latitude && items.first?.longitude == items.last?.longitude)
    }

}

public extension Int {
    var degreesToRadians: Double { return Double(self) * .pi / 180 }
}

public extension FloatingPoint {
    var degreesToRadians: Self { return self * .pi / 180 }
    var radiansToDegrees: Self { return self * 180 / .pi }
}

public extension CLLocation {
    // In meteres
    class func distance(from: CLLocationCoordinate2D, to: CLLocationCoordinate2D) -> CLLocationDistance {
        let from = CLLocation(latitude: from.latitude, longitude: from.longitude)
        let to = CLLocation(latitude: to.latitude, longitude: to.longitude)
        return from.distance(from: to)
    }
}
public extension CLLocationCoordinate2D {
    var xy:[String:Double] {
        var result = [String:Double]()
        result["x"] = self.latitude
        result["y"] = self.longitude
        return result
    }
}
