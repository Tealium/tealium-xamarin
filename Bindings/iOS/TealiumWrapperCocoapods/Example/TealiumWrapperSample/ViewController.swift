//
//  ViewController.swift
//  TealiumWrapperSample
//
//  Created by Enrico Zannini on 04/01/22.
//  Copyright © 2022 CocoaPods. All rights reserved.
//

import UIKit
import TealiumFirebaseWrapper
import TealiumWrapperCocoapods

class ViewController: UIViewController {

    override func viewDidLoad() {
        super.viewDidLoad()
        _ = FirebaseRemoteCommandWrapper(type: RemoteCommandTypeWrapper())
        // Do any additional setup after loading the view.
    }


}

