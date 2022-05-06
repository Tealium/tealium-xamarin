﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using Tealium;
using Tealium.Droid;
using Tealium.RemoteCommands.Firebase.Droid;

namespace TealiumXamarinExample.Droid
{
    [Activity(Label = "TealiumXamarinExample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // We create Tealium instance here so that application lifecycle can start right away
            Teal.Helper.SetInstanceManager(new TealiumInstanceManager(new TealiumInstanceFactoryDroid(this.Application)));

            var command = new FirebaseRemoteCommandDroid(this.Application, null, null);
            Console.WriteLine(command.Name);
            Teal.Helper.RemoteCommands.Add(command);
            Teal.Helper.Init();
            Teal.Helper.Init();

            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
