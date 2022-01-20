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
    public class Shapes
    {
        List<Shape> list; // רשימה של כל השאייפים 
        int n; // מספר של הצורות
        private Paddle paddle;
        private Score score;
        public Shapes(int n, int screenWidth, int screenHight)
        {
            this.list = new List<Shape>();
            this.n = n;
            this.CreateShapes(screenWidth, screenHight);// לפי מימדי הלוח
        }
        public Paddle GetPaddle()
        {
            return this.paddle;
        }

        //יצירת צורות
        private void CreateShapes(int screenWidth, int screenHight)
        {
            this.score = new Score(screenWidth, screenHight, Color.Blue, 50);//ניצור score
            this.paddle = new Paddle(screenWidth, screenHight, Color.Black, 0, 0);//ניצור את המטקה
            Random random = new Random();
            //מאיזה מהירויות נתחיל 
            int xVelocity = 7;
            int yVelocity = 7;
            // לולאה שתרוץ איין פעמים,איין הוא מס הכדורים
            for (int i = 0; i < this.n; i++)
            {
                int radius = screenWidth / 30; //מקסימום 15 כדורים
                int x = random.Next(radius, screenWidth - radius);//האיקס של הכדור בין הרדיוס שלו לבין הרוחב פחות הרדיוס
                int y = random.Next(radius, screenHight / 2);//הוואי של הכדור בין הרדיוס לבין הגובה חלקי 2
                Point circleCenter = new Point(x, y);//נקודת מרכז
                Color color = Color.Argb(255, random.Next(256), random.Next(256), random.Next(256));//צבע אקראי
                Ball ball = new Ball(radius, circleCenter, xVelocity, yVelocity, color, screenWidth, screenHight); //יצירת כדור
                this.list.Add(ball);
                //מהירות כפול 2
                xVelocity *= 2;
                yVelocity *= 2;
            }
        }

        //ניקוד לפי פגיעות במטקה
        public int GetScore()
        {
            return this.score.Hits;
        }

        //הפעולה בודקת אם אחד הכדורים מהרשימה פגעה למטה
        public bool IsToucingDownDide()
        {
            foreach (Ball ball in this.list)
            {
                if (ball.HitDownSide())
                {
                    return true;
                }
            }
            return false;
        }

        //הפעולה בודקת אם אחד הכדורים פגעה בפדל 
        public bool CollisionPaddle()
        {
            foreach (Ball ball in this.list)
            {
                if (this.paddle.HitTheBall(ball))
                {
                    ball.ReverseYVelocity();
                    this.score.Hits++;
                    return true;
                }
            }
            return false;
        }

        //הפעולה תצייר פדל וניקוד וכן את הצורות שבליסט
        public void DrawShapes(Canvas canvas)
        {
            this.paddle.Draw(canvas);
            this.score.Draw(canvas);
            foreach (Shape shape in this.list)
            {
                shape.Draw(canvas);
            }
        }

        //הפעולה תזיז את הצורה,עדכון
        public void MoveShapes(Canvas canvas)
        {
            foreach (Shape shape in this.list)
            {
                shape.Move(canvas);
            }
        }
    }
}