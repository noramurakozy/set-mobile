using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Find X SETs in a row without making a mistake
    public class XSetsInARowNoMistakes : Achievement
    {
        private int _setsCountCondition;
        public XSetsInARowNoMistakes(string text, int x) : base(text)
        {
            _setsCountCondition = x;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.MaxSetsFoundInARow >= _setsCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var setsInARowCountCategory = DifficultyUtils.CalculateSetsInARowCountCategory(_setsCountCondition);
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
    }
}