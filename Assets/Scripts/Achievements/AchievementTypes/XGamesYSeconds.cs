using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesYSeconds : Achievement
    {
        public XGamesYSeconds(string text, int[] conditions) : base(text, conditions)
        {
            
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            throw new System.NotImplementedException();
        }

        protected override Difficulty CalculateDifficulty(int[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}