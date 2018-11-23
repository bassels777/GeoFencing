using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GeoFencing
{
    [Activity(Label = "UsernamePasswordActicity", MainLauncher = false)]
    public class UsernamePasswordActicity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.UsernamePasswordLoginLayout);
        }
    }
}