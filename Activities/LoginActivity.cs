using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Arkanoid.Model;

namespace Arkanoid.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        TextInputLayout emailLoginText;
        TextInputLayout passwordLoginText;
        Button buttonLoginEmailPassword;
        ProgressDialog ProgressDialog;
        User user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.layoutLogin);
            Initialize();
        }
        void Initialize()
        {
            this.emailLoginText = this.FindViewById<TextInputLayout>(Resource.Id.emailLoginText);
            this.passwordLoginText = this.FindViewById<TextInputLayout>(Resource.Id.passwordLoginText);
            this.buttonLoginEmailPassword = (Button)this.FindViewById(Resource.Id.buttonLoginEmailPassword);
            this.buttonLoginEmailPassword.Click += ButtonLoginEmailPassword_Click;
            this.ProgressDialog = new ProgressDialog(this);
        }
        private async void ButtonLoginEmailPassword_Click(object sender, EventArgs e)
        {
            string email = this.emailLoginText.EditText.Text;
            if (email == "")
            {
                Toast.MakeText(this, "You Should Enter Email", ToastLength.Short).Show();
            }
            string password = this.passwordLoginText.EditText.Text;
            if (password == "")
            {
                Toast.MakeText(this, "You Should Enter password", ToastLength.Short).Show();
            }
            this.ShowProgressDialogue("LogIn...");
            this.user = new User(email, password);
            if (await this.user.Login() == true)
            {
                Toast.MakeText(this, "Login completed", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Login failed", ToastLength.Short).Show();
                this.ProgressDialog.Dismiss();
                StartActivity(typeof(MainActivity));
                return;

            }
            this.ProgressDialog.Dismiss();
            ISharedPreferencesEditor editor = MainActivity.sp.Edit();
            editor.PutString("email",email);
            editor.PutString("password", password);
            editor.PutInt("highscore",await user.ReturnHighS());
            editor.Apply();
            StartActivity(typeof(MainActivity));
        }
        void ShowProgressDialogue(string status)
        {
            this.ProgressDialog = new ProgressDialog(this);
            ProgressDialog.SetCancelable(false);
            ProgressDialog.SetMessage(status);
            ProgressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            ProgressDialog.Show();
        }
       
    }
}