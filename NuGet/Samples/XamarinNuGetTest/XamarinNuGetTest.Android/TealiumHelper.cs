﻿using System;
using System.Collections.Generic;

using Tealium;
using Tealium.Droid;

namespace XamarinNuGetTest.Droid
{


    /// <summary>
    /// This class demonstrates how to configure the Tealium APIs for using
    /// <see cref="TealiumInstanceManager"/> and how to acquire a 
    /// Tealium instance configured to automatically track application
    /// lifecycle, how to add a remote command, hot to configure data sources,
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
                //1. Mandatory configuration - this is platform specific
                //***********************************************************
                instanceManager = new TealiumInstanceManager(new TealiumInstanceFactoryDroid(app));

                //provide the Tealium automatic lifecycle tracking delegate - without this TealiumInstanceFactoryDroid won't
                //be able to set automatic lifecycle tracking (not set will result in null and not being able to track lifecycle)
                //TealiumLifecycleManager.SetLifecycleAutoTracking = TealiumDroid.Lifecycle.TealiumLifecycleControlDelegation.SetLifecycleAutoTracking;


                //***********************************************************
                //2. Optional configuration - this is cross-platform
                //***********************************************************

                //the below two steps are optional, but they demonstrate
                //how to use remote commands, dispatch validators and event listeners
                var commands = SetupRemoteCommands();
                TealiumAdvancedConfig advConfig = SetupAdvancedConfig();


                //***********************************************************
                //3. Final configuration and Tealium instance creation - this is also cross-platform
                //***********************************************************

                if (TealiumConsts.ENVIRONMENT  == "dev"){
                    Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
                }

                TealiumConfig config = new TealiumConfig(TealiumConsts.INSTANCE_ID,
                                                         TealiumConsts.ACCOUNT_NAME,
                                                         TealiumConsts.PROFILE_NAME,
                                                         TealiumConsts.ENVIRONMENT,
                                                         true,
                                                         commands,
                                                         advConfig);
                
                var tealium = instanceManager.CreateInstance(config);

                //***********************************************************
                //4. Optionally add data sources - this is cross-platform too
                //***********************************************************

                SetupDataSources(tealium);
            }

            return instanceManager.GetExistingInstance(TealiumConsts.INSTANCE_ID);
        }

        static void SetupDataSources(ITealium tealium)
        {
            tealium.AddVolatileDataSources(new Dictionary<string, object>(5) 
            { 
                { "VolatileDataTest", "Example volatile value." },
                {"volatile_1" , "volatile_value_1"},
                {"volatile_2" , "volatile_value_2"},
                {"volatile_3" , "volatile_value_3"},
                {"event_source" , "collect"}
            });

            tealium.AddPersistentDataSources(new Dictionary<string, object>(4)
            {
                { "PersistentDataTest", "Example persistent value." }/*,
                {"persistent_1" , "persistent_value_1"},
                {"persistent_2" , "persistent_value_2"},
                {"persistent_3" , "persistent_value_3"}*/
            });
        }

        static TealiumAdvancedConfig SetupAdvancedConfig()
        {
            DelegateDispatchValidator validator = new DelegateDispatchValidator()
            {
                ShouldDropDispatchDelegate = (ITealium arg1, IDispatch arg2) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside ShouldDropDispatchDelegate!");
                    return false;
                },

                ShouldQueueDispatchDelegate = (ITealium arg1, IDispatch arg2, bool shouldQueue) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside ShouldQueueDispatchDelegate!");
                    return shouldQueue;
                }
            };

            DispatchSentDelegateEventListener sendingListener = new DispatchSentDelegateEventListener()
            {
                DispatchSent = (tealium, dispatch) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside DispatchSent!");
                    dispatch.PutString("KeyAddedBySendListener", "Value added by sending listener.");
                }
            };

            DispatchQueuedDelegateEventListener queuingListener = new DispatchQueuedDelegateEventListener()
            {
                DispatchQueued = (tealium, dispatch) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside DispatchQueued!");
                    dispatch.PutString("KeyAddedByQueuedListener", "Value added by queuing listener.");
                }
            };

            WebViewReadyDelegateEventListener webViewListener = new WebViewReadyDelegateEventListener()
            {
                WebViewReady = (tealium, webView) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside WebViewReady!");
                }
            };

            SettingsPublishedDelegateEventListener settingsListener = new SettingsPublishedDelegateEventListener()
            {
                SettingsPublished = (tealium) =>
                {
                    System.Diagnostics.Debug.WriteLine("Inside SettingsPublished!");
                }
            };

            TealiumAdvancedConfig advConfig = new TealiumAdvancedConfig(validator, 
                                                                        sendingListener, 
                                                                        queuingListener,
                                                                        webViewListener,
                                                                        settingsListener);
            return advConfig;
        }

        static List<IRemoteCommand> SetupRemoteCommands()
        {
            var command = new DelegateRemoteCommand(TealiumConsts.REMOTE_COMMAND_ID, "Test command " + TealiumConsts.REMOTE_COMMAND_ID)
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
