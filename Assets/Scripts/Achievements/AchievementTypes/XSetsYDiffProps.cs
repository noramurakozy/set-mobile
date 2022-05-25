using Statistics;

namespace Achievements.AchievementTypes
{
    public class XSetsYDiffProps : Achievement
    {
        public XSetsYDiffProps(string text, int[] conditions) : base(text, conditions)
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