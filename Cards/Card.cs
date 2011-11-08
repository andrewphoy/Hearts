using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Cards {
    public class Card {

        private Rank myRank;
        private Suit mySuit;

        public Card(Suit suit, Rank rank) {
            mySuit = suit;
            myRank = rank;
        }

        public Card (Rank rank, Suit suit) {
            mySuit = suit;
            myRank = rank;
        }

        public Rank Rank { get { return myRank; } }
        public Suit Suit { get { return mySuit; } }

        public override bool Equals(object obj) {
            Card c = (Card)obj;
            if (c == null) {
                return false;
            }

            return c.Rank == this.Rank && c.Suit == this.Suit;
        }

        public override int GetHashCode() {
            return (int)myRank + 13*(int)mySuit;
        }

        public override string ToString() {
            return myRank.ToString() + " of " + mySuit.ToString();
        }

        public Bitmap Image {
            get {
                Bitmap bmp = (Bitmap) Cards.Properties.Resources.ResourceManager.GetObject("_" + this.Tag);
                bmp.MakeTransparent(bmp.GetPixel(0, 0));
                return bmp;
            }
        }

        public string Tag {
            get { return ((int)myRank).ToString() + mySuit.ToString().First(); }
        }

    }
}
