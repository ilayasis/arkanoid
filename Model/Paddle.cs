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
    public class Paddle : Shape
    {
        private float width;
        private float height;
        public Paddle(int screenWidth, int screenHigth, Color color, int speedx, int speedy) : base(screenWidth / 2 - screenWidth / 2, screenHigth - screenHigth /15, color, speedx, speedy)
        {
            width = screenWidth /3; //שלוש מטקות מקסימום ברוחב
            height = screenHigth / 30; // שלושים מטקות במקסימום אורך
        }

        //יביא את ממדי המלבן העוטף של המטקה
        public RectF GetRect()
        {
            return new RectF(x, y, x + width, y + height);
        }

        //נקבל נקודות ונמיר לאינט
        public int GetTopLeftY()
        {
            return (int)this.y;
        }
        public int GetWidth()
        {
            return (int)this.width;
        }

        //נקבל x
        public float GetCenterX()
        {
            return this.x + width / 2;
        }

        //שינוי בציר איקס  
        public void AddTopLeftX(float dx)
        {
            this.x += dx;
        }

        //המטקה והכדור מדברים בניהם
        public bool HitTheBall(Ball b)
        {
            float bRadius = b.GetRadius();
            if (b.Y + bRadius >= this.Y)
            {
                if (b.X >= this.x && b.X <= this.X + width) // מרכז העיגול,צריך להיות בין התחומים של המטקה
                {
                    if (b.GetYVelocity() > 0)  // רק שהמהירות כלפי מטה הכדור יפגע
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //ציור מלבן
        public override void Draw(Canvas canvas)
        {
            canvas.DrawRect(x, y, x + width, y + height, this.paint);
        }

        //--------------------------------------
        public override bool Move(Canvas canvas)
        { 
            return true;
        }
    }
}