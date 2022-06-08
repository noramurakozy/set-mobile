using System;
using System.Collections.Generic;
using Achievements;
using Achievements.AchievementTypes;
using Dialogs;
using Firebase.Analytics;
using GameScene.GUtils;
using GameScene.Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSummaryScene
{
    public class GameSummaryManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtTitle;
        [SerializeField] private TMP_Text txtNewBestTime;
        [SerializeField] private TMP_Text txtTimePlayed;
        [SerializeField] private TMP_Text txtHintsUsed;
        [SerializeField] private TMP_Text txtShufflesUsed;
        [SerializeField] private TMP_Text txtMistakes;
        [SerializeField] private Button btnPlayAgain;
        [SerializeField] private Button btnHome;
        [SerializeField] private Fader fader;

        private void Start()
        {
            fader.EnterSceneAnimation();
            var latestGameStatistics = GameStatisticsManager.Instance.GameStatistics;
            
            txtTitle.text = FeedbackUtils.GetRandomPositiveEndGameFeedback();
            if (UserStatisticsManager.Instance.UserStatistics.IsNewBestTime)
            {
                txtNewBestTime.gameObject.SetActive(true);
                txtNewBestTime.text =
                    "NEW BEST TIME - " 
                    + Utils.GetTimeSpanString(TimeSpan.FromSeconds(latestGameStatistics.DurationInSeconds));
            }
            else
            {
                txtNewBestTime.gameObject.SetActive(false);
            }

            txtTimePlayed.text = Utils.GetTimeSpanString(TimeSpan.FromSeconds(latestGameStatistics.DurationInSeconds));
            txtHintsUsed.text = latestGameStatistics.HintsUsed.ToString();
            txtShufflesUsed.text = latestGameStatistics.ShufflesUsed.ToString();
            txtMistakes.text = latestGameStatistics.MistakesCount.ToString();
            btnPlayAgain.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "GameSummaryScene"), 
                    new Parameter("to", "GameScene"));
                fader.ExitSceneAnimation("GameScene");
            });
            btnHome.onClick.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent("switch_scene", 
                    new Parameter("from", "GameSummaryScene"), 
                    new Parameter("to", "MainMenu"));
                fader.ExitSceneAnimation("MainMenu");
            });
            AchievementManager.Instance.UpdateProgressOfAchievements(latestGameStatistics, UpdateType.EndOfGame);
        }
    }
}