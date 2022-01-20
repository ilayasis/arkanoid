using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arkanoid.Listeners;
using Arkanoid.Adapters;
using Arkanoid.Model;

namespace Arkanoid.Activities
{
    [Activity(Label = "HighScoreActivity")]
    public class HighScoreActivity : Activity
    {
        ListView listViewHighscores;//דרך הליסט ניתן נתונים
        HighscoresEventListener highscoresEventListener;//ע"י זה נתחיל להאזין
        HighScoresAdapter highScoresAdapter;//יקבל את הנתנוים ויעביר לליסט וויו
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layoutHighScores);//מהו התוכן שלך?לייאוט הייגסכור
            this.listViewHighscores = this.FindViewById<ListView>(Resource.Id.listViewHighscores);
            this.highscoresEventListener = new HighscoresEventListener();
            this.highscoresEventListener.onHighscoresRetrieved += HighscoresEventListener_onHighscoresRetrieved;//נרשם 
        }

        //הפעולה שתקבל את הנתונים
        private void HighscoresEventListener_onHighscoresRetrieved(object sender, HighscoresEventListener.HighscoreEventArgs e)
        {
            List<User> usersHighScoresList = e.highscores;//הרשימה נשלחה לליסט
            this.highScoresAdapter = new HighScoresAdapter(this, usersHighScoresList);//נבנה את האדפטר ונשתמש ברשימה
            this.listViewHighscores.Adapter = this.highScoresAdapter;//הליסט משתמש בנתנוים של האדפטר
        }
    }
}