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

namespace Arkanoid.BroadcastService
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new String[] { "com.hufsha.MusicService" } )]
    public class MusicReciver : BroadcastReceiver
    {
        Context context;
        public MusicReciver() { }
        public MusicReciver(Context context)
        {
            this.context = context;
        }
        public override void OnReceive(Context context, Intent intent)
        {
            Intent inten = new Intent(this.context, typeof(MusicService));
            ((Activity)context).StartService(inten);
        }
    }
}