using Statistics;

namespace Achievements.AchievementTypes
{
    public class XGamesYHints : Achievement
    {
        private int _gamesCount;

        private readonly int _gamesCountCondition;
        private readonly int _hintsCountCondition;
        public XGamesYHints(Difficulty difficulty, string text, AchievementTemplate template, int gamesCountCondition, int hintsCountCondition) : base(difficulty, text, template)
        {
            _gamesCountCondition = gamesCountCondition;
            _hintsCountCondition = hintsCountCondition;
        }

        protected override void UpdateProgress(GameStatistics statistics)
        {
            if (statistics.HintsUsed <= _hintsCountCondition)
            {
                _gamesCount++;
            }

            if (_gamesCount >= _gamesCountCondition)
            {
                Status = Status.Complete;
            }
        }
    }
}