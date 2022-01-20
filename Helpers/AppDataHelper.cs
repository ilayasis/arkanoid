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
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

namespace Arkanoid.Helpers
{
    public static class AppDataHelper
    {
        static ISharedPreferences preferences = Application.Context.GetSharedPreferences("userinfo", FileCreationMode.Private);
        static ISharedPreferencesEditor editor;
        static FirebaseFirestore database;
        public static FirebaseFirestore GetFirestore()
        {
            if (database!=null)
            {
                return database;
            }
            var app = FirebaseApp.InitializeApp(Application.Context);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("arkanoidpong")
                    .SetApplicationId("arkanoidpong")
                    .SetApiKey("AIzaSyBMoBo2IE1DHIZXbZrGKwhsA4wpXfpCo5I")
                    .SetDatabaseUrl("https://arkanoidpong.firebaseio.com")
                    .SetStorageBucket("arkanoidpong.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options, "FameliyShopList");
                //FirebaseApp.InitializeApp(context, options, "MarketList");
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }
        public static FirebaseAuth GetFirebaseAuthentication()
        {
            FirebaseAuth firebaseAuthentication;
            var app = FirebaseApp.InitializeApp(Application.Context);
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("arkanoidpong")
                    .SetApplicationId("arkanoidpong")
                    .SetApiKey("AIzaSyBMoBo2IE1DHIZXbZrGKwhsA4wpXfpCo5I")
                    .SetDatabaseUrl("https://arkanoidpong.firebaseio.com")
                    .SetStorageBucket("arkanoidpong.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(Application.Context, options);
                firebaseAuthentication = FirebaseAuth.Instance;
            }
            else
            {
                firebaseAuthentication = FirebaseAuth.Instance;
            }
            return firebaseAuthentication;
        }

        public static void SaveUserId(string userId)
        {
            editor = preferences.Edit();
            editor.PutString("userId", userId);
            editor.Apply();
        }

        public static string GetUserId()
        {
            string userId = "";
            userId = preferences.GetString("userId", "");
            return userId;
        }
    }
}