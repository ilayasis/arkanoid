using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Arkanoid.Model
{
    //מחלקה שממנה לא ניצור עצמים
    public abstract class Shape
    {
        protected float x, y; //מיקום 
        protected Paint paint;//מברשת ציור
        protected Color color;//צבע צורה
        protected float speedx = 0; //x מהירות,וקטור
        protected float speedy = 0; //y מהירות,וקטור

        //יצירת צורה כללית
        public Shape(float x, float y, Color color, float speedx, float speedy)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.speedx = speedx;
            this.speedy = speedy;
            paint = new Paint();
            paint.Color = color;
        }

        //קבלת איקס ו וואי ושינוי שלהם
        public float X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }
        public float Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        //שתי פעולות רב צורתיות ,על מנת שכל צורה תצייר את עצמה ותזוז
        public abstract void Draw(Canvas canvas);
        public abstract bool Move(Canvas canvas);
    }
}