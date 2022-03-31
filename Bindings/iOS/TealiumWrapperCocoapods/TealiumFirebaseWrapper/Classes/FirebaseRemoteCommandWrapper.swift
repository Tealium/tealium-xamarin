import TealiumWrapperCocoapods
import TealiumFirebase

@objc public class FirebaseRemoteCommandWrapper: RemoteCommandWrapper {
    
    let command: FirebaseRemoteCommand
    @objc public init(type: RemoteCommandTypeWrapper) {
        let command = FirebaseRemoteCommand(type: type.type)
        self.command = command
        super.init(commandId: command.commandId,
                   description: command.description) { response in
            command.completion(response.response)
        }
    }
}
