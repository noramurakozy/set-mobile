using System.Collections.Generic;
using System.Diagnostics;
using DefaultNamespace;
using GameScene.Statistics;

namespace GameScene
{
    public class GameData
    {
        public Deck Deck { get; set; }
        public List<SetCard> CardsOnTable { get; set; }
        public int ElapsedSeconds { get; set; }
        public GameStatistics GameStatistics { get; set; }

        public GameData(Deck deck, List<SetCard> cardsOnTable, int elapsedSeconds, GameStatistics gameStatistics)
        {
            Deck = deck;
            CardsOnTable = cardsOnTable;
            ElapsedSeconds = elapsedSeconds;
            GameStatistics = gameStatistics;
        }
    }
}