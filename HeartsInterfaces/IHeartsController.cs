using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

namespace HeartsInterfaces {
    public interface IHeartsController {
        // metadata
        int PlayerID { get; }
        Hand Hand { get; }
        int Score { get; }
        int[] Scores { get; }
        IRuleSet RuleSet { get; }

        // round specific
        PassDirection PassDirection { get; }
        bool PlayingRound { get; }

        // trick specific
        ITrick Trick { get; }
        bool MyTurn { get; }
        bool IsLegalCard(Card card);
        bool IsLegalCard(Card card, ref string errstring);
        bool WillTakeTrick(Card card);
    }
}
