using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Plugin.Geolocator;

namespace GeoFencing
{
    [Activity(Label = "LocationPickActivity", MainLauncher = false)]
    public class LocationPickActivity : FragmentActivity, IOnMapReadyCallback
    {
        private GoogleMap googleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.LocationPickLayout);
            var supportMapFragment = (SupportMapFragment)  SupportFragmentManager.FindFragmentById(Resource.Id.userHomeLocationPickMapView);
            supportMapFragment.OnCreate(savedInstanceState);
            supportMapFragment.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            this.googleMap = googleMap;
            //Setup and customize your Google Map
            this.googleMap.UiSettings.CompassEnabled = false;
            this.googleMap.UiSettings.MyLocationButtonEnabled = false;
            this.googleMap.UiSettings.MapToolbarEnabled = false;

            MapsInitializer.Initialize(this);
            var me = new LatLng(40.759010, -73.984474);
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLng(me));
            googleMap.MoveCamera(CameraUpdateFactory.ZoomTo(15));
        }
    }
}