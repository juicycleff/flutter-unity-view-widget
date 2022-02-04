//
//  FLTUnityView.swift
//  flutter_unity_widget
//
//  Created by Rex Raphael on 30/01/2021.
//

import Foundation
import UIKit
import UnityFramework

class FLTUnityView: UIView {
    public var uView: UIView?

    override init(frame: CGRect) {
        super.init(frame: frame)
    }

    deinit {
    }

    func setUnityView(_ view: UIView?) {
        uView = view
        setNeedsLayout()
    }

    override func layoutSubviews() {
        super.layoutSubviews()
        uView?.removeFromSuperview()
        if let uView = uView {
            insertSubview(uView, at: 0)
        }
        (uView)?.frame = bounds
        uView?.setNeedsLayout()
    }

    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
}
