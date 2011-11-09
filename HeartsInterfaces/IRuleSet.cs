using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;

namespace HeartsInterfaces {
    public interface IRuleSet {
        string Name { get; }
        string Description { get; }

        Cards.Card StartCard { get; }

        bool IsPointCard(Card card);
        int PointValue(Card card);
        int[] CalculatePoints(IEnumerable<Card>[] takenCards);
        int[] AddPoints(int[] scores, int[] points, int roundNumber);
        bool IsGameOver(int[] scores, int numCompletedRounds);
        int[] GetPlaces(int[] scores);

    }
}
