using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Arkanoid.BroadcastService
{
    [Service(Label = "MusicService")]//write service to menifest file 
    [IntentFilter(new String[] { "com.hufsha.MusicService" })]
    public class MusicService : Service
    {
        IBinder binder;//null not in bagrut 
        Timer t = new Timer();
        MediaPlayer mp;
        public static bool NowIsPlaying;
        public override IBinder OnBind(Intent intent)
        {
            binder = new MusicServiceBinder(this);
            return binder;
        }
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            // start your service logic here
            mp = MediaPlayer.Create(this, Resource.Raw.backgroundMusic);
            mp.Start();
            NowIsPlaying = true;
            t = new Timer(mp.Duration);
            t.Elapsed += T_Elapsed;

            // Return the correct StartCommandResult for the type of service you are building
            return StartCommandResult.NotSticky;
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            mp.Stop();
            mp.Release();
            mp = null;
            this.StopSelf();
            Intent intent = new Intent(this, typeof(MusicReciver));
            this.StartActivity(intent);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            mp.Pause();
            NowIsPlaying = false;
            //mp.Stop();
            //mp.Release();
            //mp = null;
            
        }
    }
    public class MusicServiceBinder : Binder
    {
        readonly MusicService service;

        public MusicServiceBinder(MusicService service)
        {
            this.service = service;
        }

        public MusicService GetFirstService()
        {
            return service;
        }
    }
}