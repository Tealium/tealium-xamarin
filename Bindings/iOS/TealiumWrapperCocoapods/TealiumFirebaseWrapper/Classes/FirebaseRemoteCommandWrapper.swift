import TealiumWrapperCocoapods
import TealiumFirebase

@objc public class FirebaseRemoteCommandWrapper: RemoteCommandWrapper {
    
    let command: FirebaseRemoteCommand
    init(type: RemoteCommandTypeWrapper) {
        command = FirebaseRemoteCommand(type: type.type)
    }
}
