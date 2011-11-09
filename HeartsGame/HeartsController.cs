using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeartsInterfaces;
using Cards;

namespace HeartsGame {
    internal class HeartsController : IHeartsController {

        #region Metadata
        public int PlayerID { get; internal set; }
        public Cards.Hand Hand { get; internal set; }
        public int NumberPlayers { get; internal set; }
        public string[] PlayerNames { get; internal set; }
        public int Score { get; internal set; }
        public int[] Scores { get; internal set; }
        public IRuleSet RuleSet { get; internal set; }
        #endregion

        #region Round Specific
        public PassDirection PassDirection { get; internal set; }
        public bool PlayingRound { get; internal set; }
        #endregion

        #region Trick Specific
        public ITrick Trick { get; internal set; }
        public bool MyTurn { get; internal set; }

        internal LegalCardDelegate LegalCardDelegate { private get; set; }
        internal LegalCardVerboseDelegate LegalCardVerboseDelegate { private get; set; }
        internal WillTakeTrickDelegate WillTakeTrickDelegate { private get; set; }

        public bool IsLegalCard(Card card) {
            return LegalCardDelegate(card);
        }

        public bool IsLegalCard(Card card, ref string errstring) {
            return LegalCardVerboseDelegate(card, ref errstring);
        }

        public bool WillTakeTrick(Card card) {
            return WillTakeTrickDelegate(card);
        }
        #endregion

    }
}
