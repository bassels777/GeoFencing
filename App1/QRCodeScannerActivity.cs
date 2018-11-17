using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App1.Services;

namespace App1
{
    [Activity(Label = "QRCodeScannerActivity", MainLauncher = true)]
    public class QRCodeScannerActivity : Activity,  View.IOnClickListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.QCodeScannerLayout);
            var buttonView = FindViewById<Button>(Resource.Id.scanQrCode);
            buttonView.SetOnClickListener(this);
        }

        public async void OnClick(View v)
        {
            try
            {
                var scan = await (new QrScanningService()).ScanAsync(this);
            }
            catch (Exception e)
            {

            }
        }
    }
}