using Achievements.Difficulties;
using GameScene.Statistics;
using Statistics;

namespace Achievements.AchievementTypes
{
    // Complete X games with maximum Y mistakes 
    public class XGamesYMistakes : Achievement
    {
        private int _gamesCount;
        
        private int _gamesCountCondition;
        private int _mistakesCountCondition;
        
        public XGamesYMistakes(string text, int x, int y) : base(text)
        {
            _gamesCountCondition = x;
            _mistakesCountCondition = y;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.MistakesCount <= _mistakesCountCondition)
            {
                _gamesCount++;
            }

            if (_gamesCount >= _gamesCountCondition)
            {
                Status = Status.Complete;
            }
        }

        public override void CalculateDifficulty()
        {
            var gamesCountCategory = DifficultyUtils.CalculateGameCountCategory(_gamesCountCondition);
            var mistakesCountCategory = DifficultyUtils.CalculateMistakesCountCategory(_mistakesCountCondition);
            
            switch (gamesCountCategory)
            {
                case ParameterCountCategory.Low when mistakesCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Low when mistakesCountCategory == ParameterCountCategory.Medium:
                case ParameterCountCategory.Low when mistakesCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.Medium when mistakesCountCategory == ParameterCountCategory.Low:
                case ParameterCountCategory.Medium when mistakesCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.Medium when mistakesCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                case ParameterCountCategory.High when mistakesCountCategory == ParameterCountCategory.Low:
                    Difficulty = Difficulty.Hard;
                    break;
                case ParameterCountCategory.High when mistakesCountCategory == ParameterCountCategory.Medium:
                    Difficulty = Difficulty.Medium;
                    break;
                case ParameterCountCategory.High when mistakesCountCategory == ParameterCountCategory.High:
                    Difficulty = Difficulty.Easy;
                    break;
                default:
                    Difficulty = Difficulty.Medium;
                    break;
            }
        }
    }
}