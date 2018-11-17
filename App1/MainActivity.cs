using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Java.Lang;
using Exception = Java.Lang.Exception;
using ILocationListener = Android.Gms.Location.ILocationListener;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = false, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        int count = 1;
        private IGeofencingApi m_geofencingClient;
        private GoogleApiClient m_googleApiClient;
        private LocationService m_locationService;
        private ServiceConnection m_connection = new ServiceConnection();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            m_googleApiClient = new GoogleApiClient.Builder(ApplicationContext)
                .AddApi(LocationServices.API)
                .AddConnectionCallbacks(() =>
                OnConnected())
                .AddOnConnectionFailedListener(result =>
                {
                    var x = 5;
                })
                .Build();

            
            button.Click += delegate
            {
                //var location = LocationServices.FusedLocationApi.GetLastLocation(m_googleApiClient);
                //button.Text = $"{location.Longitude}, {location.Latitude}, {location.Accuracy}";

                var msg = Message.Obtain(null, 585);
                m_connection.Service.Send(msg);
            };
          
            m_googleApiClient.Connect();

           //var intent = new Intent(this, typeof(LocationService));
            //StartService(intent);
        }

        protected override void OnStart()
        {
            base.OnStart();

            BindService(new Intent(this, typeof(LocationService)), m_connection, Bind.AutoCreate);

        }

        private class ServiceConnection : Java.Lang.Object, IServiceConnection
        {
            public Messenger Service;

            public void Dispose()
            {
            }

            public void OnServiceConnected(ComponentName name, IBinder service)
            {
                Service = new Messenger(service);
            }

            public void OnServiceDisconnected(ComponentName name)
            {
                Service = null;
            }
        }

        private void OnConnected()
        {
            if (ContextCompat.CheckSelfPermission(ApplicationContext,
                    Manifest.Permission.AccessFineLocation)
                != Permission.Granted)
            {

                ActivityCompat.RequestPermissions(this,
                    new[] { Manifest.Permission.AccessFineLocation }, 5);

                // MY_PERMISSIONS_REQUEST_ACCESS_FINE_LOCATION is an
                // app-defined int constant. The callback method gets the
                // result of the request.
            }

            if (ContextCompat.CheckSelfPermission(ApplicationContext,
                    Manifest.Permission.AccessMockLocation)
                != Permission.Granted)
            {

                ActivityCompat.RequestPermissions(this,
                    new[] { Manifest.Permission.AccessMockLocation }, 6);

                // MY_PERMISSIONS_REQUEST_ACCESS_FINE_LOCATION is an
                // app-defined int constant. The callback method gets the
                // result of the request.
            }

            var requestId = "rid";
            m_geofencingClient = LocationServices.GeofencingApi;
            var geoFence = new GeofenceBuilder()
                .SetRequestId(requestId)
                .SetTransitionTypes(Geofence.GeofenceTransitionEnter | Geofence.GeofenceTransitionExit | Geofence.GeofenceTransitionDwell)
                .SetLoiteringDelay(1000)
                .SetCircularRegion(37.621313, -122.378955, 1000)
                .SetExpirationDuration(Geofence.NeverExpire)
                .SetNotificationResponsiveness(1000)
                .Build();
            Intent intent = new Intent(ApplicationContext, typeof(GeoIntentService));
            PendingIntent pendingIntent = PendingIntent.GetService(ApplicationContext, 0, intent, PendingIntentFlags.UpdateCurrent);

            try
            {
                m_geofencingClient.AddGeofences(m_googleApiClient, GetAddGeofencingRequest(geoFence), pendingIntent);
            }
            catch (IllegalStateException exception)
            {
                Log.Error("tag", exception.ToString());
            }
            catch (Exception exception)
            {
            }
        }


        private LocationRequest CreateLocationRequest()
        {
            var locationRequest = new LocationRequest();

            locationRequest.SetInterval(1000);
            locationRequest.SetFastestInterval(1000);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            return locationRequest;
        }
        public GeofencingRequest GetAddGeofencingRequest(IGeofence geofence)
        {
            List<IGeofence> geofencesToAdd = new List<IGeofence>();
            geofencesToAdd.Add(geofence);
            GeofencingRequest.Builder builder = new GeofencingRequest.Builder();
            builder.SetInitialTrigger(GeofencingRequest.InitialTriggerEnter);
            builder.AddGeofences(geofencesToAdd);
            return builder.Build();
        }

        public class GeoIntentService : IntentService
        {
            protected override void OnHandleIntent(Intent intent)
            {
                Log.Debug("geofence", "hereeeeeeeeeeeeeee");
                var geofencingEvent = GeofencingEvent.FromIntent(intent);
            }
        }

        public void OnLocationChanged(Location location)
        {
            var x = 0;
            Log.Debug("location", $"{location.Longitude}, {location.Latitude}");

            //throw new NotImplementedException();
        }
    }
}

