using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

namespace HeartsInterfaces {
    public interface IHeartsPlayer {

        IHeartsPlayer Instance { get;}
        IHeartsController HeartsController { get; set; }

        #region Notifications
        void NewGame(string[] playerNames);
        void RoundStart();
        void HandReady();
        void PassReceived(Card[] cards);
        void TrickStart(ITrick trick);
        void CardPlayed(Card card, int playerID, ITrick trick);
        void TrickEnd(ITrick trick);
        void RoundEnd(int[] scores, int[] points);
        void GameEnd(int place, int[] places, int score, int[] scores);
        void GameStopping(int currentScore, int[] currentScores, string reason);
        #endregion

        Card[] GetPass(PassDirection passDirection);
        Card GetCard(ITrick trick);
    }
}
