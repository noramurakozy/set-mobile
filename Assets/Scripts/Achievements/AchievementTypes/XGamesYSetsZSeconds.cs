using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesYSetsZSeconds : Achievement
    {
        public XGamesYSetsZSeconds(Difficulty difficulty, string text, AchievementTemplate template) : base(difficulty, text, template)
        {
            
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            throw new System.NotImplementedException();
        }
    }
}