using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

namespace HeartsGame {
    internal delegate bool LegalCardDelegate(Card card);
    internal delegate bool LegalCardVerboseDelegate(Card card, ref string errstring);
    internal delegate bool WillTakeTrickDelegate(Card card);

}
