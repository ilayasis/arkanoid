using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Arkanoid.Activities;

namespace Arkanoid.Model
{
    //עליו יהיו כל האיברים
    public class GameView : SurfaceView
    {
        private Shapes shapes; //התכונה תביא לנו את כל הצורות
        private bool threadRunning = true; // יציר לנו את העדכונים,רץ ברקע
        Paint paint; // עצם  של ציור
        private Thread gameThread; // 
        int n;
        private int screenWidth;
        private int screenHight;
        //המדיה פלייר יעזור לנו להשמיע צלילים במהלך המשחק
        MediaPlayer hitTheBallSound;
        MediaPlayer misTheBallSound;
       
        

        //פעולה בונה
        //קונטסט הוא המסך שעליו כל הציורים יצוירו
        public GameView(Context context, int screenWidth, int screenHight) : base(context)
        {
            this.n = 3; // כמות כדורים
            paint = new Paint(); // יצירת צייר
            paint.Color = Color.Blue;
            paint.StrokeWidth = 20; //עובי קו
            this.screenWidth = screenWidth; 
            this.screenHight = screenHight;
            //יצירת שאייפס
            this.shapes = new Shapes(this.n, screenWidth, screenHight);
            //יצירת מדיה פלייאר
            this.hitTheBallSound = MediaPlayer.Create(((GameActivity)this.Context), Resource.Raw.bathit);
            this.misTheBallSound = MediaPlayer.Create(((GameActivity)this.Context), Resource.Raw.explosion);
            this.gameThread = new Thread(this.Run);
            this.gameThread.Start();
        }

        //הפעולה תעזור לנו להריץ את הטראד ברקע
        public void Run()
        {
            while (this.threadRunning)
            {
                if (this.Holder.Surface.IsValid) // הולדר אחרי על הכאנבס,מתפעל אותו.יס ואליד זה האם המשטח פנוי
                {
                    Canvas canvas = this.Holder.LockCanvas();//נכין את הכנבאס וננעל אותו על מנת להשתמש בו
                    if (canvas != null)
                    {
                        canvas.DrawColor(Color.White);//צבע רקע של הכנבאס
                                                      //הצורות יזוזו ויצירו את עצמם על המסך
                        shapes.MoveShapes(canvas);
                        shapes.DrawShapes(canvas);
                        if (shapes.CollisionPaddle())//בודק אם הכדור פגע בפדאל
                        {
                            this.hitTheBallSound.Start();
                        }
                        if (shapes.IsToucingDownDide())//בדיקה אם הייתה פגיעה בחלק התחתון של המסך
                        {
                            this.threadRunning = false;
                            this.misTheBallSound.Start();
                        }
                    }
                    this.Holder.UnlockCanvasAndPost(canvas);//נשחרר את הכנבאס
                }
            }
            Intent intent = new Intent();
            intent.PutExtra("score", this.shapes.GetScore());//
            ((GameActivity)this.Context).SetResult(Result.Ok, intent);
            ((GameActivity)this.Context).Finish();
        }
        //נגיעה במסך עם האצבע
        public override bool OnTouchEvent(MotionEvent e)
        {
            if (((GameActivity)Context).gameMode=="manual")//נבדוק אם אנחנו במצב מנואל
            {
                switch (e.Action)//איזה פעולה עשינו
                {
                    case MotionEventActions.Move://אם האצבע זזה
                        float dx = e.GetX() - this.shapes.GetPaddle().GetCenterX();//כמה זזנו
                        this.shapes.GetPaddle().AddTopLeftX(dx);//ציר האיקס של הפדל יזוז
                        break;
                    case MotionEventActions.Down:
                        break;
                    case MotionEventActions.Up:

                        break;
                }
            }
            return true;
        }

        //כל פעם שיהיה שינוי בחיישן נקרא לפעולה
        public void UpdatePaddle(float x)
        {
            //האיקס יעדכן את מיקום המטקה
            Paddle paddle = shapes.GetPaddle();
            //כל פעם שיש שינוי באיקס נעדכן את המיקום
            if (paddle.X + x > 0 && paddle.X + x < screenWidth - paddle.GetWidth())//נרצה שהמטקה לא תצא מגבולות המסך במצב אקסס
            {
                paddle.AddTopLeftX(x);
            }

        }
    }
}