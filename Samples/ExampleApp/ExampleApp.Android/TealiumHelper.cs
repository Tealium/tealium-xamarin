using System;
using System.Collections.Generic;

using Tealium;
using Tealium.Droid;

namespace ExampleApp.Droid
{

    /// <summary>
    /// This class demonstrates how to configure the Tealium API for using
    /// <see cref="TealiumInstanceManager"/> and how to acquire a 
    /// Tealium instance configured to automatically track application
    /// lifecycle, how to add a remote command, how to configure data sources,
    /// how to create dispatch validator and event listeners.
    /// </summary>
    public class TealiumHelper
    {
        public static ITealiumInstanceManager instanceManager;

        static bool initialized = false;

        public static Tealium.ITealium GetTealium(Android.App.Application app)
        {
            if (!initialized)
            {
                initialized = true;

                //***********************************************************
                // 1. Mandatory configuration - this is platform specific
                //***********************************************************
                instanceManager = new TealiumInstanceManager(new TealiumInstanceFactoryDroid(app));
                TealiumConsts.InstanceManager = instanceManager;

                // provide the Tealium automatic lifecycle tracking delegate - without this TealiumInstanceFactoryDroid won't
                // be able to set automatic lifecycle tracking (not set will result in null and not being able to track lifecycle)
                //TealiumLifecycleManager.SetLifecycleAutoTracking = TealiumDroid.Lifecycle.TealiumLifecycleControlDelegation.SetLifecycleAutoTracking;


                //***********************************************************
                // 2. Optional configuration - this is cross-platform
                //***********************************************************

                // the below two steps are optional, but they demonstrate
                // how to use remote commands, dispatch validators and event listeners
                var commands = SetupRemoteCommands();


                //***********************************************************
                // 3. Final configuration and Tealium instance creation - this is also cross-platform
                //***********************************************************

                if (TealiumConsts.Environment  == Tealium.Environment.Dev){
                    Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
                }

                TealiumConfig config = new TealiumConfig(TealiumConsts.AccountName,
                                                         TealiumConsts.ProfileName,
                                                         TealiumConsts.Environment,
                                                         new List<Dispatchers> {
                                                             Dispatchers.Collect, Dispatchers.RemoteCommands, Dispatchers.TagManagement
                                                         },
                                                         new List<Collectors> {
                                                             Collectors.AppData
                                                         });


                var tealium = instanceManager.CreateInstance(config);
            }

            return instanceManager.GetExistingInstance(TealiumConsts.InstanceId);
        }


        static List<IRemoteCommand> SetupRemoteCommands()
        {
            var command = new DelegateRemoteCommand(TealiumConsts.RemoteCommandId, "Test command " + TealiumConsts.RemoteCommandId)
            {
                HandleResponseDelegate = (DelegateRemoteCommand cmd, IRemoteCommandResponse resp) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Handling command {cmd.CommandId}...");
                    /*System.Threading.Tasks.Task.Run(() =>
                    {
                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(new RemoteCommandHandledEvent(DateTime.Now));
                    }
                    );*/

                }
            };

            return new List<IRemoteCommand>() { command };
        }
    }
}
