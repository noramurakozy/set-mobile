using System;
using EasyUI.Dialogs;
using FirebaseHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Statistics
{
    public class StatisticsUIManager : MonoBehaviour
    {
        // General statistics
        [SerializeField] private TMP_Text txtTotalGamesCount;
        [SerializeField] private TMP_Text txtBestTime;
        [SerializeField] private TMP_Text txtTimeInTotal;
        [SerializeField] private TMP_Text txtAvgTimePerSet;
        [SerializeField] private TMP_Text txtAvgTimePerGame;
        [SerializeField] private TMP_Text txtAvgMistakesPerGame;
        [SerializeField] private TMP_Text txtAvgHintsPerGame;
        [SerializeField] private TMP_Text txtShufflesPerGame;
        
        // SET related statistics
        [SerializeField] private TMP_Text txtNumSets1DiffProp;
        [SerializeField] private TMP_Text txtNumSets2DiffProp;
        [SerializeField] private TMP_Text txtNumSets3DiffProp;
        [SerializeField] private TMP_Text txtNumSets4DiffProp;
        [SerializeField] private TMP_Text txtSetsInTotal;
        
        // Achievement related statistics
        [SerializeField] private TMP_Text txtNumHardAchievements;
        [SerializeField] private TMP_Text txtNumMediumAchievements;
        [SerializeField] private TMP_Text txtNumEasyAchievements;
        [SerializeField] private TMP_Text txtAchievementsInTotal;
        [SerializeField] private TMP_Text txtCustomAchievementsCount;
        
        [SerializeField] private Button btnClearStats;
        [SerializeField] private Button btnHome;
        [SerializeField] private ConfirmDialogUI confirmDialogUI;
        [SerializeField] private Fader fader;

        private void Start()
        {
            fader.EnterSceneAnimation();
            UserStatisticsManager.Instance.UpdateAchievementStatistics();
            btnClearStats.onClick.AddListener(ShowConfirmationDialog);
            btnHome.onClick.AddListener(() => fader.ExitSceneAnimation("MainMenu"));
            SetupTxtFields();
        }

        private void ClearStatistics()
        {
            UserStatisticsManager.Instance.ClearStats();
            SetupTxtFields();
        }

        private void SetupTxtFields()
        {
            var userStatistics = UserStatisticsManager.Instance.UserStatistics;
            
            txtTotalGamesCount.text = userStatistics.TotalGameCount.ToString();
            txtBestTime.text = GUtils.Utils.GetTimeSpanString(userStatistics.BestTime);
            txtTimeInTotal.text = GUtils.Utils.GetTimeSpanString(userStatistics.TotalTime);
            txtAvgTimePerSet.text = GUtils.Utils.GetTimeSpanString(userStatistics.AvgTimePerSet);
            txtAvgTimePerGame.text = GUtils.Utils.GetTimeSpanString(userStatistics.AvgTimePerGame);
            txtAvgMistakesPerGame.text = userStatistics.AvgMistakesPerGame.ToString();
            txtAvgHintsPerGame.text = userStatistics.AvgHintsPerGame.ToString();
            txtShufflesPerGame.text = userStatistics.AvgShufflesPerGame.ToString();

            txtNumSets1DiffProp.text = userStatistics.NumSets1DiffProp.ToString();
            txtNumSets2DiffProp.text = userStatistics.NumSets2DiffProp.ToString();
            txtNumSets3DiffProp.text = userStatistics.NumSets3DiffProp.ToString();
            txtNumSets4DiffProp.text = userStatistics.NumSets4DiffProp.ToString();
            txtSetsInTotal.text = userStatistics.TotalSetCount.ToString();

            txtNumHardAchievements.text = userStatistics.NumHardAchievements.ToString();
            txtNumMediumAchievements.text = userStatistics.NumMediumAchievements.ToString();
            txtNumEasyAchievements.text = userStatistics.NumEasyAchievements.ToString();
            txtAchievementsInTotal.text = userStatistics.AchievementsUnlockedInTotal.ToString();

            if (RemoteConfigValueManager.Instance.CustomAchievements)
            {
                txtCustomAchievementsCount.text = userStatistics.CustomAchievementsCount.ToString();
            }
            else
            {
                txtCustomAchievementsCount.transform.parent.gameObject.SetActive(false);
            }
        }

        private void ShowConfirmationDialog()
        {
            confirmDialogUI.gameObject.SetActive(true);
            confirmDialogUI
                .SetTitle("Clear statistics")
                .SetMessage(
                    "Are you sure you want clear your statistics? " +
                    "Please note, that this action won't delete the achievement statistics " +
                    "as it always represents the current state of your achievement progress.")
                .SetNegativeButtonText("Yes, clear")
                .SetPositiveButtonText("No, keep my data")
                .SetButtonsColor(DialogButtonColor.Green)
                .SetFadeDuration(0.1f)
                .OnNegativeButtonClicked(ClearStatistics)
                .Show();
        }
    }
}