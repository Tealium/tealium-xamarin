using System;
using System.Collections.Generic;
using Tealium;


namespace Tealium.RemoteCommands.Firebase
{
    public abstract class FirebaseRemoteCommand : IRemoteCommand
    {

        public static readonly string KeySessionTimeout = "firebase_session_timeout_seconds";
        public static readonly string KeyMinSeconds = "firebase_session_minimum_seconds";
        public static readonly string KeyAnalyticsEnabled = "firebase_analytics_enabled";
        // reserved for future use. log level can only be set
        public static readonly string KeyLogLevel = "firebase_log_level";
        public static readonly string KeyEventName = "firebase_event_name";
        public static readonly string KeyEventParams = "firebase_event_params";
        public static readonly string KeyScreenName = "firebase_screen_name";
        public static readonly string KeyScreenClass = "firebase_screen_class";
        public static readonly string KeyUserPropertyName = "firebase_property_name";
        public static readonly string KeyUserPropertyValue = "firebase_property_value";
        public static readonly string KeyUserId = "firebase_user_id";
        public static readonly string KeyCommandName = "command_name";

        protected static readonly Dictionary<string, string> eventsMap = new Dictionary<string, string>();
        protected static readonly Dictionary<string, string> parameters = new Dictionary<string, string>();

        public static readonly string DefaultCommandId = "firebaseAnalytics";
        public static readonly string DefaultCommandDesc = "Tealium Firebase Analytics integration";


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tealium.RemoteCommands.Firebase.FirebaseRemoteCommand"/> class.
        /// Pre-built Remote Command supporting Firebase Analytics for both iOS
        /// and Android on Xamarin. Empty constructor will use the 
        /// <see langword="static"/> String value of firebaseAnalytics as the
        /// command id.
        /// </summary>
        public FirebaseRemoteCommand() : this(DefaultCommandId, DefaultCommandDesc)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tealium.RemoteCommands.Firebase.FirebaseRemoteCommand"/> class.
        /// Pre-built Remote Command supporting Firebase Analytics for both iOS
        /// and Android on Xamarin. Supply a commmand id and description to 
        /// override the defaults.
        /// </summary>
        /// <param name="commandId">Command identifier.</param>
        /// <param name="description">Description.</param>
        public FirebaseRemoteCommand(string commandId, string description)
        {
            this.CommandId = commandId ?? DefaultCommandId;
            this.Description = description ?? DefaultCommandDesc;
        }

        public string CommandId { get; set; }

        public string Description { get; set; }

        public abstract string Path { get; }
        public abstract string Url { get; }

