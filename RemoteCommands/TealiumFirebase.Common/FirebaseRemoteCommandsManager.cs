using System;
using Tealium;

namespace TealiumFirebase
{
    public class FirebaseRemoteCommandsManager
    {
        IFirebaseRemoteCommandsFactory instanceFactory;
        public FirebaseRemoteCommandsManager(IFirebaseRemoteCommandsFactory instanceFactory)
        {
            this.instanceFactory = instanceFactory ?? throw new ArgumentNullException(nameof(instanceFactory));
        }


        public IRemoteCommand Create()
        {
            return instanceFactory.Create();
        }

    }

    public interface IFirebaseRemoteCommandsFactory
    {
        IRemoteCommand Create();
    }
}
