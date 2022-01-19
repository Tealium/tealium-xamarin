using System;

using Tealium;

namespace ExampleApp
{
    public static class TealiumConsts
    {
        
        public static readonly string AccountName = "tealiummobile";
        public static readonly string ProfileName = "demo";
        public static readonly Tealium.Environment Environment = Tealium.Environment.Dev;
        public static readonly string InstanceId = TealiumInstanceManager.GetInstanceId(TealiumConsts.AccountName, TealiumConsts.ProfileName, TealiumConsts.Environment);
        public static readonly string RemoteCommandId = "verify";

        public static ITealiumInstanceManager InstanceManager 
        {
            get; 
            set;
        }

        public static ITealium DefaultInstance 
        {
            get => InstanceManager?.GetExistingInstance(InstanceId);
        }
    }
}
