using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Firestore;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Extensions;
using Arkanoid.Model;
using Arkanoid.Helpers;

namespace Arkanoid.Listeners
{
    public class HighscoresEventListener : Java.Lang.Object, IEventListener
    {
        public event EventHandler<HighscoreEventArgs> onHighscoresRetrieved;
        public HighscoresEventListener()
        {
            AppDataHelper.GetFirestore().Collection("users").AddSnapshotListener(this);//שמנו ליסנר לשינוים בענן לאוסף של היוזרים 
        }
        public class HighscoreEventArgs
        {
            public List<User> highscores;//רשימה של יוזרים בעלי שם משתמש וניקוד
        }
        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            QuerySnapshot qs = (QuerySnapshot)value;
            List<User> highscores = new List<User>();
            if (!qs.IsEmpty)
            {
                foreach (DocumentSnapshot doc in qs.Documents)
                {
                    User user = new User();//נבנה יוזר רייק
                    //ניקח את השיא ניקוד שלו ושם מלא
                    user.HighScore = (int)doc.Get("highscore");
                    user.Name = doc.GetString("fullName");
                    highscores.Add(user);//ונוסיף לרשימה של ההיגסקור
                }
            }
            highscores = highscores.OrderByDescending(x => x.HighScore).ToList();//נמיין את הרשימה לפי הניקוד
            //סיימנו למיין את הרשימה
            if (onHighscoresRetrieved != null)//אם נרשמו לאירוע
            {
                //אנחנו רוצים שכל מי שהזין לאירוע יקבל את ההיגסקור
                HighscoreEventArgs args = new HighscoreEventArgs();
                args.highscores = highscores;
                onHighscoresRetrieved.Invoke(this, args);
            }
        }
    }
}