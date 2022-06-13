using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

namespace FirebaseHandlers
{
    public class FirebaseInitManager : MonoBehaviour
    {
        private void Start()
        {
            var defaults =  new Dictionary<string, object>
            {
                {"customAchievements", true},
                {"ab_test_running", true}
            };
            
            
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available) {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    var app = Firebase.FirebaseApp.DefaultInstance;
                    
                    // Messaging setup
                    InitializeFirebaseMessaging();

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                } else {
                    UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                .ContinueWithOnMainThread(task => { });
            
            Debug.Log("Fetching data...");
            Task fetchTask =
                FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                    TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        
        // End our messaging session when the program exits.
        public void OnDestroy() {
            Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
            Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
        }

        // Setup message event handlers.
        private void InitializeFirebaseMessaging() {
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            // Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(task => {
            //     LogTaskCompletion(task, "SubscribeAsync");
            // });
            Debug.Log("Firebase Messaging Initialized");

            // This will display the prompt to request permission to receive
            // notifications if the prompt has not already been displayed before. (If
            // the user already responded to the prompt, thier decision is cached by
            // the OS and can be changed in the OS settings).
            Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
                task => {
                    LogTaskCompletion(task, "RequestPermissionAsync");
                }
            );
            // isFirebaseInitialized = true;
        }

        void FetchComplete(Task fetchTask) {
            if (fetchTask.IsCanceled) {
                Debug.Log("Fetch canceled.");
            } else if (fetchTask.IsFaulted) {
                Debug.Log("Fetch encountered an error.");
            } else if (fetchTask.IsCompleted) {
                Debug.Log("Fetch completed successfully!");
            }

            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus) {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task =>
                        {
                            SetupRemoteConfigValues();
                            Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                        });

                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason) {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }
        }

        private static void SetupRemoteConfigValues()
        {
            RemoteConfigValueManager.Instance.CustomAchievements = FirebaseRemoteConfig.DefaultInstance
                .GetValue("customAchievements").BooleanValue;
            Debug.Log($"Value is set: CustomAchievements - {RemoteConfigValueManager.Instance.CustomAchievements}");
            RemoteConfigValueManager.Instance.IsABTestRunning = FirebaseRemoteConfig.DefaultInstance
                .GetValue("ab_test_running").BooleanValue;
            Debug.Log($"Value is set: IsABTestRunning - {RemoteConfigValueManager.Instance.IsABTestRunning}");
        }
        
        // Messaging setup
        public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
        {
            Debug.Log("Received Registration Token: " + token.Token);
        }

        public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        {
            Debug.Log("Received a new message");
            var notification = e.Message.Notification;
            if (notification != null) {
                Debug.Log("title: " + notification.Title);
                Debug.Log("body: " + notification.Body);
                var android = notification.Android;
                if (android != null) {
                    Debug.Log("android channel_id: " + android.ChannelId);
                }
            }
            if (e.Message.From.Length > 0)
                Debug.Log("from: " + e.Message.From);
            if (e.Message.Link != null) {
                Debug.Log("link: " + e.Message.Link.ToString());
            }
            if (e.Message.Data.Count > 0) {
                Debug.Log("data:");
                foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
                         e.Message.Data) {
                    Debug.Log("  " + iter.Key + ": " + iter.Value);
                }
            }
        }
        
        // Log the result of the specified task, returning true if the task
        // completed successfully, false otherwise.
        private bool LogTaskCompletion(Task task, string operation) {
            bool complete = false;
            if (task.IsCanceled) {
                Debug.Log(operation + " canceled.");
            } else if (task.IsFaulted) {
                Debug.Log(operation + " encounted an error.");
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
                    string errorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null) {
                        errorCode = String.Format("Error.{0}: ",
                            ((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
                    }
                    Debug.Log(errorCode + exception.ToString());
                }
            } else if (task.IsCompleted) {
                Debug.Log(operation + " completed");
                complete = true;
            }
            return complete;
        }
    }
}