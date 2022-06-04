using System;
using Achievements.AchievementTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementUI : MonoBehaviour
    {
        [SerializeField] private DifficultyUI difficultyUI;
        [SerializeField] private TMP_Text textUI;
        [SerializeField] private Button btnDelete;
        [SerializeField] private RectTransform progressArea;
        private ProgressBar _progressBar;
        private TMP_Text _txtProgress;

        public Achievement Achievement { get; set; }

        private void Awake()
        {
            _progressBar = progressArea.GetComponentInChildren<ProgressBar>();
            _txtProgress = progressArea.GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            difficultyUI.SetDifficulty(Achievement.Difficulty);
            textUI.text = Achievement.Text;
            if (Achievement.HasProgress && Achievement.Status == Status.InProgress)
            {
                _progressBar.SetProgress(Achievement.GetProgressValue());
                _txtProgress.text = Achievement.GetProgressText();
            }
            else
            {
                progressArea.gameObject.SetActive(false);
            }

            if (Achievement.CreationType == CreationType.Default)
            {
                btnDelete.gameObject.SetActive(false);
            }
            else
            {
                btnDelete.onClick.AddListener(() => AchievementUIManager.Instance.ShowConfirmationDialog(this));
            }
        }
    }
}