        /// <summary>
        /// Handles the Remote Command response - this contains all the logic 
        /// for executing the different functions Firebase Analytics.
        /// A comma separated list of command names as set out in the <see cref="Commands"/>
        /// class is required at the paylod key <see cref="KeyCommandName"/>
        /// </summary>
        /// <param name="response">Response.</param>
        public void HandleResponse(IRemoteCommandResponse response)
        {
            String command = response.Payload.GetValueForKey<String>(KeyCommandName);
            if (String.IsNullOrEmpty(command))
            {
                return;
            }

            String[] commandArray;
            // split the commands into an array
            commandArray = command.Split(",");

            for (int j = 0, commandlen = commandArray.Length; j < commandlen; j++)
            {
                try
                {
                    command = commandArray[j];
                    command = command.Trim();
                    switch (command)
                    {
                        case Commands.Config:
                            int? timeout = null;
                            try
                            {
                                timeout = response.Payload.ContainsKey(KeySessionTimeout) == true ?
                                            (int?)Int32.Parse(response.Payload.GetValueForKey<String>(KeySessionTimeout)) * 1000 : null;
                            }
                            catch { }

                            int? minSeconds = null;
                            try
                            {
                                minSeconds = response.Payload.ContainsKey(KeyMinSeconds) == true ?
                                            (int?)Int32.Parse(response.Payload.GetValueForKey<String>(KeyMinSeconds)) * 1000 : null;
                            }
                            catch { }

                            bool? analyticsEnabled = null;
                            try
                            {
                                analyticsEnabled = response.Payload.GetValueForKey<String>(KeyAnalyticsEnabled) != null && response.Payload.GetValueForKey<String>(KeyAnalyticsEnabled) == "false" ?
                                            (bool?)false : true;
                            }
                            catch { }

                            Configure(timeout, minSeconds, analyticsEnabled);

                            break;
                        case Commands.LogEvent:
                            String eventName = response.Payload.GetValueForKey<String>(KeyEventName);

                            Dictionary<string, string> eventParams;
                            if (response.Payload.ContainsKey(KeyEventParams))
                            {
                                try
                                {
                                    eventParams = response.Payload.GetValueForKey<Dictionary<string, string>>(KeyEventParams);
                                }
                                catch
                                {
                                    //fallback to empty dictionary.
                                    eventParams = new Dictionary<string, string>();
                                }
                            }
                            else
                            {
                                eventParams = new Dictionary<string, string>();
                            }

                            if (!IsNullOrNullString(eventName))
                            {
                                LogEvent(mapEventNames(eventName), eventParams);
                            }
                            break;
                        case Commands.SetScreenName:
                            String screenName = response.Payload.GetValueForKey<String>(KeyScreenName);
                            String screenClass = response.Payload.GetValueForKey<String>(KeyScreenClass);
                            if (!IsNullOrNullString(screenName) && !IsNullOrNullString(screenClass))
                            {
                                SetScreenName(screenName, screenClass);
                            }
                            break;
                        case Commands.SetUserProperty:
                            String propertyName = response.Payload.GetValueForKey<String>(KeyUserPropertyName);
                            String propertyValue = response.Payload.GetValueForKey<String>(KeyUserPropertyValue);
                            if (!IsNullOrNullString(propertyName) && !IsNullOrNullString(propertyValue))
                            {
                                SetUserProperty(propertyName, propertyValue);
                            }
                            break;
                        case Commands.SetUserId:
                            String userId = response.Payload.GetValueForKey<String>(KeyUserId);
                            if (!IsNullOrNullString(userId))
                            {
                                SetUserId(userId);
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error handling " + this.CommandId + ":" + command + " - " + e.Message);
                    System.Diagnostics.Debug.WriteLine("Stack Trace: " + e.StackTrace);
                }

            }

        }

        /// <summary>
        /// Helper method that returns a known event name or the <paramref name="eventName"/>
        /// provided.
        /// </summary>
        /// <returns>The known even name, or provided event name</returns>
        /// <param name="eventName">Event name.</param>
        protected static String mapEventNames(String eventName)
        {
            return eventsMap[eventName] ?? eventName;
        }

        /// <summary>
        /// Helper method that returns a known param name or the <paramref name="param"/>
        /// provided.        
        /// </summary>
        /// <returns>The known parameter name, or provided param name.</returns>
        /// <param name="param">Parameter.</param>
        protected static String mapParams(String param)
        {
            return parameters[param] ?? param;
        }

        /// <summary>
        /// Helper method to determine whether a string is null, or a null string.
        /// The conversion from a <see langword="null"/> entry in an iOS disctionary
        /// will end up casting the null to a string value "&lt;null&gt;"
        /// Although this is iOS specific, we need to keep the handling in this class
        /// so we can stop any unnecessary subclass method calls.
        /// </summary>
        /// <returns><c>true</c>, if null or null string was found, <c>false</c> otherwise.</returns>
        /// <param name="value">Value.</param>
        private bool IsNullOrNullString(string value)
        {
            //iOS null values in a NSDictionary come through as "<null>" string.
            return value == null || value == "<null>";
        }

        /// <summary>
        /// Configure Firebase Analytics with the specified sessionTimeoutDuration, 
        /// minSessionDuration and analyticsEnabled.
        /// </summary>
        /// <param name="sessionTimeoutDuration">Session timeout duration.</param>
        /// <param name="minSessionDuration">Minimum session duration.</param>
        /// <param name="analyticsEnabled">Analytics enabled.</param>
        protected abstract void Configure(int? sessionTimeoutDuration, int? minSessionDuration, bool? analyticsEnabled);

        /// <summary>
        /// Logs a Firebase event with the given <paramref name="eventName"/> and <paramref name="eventParams"/>.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="eventParams">Event parameters.</param>
        protected abstract void LogEvent(String eventName, Dictionary<String, String> eventParams);

        /// <summary>
        /// Sets the <paramref name="screenName"/> and <paramref name="screenClass"/>.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        /// <param name="screenClass">Screen class.</param>
        protected abstract void SetScreenName(String screenName, String screenClass);

        /// <summary>
        /// Sets a property on the existing User, given by the supplied 
        /// <paramref name="propertyName"/> and <paramref name="propertyValue"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="propertyValue">Property value.</param>
        protected abstract void SetUserProperty(String propertyName, String propertyValue);

        /// <summary>
        /// Sets the current user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        protected abstract void SetUserId(String userId);

        public void Dispose()
        {
            //do nothing.
        }

        /// <summary>
        /// Available Command names, this will be looked for in any Remote
        /// Command payload at the key of <see cref="KeyCommandName"/>
        /// </summary>
        public static class Commands
        {
            public const String Config = "config";
            public const String SetUserId = "setUserId";
            public const String SetUserProperty = "setUserProperty";
            public const String SetScreenName = "setScreenName";
            public const String LogEvent = "logEvent";
        }

    }
}
