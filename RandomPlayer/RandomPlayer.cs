using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeartsInterfaces;
using Cards;

namespace RandomPlayer {
    [HeartsPlayer("Random")]
    public class RandomPlayer : IHeartsPlayer {

        public RandomPlayer() {
            
        }

        public IHeartsPlayer Instance {
            get { return new RandomPlayer(); }
        }

        public IHeartsController HeartsController { get; set; }
        public IHeartsController HC { get { return this.HeartsController; } }

        #region Notifications
        public void NewGame(string[] playerNames) { }
        public void RoundStart() { }
        public void HandReady() { }
        public void PassReceived(Cards.Card[] cards) { }
        public void TrickStart(ITrick trick) { }
        public void CardPlayed(Cards.Card card, int playerID, ITrick trick) { }
        public void TrickEnd(ITrick trick) { }
        public void RoundEnd(int[] scores, int[] points) { }
        public void GameEnd(int place, int[] places, int score, int[] scores) { }
        public void GameStopping(int currentScore, int[] currentScores, string reason) { }
        #endregion

        public Card[] GetPass(PassDirection passDirection) {
            return HC.Hand.Take(3).ToArray();
        }

        public Card GetCard(ITrick trick) {
            return (Card)HC.Hand.Where(c => HC.IsLegalCard(c)).First();
        }
    }
}
