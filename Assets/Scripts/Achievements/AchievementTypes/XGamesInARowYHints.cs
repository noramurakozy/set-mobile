using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games in a row using maximum Y hints
    public class XGamesInARowYHints : Achievement
    {
        private int _gamesCount;
        
        private int _gamesInARowCondition;
        private int _maximumHintsCondition;
        
        public XGamesInARowYHints(string text, int x, int y) : base(text)
        {
            _gamesInARowCondition = x;
            _maximumHintsCondition = y;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.HintsUsed <= _maximumHintsCondition)
            {
                _gamesCount++;
            }

            if (statistics.HintsUsed > _maximumHintsCondition)
            {
                _gamesCount = 0;
            }
            
            if (_gamesCount >= _gamesInARowCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(_gamesInARowCondition);
            var hintCountCategory = DifficultyUtils.CalculateHintCountCategory(_maximumHintsCondition);
            
            switch (gamesCountCategory)
            {
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.Medium:
                case ParameterCountCategory.Low when hintCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.Medium when hintCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Low:
                    Difficulty = Difficulty.Hard;
                    break;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High when hintCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }
    }
}