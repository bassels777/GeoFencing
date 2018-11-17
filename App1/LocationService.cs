using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ILocationListener = Android.Gms.Location.ILocationListener;

namespace App1
{
    [Service]
    public class LocationService : Service, ILocationListener
    {
        private GoogleApiClient m_googleApiClient;
        private Messenger m_messenger;

        public LocationService()
        {
            m_messenger = new Messenger(new IncomingHandler());
        }

        public override IBinder OnBind(Intent intent)
        {
            return m_messenger.Binder;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            m_googleApiClient = new GoogleApiClient.Builder(ApplicationContext)
                .AddApi(LocationServices.API)
                .AddConnectionCallbacks(() => OnConnected())
                .AddOnConnectionFailedListener(result => {})
                .Build();

            m_googleApiClient.Connect();

            Log.Debug("LocationService","Started service");
            return base.OnStartCommand(intent, flags, startId);
        }

        private void OnConnected()
        {
            LocationServices.FusedLocationApi.RequestLocationUpdates(m_googleApiClient, CreateLocationRequest(1000), this);
        }

        public override void OnCreate()
        {
            Log.Debug("LocationService", "created service");
            base.OnCreate();
        }

        private LocationRequest CreateLocationRequest(long waitTime)
        {
            var locationRequest = new LocationRequest();

            locationRequest.SetMaxWaitTime(waitTime);
            locationRequest.SetInterval(1000);
            locationRequest.SetFastestInterval(1000);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            return locationRequest;
        }

        public void OnLocationChanged(Location location)
        {
            LocationServices.FusedLocationApi.RequestLocationUpdates(m_googleApiClient, CreateLocationRequest(10000), this);
        }

        private class IncomingHandler : Handler
        {
            public override void HandleMessage(Message msg)
            {
            }
        }
        
    }
}