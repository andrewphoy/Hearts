using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

namespace HeartsInterfaces {
    public interface ITrick {
        // trick metadata
        bool HeartsBroken { get; }
        bool IsFirstTrick { get; }

        // cards on the table
        Card[] Cards { get; }
        bool HasPointCards { get; }
        Card HighCard { get; }
        Card[] PlayerCards { get; }
        int? Starter { get; }
        Suit? Suit { get; }
        int? TrickLeader { get; }
    }
}
