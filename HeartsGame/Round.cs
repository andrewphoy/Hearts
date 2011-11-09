using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;
using HeartsInterfaces;

namespace HeartsGame {
    internal class Round {
        private HeartsGame myGame;
        private Hand[] myHands;
        private Hand[] myTakenCards;
        private int[] myPoints;

        private bool myHeartsBroken;
        private bool myIsFirstTrick;
        private int myTrickStarter;

        private object passLock = new object();


        internal Round(HeartsGame game) {
            myGame = game;
            myTakenCards = new Hand[game.NumberPlayers];

            for (int i = 0; i < game.NumberPlayers; i++) {
                myTakenCards[i] = new Hand();
            }
        }

        internal List<PlayerController> PlayerControllers { get { return myGame.PlayerControllers; } }

        internal IRuleSet RuleSet { get { return myGame.RuleSet; } }

        public int NumberPlayers { get { return myGame.NumberPlayers; } }

        internal int[] Points {
            get { return myPoints; }
        }

        internal Hand[] Hands {
            get { return myHands; }
        }

        internal void Play() {
            PrepareHands();
            DoPass();
            PlayTricks();
        }

        #region PreTrickPrep
        private void PrepareHands() {
            Deck deck = new Deck();
            deck.Shuffle();
            this.myHands = deck.Deal(myGame.NumberPlayers);
            if (deck.Cards.Length > 0) {
                //TODO create a kitty and notify players
            }
            myGame.PlayerControllers.ForEach(pc => pc.HandReady());
        }

        private void DoPass() {
            PassDirection direction = this.PassDirection;
            // get delta
            int passDelta = (int)direction;
            if (passDelta == 0) {
                return;
            } else {
                // we need to do the pass
                // get the cards from each player
                List<Card[]> passes = new List<Card[]>();
                for (int i = 0; i < myGame.NumberPlayers; i++) {
                    passes.Add(null);
                }
                // the playerController will ensure that the pass cards are all legal cards
                // it will also remove them from the respective hand
                //TODO catch exceptions from a player controller and penalize that player
                foreach (PlayerController pc in myGame.PlayerControllers) {
                    passes[pc.PlayerID] = pc.GetPass(direction);
                }

                // now we have all the cards, give them to the players
                myGame.PlayerControllers.ForEach(
                    pc => pc.PassReceived(passes[(pc.PlayerID + passDelta + myGame.NumberPlayers) % myGame.NumberPlayers])
                );
            }
        }

        internal PassDirection PassDirection {
            get { return PassDirection.Left; }
        }
        #endregion

        #region Tricks
        private void PlayTricks() {
            myHeartsBroken = false;
            myIsFirstTrick = true;
            myTrickStarter = GetStartingPlayer();

            while (myHands[0].Count > 0) {
                PlayTrick();
            }

            // assign the points to the players
            myPoints = myGame.RuleSet.CalculatePoints(myTakenCards);
        }

        private void PlayTrick() {
            Trick t = new Trick(this, myIsFirstTrick, myHeartsBroken);

            t.Play(myTrickStarter);

            myTakenCards[t.Winner].AddRange(t.Cards);

            if (!myHeartsBroken && t.HasPointCards) {
                myHeartsBroken = true;
            }

            myTrickStarter = t.Winner;
            myIsFirstTrick = false;
        }

        private int GetStartingPlayer() {
            Card startCard = GetStartCard();
            for (int i = 0; i < myGame.NumberPlayers; i++) {
                if (myHands[i].Contains(startCard)) {
                    return i;
                }
            }
            throw new Exception("Could not find the start player");
        }

        internal Card GetStartCard() {
            return myGame.RuleSet.StartCard;
        }

        #endregion

    }
}
