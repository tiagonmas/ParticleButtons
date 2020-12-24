using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleButtons.Droid
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://particlebuttonnotificationhubns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=+eqDD4FXinf1BSFY7rPGP0741AdLmH3gc1FQeIECsBk=";
        public const string NotificationHubName = "ParticleButtonNotificationHub";
    }
}