using Statistics;

namespace Achievements.AchievementTypes
{
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
            throw new System.NotImplementedException();
        }

        public override void CalculateDifficulty()
        {
            throw new System.NotImplementedException();
        }
    }
}