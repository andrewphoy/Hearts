using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cards {
    public class Hand : List<Card>{
        public Hand() : base() { }
        public Hand(Hand hand) : base(hand) { }
        public Hand(IEnumerable<Card> cards) : base(cards) { }
        public Hand(int capacity) : base(capacity) { }

        public bool ContainsSuit(Suit suit) {
            return this.Any(c => c.Suit == suit);
        }

        public int SuitCount(Suit suit) {
            return this.Count(c => c.Suit == suit);
        }

        public bool OnlyContainsSuit(Suit suit) {
            return !this.Any(c => c.Suit != suit);
        }

        public Card CardByTag(string tag) {
            var cards = from c in this
                        where c.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase)
                        select c;
            if (cards.Count() == 0) {
                return null;
            } else {
                return (Card)cards.First();
            }
        }

        public string PrintCards() {
            string str = string.Empty;
            this.ForEach(c => str += c.ToString() + "\n");
            return str;
        }

        #region Sorting
        public void SortBySuit() {
            this.SortBySuit(Direction.Ascending);
        }

        public void SortBySuit(Direction direction) {
            this.Sort(new SuitComparer(direction));
        }

        public void SortByRank() {
            this.SortByRank(Direction.Ascending);
        }

        public void SortByRank(Direction direction) {
            this.Sort(new RankComparer(direction));
        }

        private class SuitComparer : IComparer<Card> {

            private Direction Direction { get; set; }

            public SuitComparer(Direction direction) {
                this.Direction = direction;
            }

            public int Compare(Card x, Card y) {
                if ((int)x.Suit > (int)y.Suit) {
                    return 1 * (int)Direction;
                } else if ((int)x.Suit < (int)y.Suit) {
                    return -1 * (int)Direction;
                } else {
                    // same suit
                    if ((int)x.Rank > (int)y.Rank) {
                        return 1 * (int)Direction;
                    } else if ((int)x.Rank < (int)y.Rank) {
                        return -1 * (int)Direction;
                    } else {
                        return 0;
                    }

                }
            }
        }

        private class RankComparer : IComparer<Card> {

            private Direction Direction { get; set; }

            public RankComparer(Direction direction) {
                this.Direction = direction;
            }

            public int Compare(Card x, Card y) {
                if ((int)x.Rank > (int)y.Rank) {
                    return 1 * (int)Direction;
                } else if ((int)x.Rank < (int)y.Rank) {
                    return -1 * (int)Direction;
                } else {
                    // same rank
                    if ((int)x.Suit > (int)y.Suit) {
                        return 1 * (int)Direction;
                    } else if ((int)x.Suit < (int)y.Suit) {
                        return -1 * (int)Direction;
                    } else {
                        return 0;
                    }

                }
            }
        }
        #endregion
    }


}
