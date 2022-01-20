using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Arkanoid.Model;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        TextInputLayout textInputLayoutRegisterName;
        TextInputLayout textInputLayoutRegisterEmail;
        TextInputLayout textInputLayoutRegisterPassword;
        TextInputLayout textInputLayoutRegisterConfirm;
        Button buttonSubmitRegister;
        FirebaseAuth firebaseAuth;
        User user;
        ProgressDialog progressDialog;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layoutRegister);
            this.Initialize();
        }
        void Initialize()
        {
            this.textInputLayoutRegisterName = (TextInputLayout)this.FindViewById(Resource.Id.textInputLayoutRegisterName);
            this.textInputLayoutRegisterEmail = (TextInputLayout)this.FindViewById(Resource.Id.textInputLayoutRegisterEmail);
            this.textInputLayoutRegisterPassword = (TextInputLayout)this.FindViewById(Resource.Id.textInputLayoutRegisterPassword);
            this.textInputLayoutRegisterConfirm = (TextInputLayout)this.FindViewById(Resource.Id.textInputLayoutRegisterConfirm);
            this.buttonSubmitRegister = (Button)this.FindViewById(Resource.Id.buttonSubmitRegister);
            this.buttonSubmitRegister.Click += ButtonSubmitRegister_Click;
        }
        private async void ButtonSubmitRegister_Click(object sender, EventArgs e)
        {
            string name = this.textInputLayoutRegisterName.EditText.Text;
            if (name == "")
            {
                Toast.MakeText(this, "you should enter your name", ToastLength.Short).Show();
                return;
            }
            string email = this.textInputLayoutRegisterEmail.EditText.Text;
            if (email == "")
            {
                Toast.MakeText(this, "you should enter your email", ToastLength.Short).Show();
                return;
            }
            string password = this.textInputLayoutRegisterPassword.EditText.Text;
            if (password == "")
            {
                Toast.MakeText(this, "you should enter your password", ToastLength.Short).Show();
                return;
            }
            string confirm = this.textInputLayoutRegisterConfirm.EditText.Text;
            if (confirm == "")
            {
                Toast.MakeText(this, "you should enter your confirm", ToastLength.Short).Show();
                return;
            }
            if (password != confirm)
            {
                Toast.MakeText(this, "you should match your confirm", ToastLength.Short).Show();
                return;
            }
            this.ShowProgressDialogue("Registering............");
            this.user = new User(name, email, password,0);
            if (await this.user.Register() == true)
            {
                Toast.MakeText(this, "register success", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "register failed", ToastLength.Long).Show();
            }
            this.progressDialog.Dismiss();
            StartActivity(typeof(MainActivity));
        }
        void ShowProgressDialogue(string status)
        {
            this.progressDialog = new ProgressDialog(this);
            progressDialog.SetCancelable(false);
            progressDialog.SetMessage(status);
            progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            progressDialog.Show();
        }
    }
}