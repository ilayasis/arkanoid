using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Arkanoid.Activities;
using Android.Content;
using Arkanoid.Model;
using Android.Views;
using Arkanoid.BroadcastService;

namespace Arkanoid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/ArkanoidAppPhoto")]
    public class MainActivity : AppCompatActivity
    {
        public User CurrentUser { get; set; } //היוזר שכרגע מחובר
        public static ISharedPreferences sp;
        
        public static bool first = true;
        Button MainButtonPlay;
        TextView MainTextViewScore;
        Button MainButtonRegister;
        Button MainButtonLogin;
        Button MainButtonShowHighScores;
        BroadcastService.MusicReciver musicReciver;
        int UserId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);         
            this.Initilaize();
        }
        //הפניות//
        public void Initilaize()
        {
            sp = this.GetSharedPreferences(User.CURRENT_USER_FILE, FileCreationMode.Private);//הפניה,יצירת שארפראפרנס
            this.MainButtonPlay = (Button)this.FindViewById(Resource.Id.MainButtonPlay);
            this.MainButtonPlay.Click += MainButtonPlay_Click;
            this.MainButtonRegister = (Button)this.FindViewById(Resource.Id.MainButtonRegister);
            this.MainButtonRegister.Click += MainButtonRegister_Click;
            this.MainTextViewScore = (TextView)this.FindViewById(Resource.Id.MainTextViewScore);
            this.MainButtonLogin = (Button)this.FindViewById(Resource.Id.MainButtonLogin);
            this.MainButtonLogin.Click += MainButtonLogin_Click;
            this.MainButtonShowHighScores = (Button)this.FindViewById(Resource.Id.MainButtonShowHighScores);
            this.MainButtonShowHighScores.Click += MainButtonShowHighScores_Click;
            this.musicReciver = new BroadcastService.MusicReciver(this);
            Intent inta = new Intent(this, typeof(BroadcastService.MusicService));
            //this.StopService(inta);
            //this.StartService(inta);
            if (first)
            {
                first = false;
                this.StartService(inta);
                
            }

        }
        public async void InitilaizeUser()
        {
            if (this.CurrentUser != null)//מחוברים
            {
                return;
            }
            //לא מחוברים
            string email = sp.GetString("email", "");//תביא לנו מהשארפראפרנס את האיימל ששמרת בפעם האחרונה,אחרת תביא כלום
            if (email != "")
            {
                //לך לשארפראפרנס ותביא לנו סיסמה ושם משתמש
                string pass = sp.GetString("password", "");
                string name = sp.GetString("fullname", "");
                int highScore = sp.GetInt("highscore", 0);
                this.CurrentUser = new User(name,email,pass,highScore);//ניצור יוסר
                if (await CurrentUser.Login() == true)//אם הצלחנו להתחבר
                {
                    this.MainButtonLogin.Text = "התנתק";//התחבר יהפוך להתנתק
                    this.MainButtonRegister.Visibility = ViewStates.Gone;//כפתור הרשם יעלם
                    this.SetButtonsEnabled(true);//נפעיך את הכפתורים
                }
                else//אם לא הצלחנו להתחבר
                {
                    //הכל ישאר אותו הדבר
                    this.MainButtonLogin.Text = "התחבר";
                    this.MainButtonRegister.Visibility = ViewStates.Visible;
                    this.SetButtonsEnabled(false);
                }
            }
        }
        private async void MainButtonLogin_Click(object sender, System.EventArgs e)
        {
            if (MainButtonLogin.Text == "התנתק")//עכשיו מתנתקים
            {
                await this.CurrentUser.Logout();//נלך ליוזר הנוכחי ונגיד לו להתנתק
                this.CurrentUser = null;//נבטל את היוזר
                this.MainButtonLogin.Text = "התחבר";//נשנה את הכיתוב להתחבר
                this.MainButtonRegister.Visibility = ViewStates.Visible;//נראה אותו
                this.SetButtonsEnabled(false);//כל הכפתורים יהיו חסומים
                return;
            }
            //אם היינו במצב התחברות,נלך להתחברות
            else
            {
                this.StartActivity(typeof(LoginActivity));
            }
        }
        private void MainButtonRegister_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterActivity));
            this.StartActivity(intent);
        }

        //עדכון כפתורים
        private void SetButtonsEnabled(bool state)
        {
            this.MainButtonPlay.Enabled = state;
        }
        protected override void OnPause()
        {
            base.OnPause();
            this.UnregisterReceiver(this.musicReciver);
        }

        //מתבצע לאחר האון כריאט
        //בהמתנה
        protected override void OnResume()
        {
            base.OnResume();
            InitilaizeUser();//נפעיל את הפעולה
            if (this.CurrentUser == null)
            {
                this.MainButtonLogin.Text = "התחבר";
                this.MainButtonRegister.Visibility = ViewStates.Visible;
                this.SetButtonsEnabled(false);
            }
            else
            {
                this.MainButtonLogin.Text = "התנתק";
                this.MainButtonRegister.Visibility = ViewStates.Gone;
                this.SetButtonsEnabled(true);
            }
            this.RegisterReceiver(this.musicReciver, new IntentFilter("com.hufsha.MusicService"));

        }
        //ע"י לחיצה על לחצן זה נתחיל לשחק
        private void MainButtonPlay_Click(object sender, System.EventArgs e)
        {
            //במקום שישר נתחיל לשחק ניצור דיאלוג שבו נבחר באיזה אופציה נעדיף לשחק
            Dialog dialogPlay = new Dialog(this);
            dialogPlay.SetTitle("Which GameType You Prefer ?");
            dialogPlay.SetContentView(Resource.Layout.layoutDialogPlay);
            dialogPlay.SetCancelable(true);//אם נלחץ בצד של הדילוג ,יבטל את הדילוג
            dialogPlay.Show();//כל פעם שנלחץ על  לשחק זה יקפיץ לנו את הלייאוט של הדילוג
            Button buttonManual = (Button)dialogPlay.FindViewById(Resource.Id.buttonDialogManual);
            Button buttonAccelerometer = (Button)dialogPlay.FindViewById(Resource.Id.buttonDialogAccelerometer);
            buttonManual.Click += (senderManual, args) => //
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("gameMode", "manual");//מצב משחק
                this.StartActivityForResult(intent, 100);
                dialogPlay.Dismiss();
            };
            buttonAccelerometer.Click += (senderAcc, args) =>
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("gameMode", "accelerometer");//מצב משחק
                this.StartActivityForResult(intent, 100);
                dialogPlay.Dismiss();
            };        
        }
        protected async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)//האם התשובה טובה
            {
                if (requestCode == 100)
                {
                    int currentGameScore = data.GetIntExtra("score", 0);//שיא
                    //נשמור שיאים בענן
                    this.MainTextViewScore.Text = string.Format("Your Score : {0}", currentGameScore.ToString());
                    if (currentGameScore > sp.GetInt("highscore",0))//נבדוק אם הישא הנוכחי עבר את השיא
                    {
                        var editor = sp.Edit();//נעדכן בשארפראפרנדס
                        editor.PutInt("highScore", currentGameScore);
                        editor.Apply();
                        CurrentUser.HighScore = currentGameScore;
                        await CurrentUser.UpdateHighScore();
                    }
                }
            }
        }
        private void MainButtonShowHighScores_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(HighScoreActivity));//לך להייגסכור אקטיבטי
            this.StartActivity(intent);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)//יצירת התפריט שהגדרנו,ניפוחה למסך
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_Exit)
            {
                //Intent intent = new Intent(this, typeof(BackGroundMusicService));
                //StopService(intent);
                //Process.KillProcess(Process.MyPid());
                //this.Finish();
                //return true;
                this.FinishAffinity();
            }
            if (item.ItemId == Resource.Id.action_About)
            {
                Android.Support.V7.App.AlertDialog.Builder mine = new Android.Support.V7.App.AlertDialog.Builder(this);
                mine.SetTitle("About Us");
                mine.SetMessage("Arkanoid is a 1986 block breaker arcade game developed and published by Taito. In North America, it was published by Romstar.");

                mine.SetPositiveButton("Ok...", (senderDialog, eDialog) =>
                {
                    Toast.MakeText(this, "LET'S GO !!! ", ToastLength.Long).Show();

                });
                {

                }
                Dialog d = mine.Create();
                d.Show();

            }
            if (item.ItemId == Resource.Id.action_Music)
            {
                Intent intent = new Intent(this, typeof(BackGroundMusicService));
                Intent intent2 = new Intent(this, typeof(MusicService));
                if (BackGroundMusicService.NowIsPlaying)
                {
                    StopService(intent);
                    item.SetTitle("Play Song");

                }
                else if (MusicService.NowIsPlaying)
                {
                    StopService(intent2);
                    item.SetTitle("Play Song");
                }
                else if (!MusicService.NowIsPlaying && !BackGroundMusicService.NowIsPlaying)
                {
                    StartService(intent);
                    item.SetTitle("Pause Song");
                }
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}