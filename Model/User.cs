using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Arkanoid.Helpers;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;

namespace Arkanoid.Model
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int HighScore { get; set; }
        FirebaseAuth firebaseAuthentication;
        FirebaseFirestore database;
        public const string COLLECTION_NAME = "users";
        public const string CURRENT_USER_FILE = "currentUserFile";
        public User()
        {
            this.firebaseAuthentication = AppDataHelper.GetFirebaseAuthentication();
            this.database = AppDataHelper.GetFirestore();
        }
        public User( string email, string password)
        {
            this.Name = "";
            this.Email = email;
            this.Password = password;
            this.firebaseAuthentication = AppDataHelper.GetFirebaseAuthentication();
            this.database = AppDataHelper.GetFirestore();
        }
        public User(string name, string email, string password,int HighScore)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.HighScore = HighScore;
            this.firebaseAuthentication = AppDataHelper.GetFirebaseAuthentication();
            this.database = AppDataHelper.GetFirestore();
        }
        public async Task<bool>Login() 
        {
            try
            {
                await this.firebaseAuthentication.SignInWithEmailAndPassword(this.Email, this.Password);
                var editor = Application.Context.GetSharedPreferences(CURRENT_USER_FILE, FileCreationMode.Private).Edit();  
                
                editor.PutString("email", this.Email);
                editor.PutString("password", this.Password);
                editor.PutString("fullname", this.Name);

                editor.Apply();
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                return false;
            }
            return true;
        }
        public async Task<bool> Logout()
        {
            try
            {
                var editor = Application.Context.GetSharedPreferences(User.CURRENT_USER_FILE, FileCreationMode.Private).Edit();
                editor.PutString("email", "");
                editor.PutString("password", "");
                editor.PutString("fullname", "");
                editor.Apply();
                firebaseAuthentication.SignOut();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public async Task<bool>Register()
        {
            try
            {
                await this.firebaseAuthentication.CreateUserWithEmailAndPassword(this.Email, this.Password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false; ;
            }
            try
            {
                //ניצור האש מאפ של השלושה הבאים
                HashMap userMap = new HashMap();
                userMap.Put("email", this.Email);
                userMap.Put("fullName", this.Name);
                userMap.Put("highscore", 0);
                //נשמור בכראנט יוזר שכרגע מחובר 
                DocumentReference userReference = this.database.Collection(COLLECTION_NAME).Document(this.firebaseAuthentication.CurrentUser.Uid);
                await userReference.Set(userMap);
            }
            catch 
            {

                return false;
            }
            return true;
        }

        //עומדים על המסד,מהמסד עוברים לאוסף של היוזרים ,הולכים לדוקיומנט של היוזר הנוכחי
        //,מביא הפניה ולאחר מכן נעשה עדכון להייג סקור הנוכחי
        public async Task<bool> UpdateHighScore()
        {
            try
            {
                this.database = AppDataHelper.GetFirestore();//מביא את המסד
                //נלך לקוליקשן של היוזר שלנו
                DocumentReference userReference = database.Collection(COLLECTION_NAME).Document(AppDataHelper.GetFirebaseAuthentication().CurrentUser.Uid);
                //נעשה עדכון להייג סכור
                await userReference.Update("highscore", this.HighScore);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public async Task<int> ReturnHighS()
        {
            DocumentReference userReference = this.database.Collection(COLLECTION_NAME).Document(this.firebaseAuthentication.CurrentUser.Uid);
            DocumentSnapshot documentSnapshot = (DocumentSnapshot)await userReference.Get();
            return (int)documentSnapshot.Get("highscore");
        }
    }
}