﻿using System;
using TealiumFirebase;
using TealiumFirebase.iOS;
using Tealium;
using Tealium.Platform.iOS;
using TealiumFirebase.Platform.iOS;


namespace TealiumFirebase.iOS
{
    public class FirebaseRemoteCommandsFactoryIOS : IFirebaseRemoteCommandsFactory
    {
        RemoteCommandTypeWrapper type;
        public FirebaseRemoteCommandsFactoryIOS(RemoteCommandTypeWrapper type)
        {
            this.type = type;
        }

        public IRemoteCommand Create()
        {
            return new FirebaseRemoteCommandWrapper(type);
        }
    }
}
