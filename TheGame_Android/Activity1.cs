using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Xna.Framework;
using Android.Content.PM;

namespace TheGame
{
    [Activity(Label = "Example Game", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar.Fullscreen", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : AndroidGameActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ExampleGame.ExampleGameMain.Activity = this;
            var g = new ExampleGame.ExampleGameMain();
            SetContentView(g.Window);
            g.Run();
        }
    }
}

