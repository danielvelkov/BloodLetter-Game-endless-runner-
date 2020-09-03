using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using GameDemo.Shared;
using Android.Content.PM;
using Android.Runtime;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Android.Util;
using Android.Views.InputMethods;
using System;
using SQLite;
using GameDemo.Shared.Menu;

namespace GameDemo.Android
{
    [Activity(Label = "BloodLetterDemo"
        , MainLauncher = true
        , Icon = "@drawable/Icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , ScreenOrientation = ScreenOrientation.Portrait
        , LaunchMode = LaunchMode.SingleInstance
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout
        
        )]

    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
       
        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            
            var g = new Game1();
            // gets the game screen
            SetContentView((View)g.Services.GetService(typeof(View)));
            
            // prevent screen from turning off due to sleep timer
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
           
            g.Run();
            
        }

        
        
    }

}

