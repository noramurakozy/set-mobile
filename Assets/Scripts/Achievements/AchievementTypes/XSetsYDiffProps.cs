using Statistics;

namespace Achievements.AchievementTypes
{
    public class XSetsYDiffProps : Achievement
    {
        public XSetsYDiffProps(Difficulty difficulty, string text, AchievementTemplate template) : base(difficulty, text, template)
        {
            
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            throw new System.NotImplementedException();
        }
    }
}