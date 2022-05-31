using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Find X SETs in a row without making a mistake or using hints
    public class XSetsInARowNoMistakesNoHints : Achievement
    {
        public int setsCountCondition;
        public XSetsInARowNoMistakesNoHints(CreationType creationType, string text, int x) : base(text, creationType)
        {
            setsCountCondition = x;
            UpdateType = UpdateType.DuringGame;
            HasProgress = false;
        }

        public override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.MaxSetsFoundInARow >= setsCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var setsInARowCountCategory = DifficultyUtils.CalculateSetsInARowCountCategory(setsCountCondition);
            switch (setsInARowCountCategory)
            {
                case ParameterCountCategory.Low:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High:
                    Difficulty = Difficulty.Hard;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }

        // Has no progress
        public override float GetProgressValue()
        {
            return 0.0f;
        }

        public override string GetProgressText()
        {
            return "";
        }
    }
}