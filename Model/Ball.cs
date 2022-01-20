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
    public class Ball : Shape
    {
        private float radius;
        private int screenWidth;
        private int screenHight;
        public Ball(float radius, Point center, int xVelocity, int yVelocity, Color color, int screenWidth, int screenHight) : base(center.X, center.Y, color, xVelocity, yVelocity)
        {
            this.radius = radius;
            this.screenWidth = screenWidth;
            this.screenHight = screenHight;
        }

        //פעולת ציור העיגול
        public override void Draw(Canvas canvas)
        {
            canvas.DrawCircle(this.x, this.y, this.radius, this.paint);
        }

        //החזרת מהירות 
        public float GetYVelocity()
        {
            return this.speedy;
        }

        //החזרת רדיוס 
        public float GetRadius()
        {
            return this.radius;
        }

        //פעולת תוצאת הכדור לאחר פגיעה באחד מן המקומות
        public override bool Move(Canvas canvas)
        {
            //התחלנו את התנועה
            this.x += this.speedx;
            this.y += this.speedy;
            //פגענו באחד מהאופציות
            if (this.HitRightSide())
            {
                this.ReverseXVelocity();
            }
            else if (HitLeftSide())
            {
                this.ReverseXVelocity();
            }
            else if (HitUpSide())
            {
                this.ReverseYVelocity();
            }
            else if (HitDownSide())
            {
                return false;
            }
            return true;
        }
       
        //החזרת ערך בוליאני אם נוצרה פגיעה באחד מן הצדדים
        public bool HitRightSide()
        {
            if (this.x + radius >= this.screenWidth)
            {
                if (this.speedx > 0)//כיוון קיר ימני
                {
                    return true;
                }
            }
            return false;
        }
        public bool HitLeftSide()
        {
            if (this.x - radius <= 0)
            {
                if (speedx < 0)//כיוון קיר שמאלי
                {
                    return true;
                }
            }
            return false;
        }
        public bool HitUpSide()
        {
            if (this.y - radius <= 0)
            {
                if (this.speedy < 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HitDownSide()
        {
            if (this.y + radius >= this.screenHight)
            {
                return true;
            }
            return false;
        }

        //שינוי כיוון
        public void ReverseYVelocity()
        {
            this.speedy *= -1;
        }
        public void ReverseXVelocity()
        {
            this.speedx *= -1;
        }
    }
}