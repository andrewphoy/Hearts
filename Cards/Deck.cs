using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cards {
    public class Deck {

        private List<Card> myCards;

        public Deck() {
            myCards = new List<Card>();
            foreach (Suit s in Enum.GetValues(typeof(Suit))) {
                foreach (Rank r in Enum.GetValues(typeof(Rank))) {
                    myCards.Add(new Card(s, r));
                }
            }
        }

        public Card[] Cards {
            get { return myCards.ToArray(); }
        }

        public void Shuffle() {
            Random rnd = new Random();

            for (int i = myCards.Count - 1; i > 0; i--) {
                int swapIdx = rnd.Next(i + 1);
                Card temp = myCards[i];
                myCards[i] = myCards[swapIdx];
                myCards[swapIdx] = temp;
            }
        }

        public Hand[] Deal(int numPlayers) {
            Hand[] hands = new Hand[numPlayers];

            for (int i = 0; i < numPlayers; i++) {
                hands[i] = new Hand();
            }

            while (myCards.Count >= numPlayers) {
                for (int j = 0; j < numPlayers; j++) {
                    hands[j].Add(myCards[0]);
                    myCards.RemoveAt(0);
                }
            }

            // at this point we might still have cards left in the deck, that is fine
            // it is left to the calling program to distribute those cards

            return hands;
        }

    }
}
