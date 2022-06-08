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

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                .ContinueWithOnMainThread(task => { });
            
            Debug.Log("Fetching data...");
            Task fetchTask =
                FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                    TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);
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
    }
}