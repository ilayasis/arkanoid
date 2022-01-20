using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Arkanoid.Model;
using Android.Graphics;
using Android.Hardware;
namespace Arkanoid.Activities
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : AppCompatActivity,ISensorEventListener //מאזין לחישנים
    {
        GameView gameView;
        SensorManager SensorManager; //אחראי לנהל את החיישנים
        public string gameMode; //התכונה הזאת תהיה המוד שהתחלנו את האינטנט

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SupportActionBar.Hide();//מחביאים את המלבן שבמסך למעלה
            Point pointSreenSize = new Point();//נקודה
            this.WindowManager.DefaultDisplay.GetSize(pointSreenSize);// מביא לנו איקס ו ווי ומכניס לנקודה 
            this.gameView = new GameView(this, pointSreenSize.X, pointSreenSize.Y);//נותן לנו איקס וואי
            SensorManager = (SensorManager)GetSystemService(SensorService);// סיסיטם סרביב יביא לנו את השיורתים של הטלפון
            this.SetContentView(this.gameView);//תוכן המסך יהיה גאמ וויו
            gameMode = Intent.GetStringExtra("gameMode");//ה גאמ מוד הנוכחי
        }

        //פעולה של האינטרפס שלא נשתמש בה
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        //כל פעם שיש שינוי בחיישן הפעולה תקרא
        public void OnSensorChanged(SensorEvent e)
        {
            gameView.UpdatePaddle(-e.Values[0]);
        }

        //כל פעם שנעבור אקטיבטי נעבור מסך 
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (gameMode == "accelerometer")
            {
                //נרצה שסנסור מנאגר יפסיק להאזין
                SensorManager.UnregisterListener(this);
            }
            
        }

        //אחרי אונ דיזטורי נגיע לפעולה הזאת
        protected override void OnResume()
        {
            base.OnResume();
            if (gameMode=="accelerometer")//רק אם המשחק במוד של אקסס ניגש למוד אכסס
            {
                //נקשר בין האקטיביבי לבין האקסלומטר
                //הסנסור מנגר יקשר לנו בין הליסנדר לבין האקסלומטר
                SensorManager.RegisterListener(this, SensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Fastest);
            }
            
        }
    }
}