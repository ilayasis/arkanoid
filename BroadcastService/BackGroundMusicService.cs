using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Arkanoid
{ [Service]
    class BackGroundMusicService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        MediaPlayer MP;
        public static bool NowIsPlaying;

        public override void OnCreate()
        {
            base.OnCreate();
            MP = MediaPlayer.Create(this, Resource.Raw.backgroundMusic);
            MP.Looping = true;
        }
        [return: GeneratedEnum]

        public override StartCommandResult OnStartCommand (Intent intent, [GeneratedEnum] StartCommandFlags flags, int IdStart)//מתחיל לפעול
        {
            MP.Start();
            NowIsPlaying = true;
            Intent intentMusic = new Intent("BackgroundMusic");
            intentMusic.PutExtra("ItHasStarted", true);//קורא לברודקאסט ואומר לו שהוא התחיל
            SendBroadcast(intentMusic);
            return base.OnStartCommand(intent, flags, IdStart);


        }
        public override void OnDestroy()//מפסיק לפעול
        {
            base.OnDestroy();
            MP.Pause(); 
            NowIsPlaying = false;
            Intent intentMusic = new Intent("BackgroundMusic");
            intentMusic.PutExtra("ItHasStarted", false);//קורא לברודקאסט ואומר שהוא סיים
            SendBroadcast(intentMusic);
        }
    }
}