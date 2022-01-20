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
using Arkanoid.Activities;
using Arkanoid.Model;

namespace Arkanoid.Adapters
{
    class HighScoresAdapter : BaseAdapter<User>
    {
        Context context;//הדף שהליסט וויו נמצא בו
        List<User> highScoresUsers;//מקבל את הרשימה של השיאים

        //פעולה בונה
        public HighScoresAdapter(Context context, List<User> highScoresUsers)
        {
            this.context = context;
            this.highScoresUsers = highScoresUsers;
        } 
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((HighScoreActivity)context).LayoutInflater;//ניצור הפניה ע"י המרה
            View v = layoutInflater.Inflate(Resource.Layout.layoutHighScoreRow, parent, false);//מנפח לנו שורה לפי  לייאוט הייג..
            TextView UserName = v.FindViewById<TextView>(Resource.Id.textViewHighscoreUsername);//שתי הפניות
            TextView HighScore = v.FindViewById<TextView>(Resource.Id.textViewHighscoreHighscore);
            //הנתנוים נמצאים ביוזר וניגש אליה בעזרת הרשימה
            User usertoinflate = highScoresUsers[position];
            //ניקח מהיוזר את השם והייג סכור
            UserName.Text = usertoinflate.Name;
            HighScore.Text = usertoinflate.HighScore + "";
            return v;
        }
        public override int Count
        {
            get
            {
                return highScoresUsers.Count;
            }
        }//יביא לנו את אורך הרשימה
        public override User this[int position]
        {
            get
            {
                return this.highScoresUsers[position];
            }
        } //מחזיר יוזר במקום מסויים

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
    }
}