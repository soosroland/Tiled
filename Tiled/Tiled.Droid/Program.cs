using System;
using System.Diagnostics;

using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using Microsoft.Xna.Framework;

using CocosSharp;
using Tiled.Shared;

namespace Tiled.Droid
{

    [Activity(Label = "Tiled.Droid"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , ScreenOrientation = ScreenOrientation.Landscape
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class Program : AndroidGameActivity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CCApplication application = new CCApplication();
			application.ApplicationDelegate = new AppDelegate();

			this.SetContentView(application.AndroidContentView);

			application.StartGame();

        }        
    }


}

