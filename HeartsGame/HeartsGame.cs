using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeartsInterfaces;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using HeartsGame.RuleSets;

namespace HeartsGame {
    [Export]
    public class HeartsGame {

        private bool myIsGameStarted;
        private int myNumPlayers;
        private IRuleSet myRuleSet;
        private Object myPlayersLock = new Object();
        private List<PlayerController> myPlayers;
        private int[] myScores;
        private int[] myPlaces;
        private int myRoundNumber;
        private System.Threading.Thread myMasterThread;

        [ImportMany]
        private Lazy<IHeartsPlayer, IHeartsPlayerData>[] myAvailablePlayers { get; set; }
        private CompositionContainer container;

        public HeartsGame() {
            myIsGameStarted = false;
            myPlayers = new List<PlayerController>();
            myRoundNumber = 0;

            ComposePlayers();
        }

        private void ComposePlayers() {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Environment.CurrentDirectory));
            this.container = new CompositionContainer(catalog);
            this.container.ComposeParts(this);
        }

        public bool GameStarted {
            get { return myIsGameStarted; }
        }

        public IHeartsPlayer[] Players {
            get { return (from p in myPlayers select p.Player).ToArray(); }
        }

        public string[] AvailablePlayers {
            get {
                var result = from p in myAvailablePlayers
                             select p.Metadata.PlayerName;
                return result.ToArray();
            }
        }

        internal List<PlayerController> PlayerControllers {
            get { return myPlayers; }
        }

        public IRuleSet RuleSet {
            get { return myRuleSet; }
            set { myRuleSet = value; }
        }

        public int NumberPlayers {
            get { return myNumPlayers; }
        }

        public int RoundNumber {
            get { return myRoundNumber; }
        }

        public int[] Scores {
            get { return myScores.ToArray(); }
        }

        public int[] Places {
            get { return myPlaces; }
        }

        public bool AddPlayer(string playerName, ref string errstring) {
            if (!myIsGameStarted) {
                try {
                    // create the Player
                    foreach (var player in myAvailablePlayers) {
                        if (player.Metadata.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase)) {
                            // create the PlayerController
                            lock (myPlayersLock) {
                                PlayerController pc = new PlayerController(this, player.Value.Instance, myPlayers.Count, player.Metadata.PlayerName);
                                myPlayers.Add(pc);
                            }
                            return true;
                        }
                    }

                    // no players found
                    errstring = "No players found for '" + playerName + "'";
                    return false;

                } catch (Exception ex) {
                    errstring = ex.Message;
                    return false;
                }
            } else {
                errstring = "Game has already started";
                return false;
            }
        }

        public void StartGame() {
            myRuleSet = new StandardRuleSet();

            myMasterThread = System.Threading.Thread.CurrentThread;
            myNumPlayers = myPlayers.Count;

            myScores = new int[myNumPlayers];

            myIsGameStarted = true;
            string[] names = this.PlayerNames;
            myPlayers.ForEach(p => p.NewGame(names));
            DoLoop();
        }

        public string[] PlayerNames {
            get {
                string[] names;
                lock(myPlayersLock) {
                    names = (from p in myPlayers
                             select p.PlayerName).ToArray();
                }
                return names;
            }
        }

        private void DoLoop() {
            while (!this.IsGameOver) {
                myRoundNumber++;
                Round r = new Round(this);
                myPlayers.ForEach(p => p.RoundStart(r));
                
                r.Play();
                AddPoints(r.Points);
                myPlayers.ForEach(p => p.RoundEnd(myScores.ToArray(), r.Points));
            }

            myPlaces = myRuleSet.GetPlaces(myScores);
            for (int i = 0; i < myPlayers.Count; i++) {
                myPlayers[i].GameEnd(myPlaces[i], myPlaces, myScores[i], myScores);
            }

            //TODO notify the GUI that the game is over
        }

        private void AddPoints(int[] points) {
            myScores = myRuleSet.AddPoints(myScores, points, myRoundNumber);
        }

        private bool IsGameOver {
            get { return myRuleSet.IsGameOver(myScores, myRoundNumber); }
        }

        public void RequestShutdown(int playerID) {
            if (playerID == 0) {
                string reason = "Shutdown requested by Player 0";
                myPlayers.ForEach(p => p.GameStopping(myScores[p.PlayerID], myScores, reason));

                myMasterThread.Abort();
                System.Threading.Thread.CurrentThread.Abort();
            }
        }
    }

}
