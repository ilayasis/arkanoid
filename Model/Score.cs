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
    public class Score : Shape //יורש משאיפ מכיוון שגם לו יש צורה..
    {
        private int hits;
        public Score(int screenWidth, int screenHigth, Color color, int textSize) : base(screenWidth / 2 - 50, screenHigth / 2, color, 0, 0)
        {
            this.hits = 0;
            this.paint.TextSize = textSize;
        }

        //מאפיין פגיעות
        public int Hits
        {
            get
            {
                return this.hits;
            }
            set
            {
                this.hits = value;
            }
        }

        //ציור מילה + מיקום
        public override void Draw(Canvas canvas)
        {
            canvas.DrawText(string.Format("Score: {0}", this.hits), this.x, this.y, this.paint);
        }

        //ציור נקוודת ע"י מוב
        public override bool Move(Canvas canvas)
        {
            this.Draw(canvas);
            return true;
        }
    }
}