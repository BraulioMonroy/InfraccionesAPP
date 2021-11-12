using Android.Content;
using Android.Nfc;
using Android.OS;
using Infracciones.Infrastructure.ZebraPrintStation.Services;
using System;
using System.Text;

namespace Infracciones.App.Droid
{
    public class NfcManagerImplementation : INfcManager
    {
        public event EventHandler<string> TagScanned;

        public void OnNewIntent(object sender, Intent e)
        {
            IParcelable[] tags = e.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
            if (tags?.Length > 0)
            {
                NdefMessage message = (NdefMessage)tags[0];
                string nfcData = Encoding.UTF8.GetString(message.GetRecords()[0].GetPayload());

                TagScanned?.Invoke(this, nfcData);
            }
        }

        public bool IsNfcAvailable()
        {
            try
            {
                return ((NfcManager)Android.App.Application.Context.GetSystemService(Context.NfcService)).DefaultAdapter.IsEnabled;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}