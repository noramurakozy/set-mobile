using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesInARowYHints : Achievement
    {
        public XGamesInARowYHints(string text, int[] conditions) : base(text)
        {
            
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            throw new System.NotImplementedException();
        }

        public override void CalculateDifficulty()
        {
            throw new System.NotImplementedException();
        }
    }
}