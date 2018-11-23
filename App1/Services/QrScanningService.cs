using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;

namespace GeoFencing.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync(Context context);
    }

    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync(Context context)
        {
            var optionsCustom = new MobileBarcodeScanningOptions();
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the QR Code",
            };

            var scanResult = await scanner.Scan(context, optionsCustom);
            return scanResult.Text;
        }
    }
}