using Elections.Execptions;
using Elections.Interfaces;

namespace Elections.Elections;

public class RankedChoiceElection : IElection<IRankedBallot>
{
    public ICandidate Run(IReadOnlyList<IRankedBallot> ballots)
    {
        ArgumentNullException.ThrowIfNull(ballots);        

        if (ballots.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(ballots));

        var remainingCandidates = ballots.SelectMany(b => b.Votes).GroupBy(v => v.Candidate).Select(g => g.Key).ToList();        

        while (remainingCandidates.Count > 1)
        {
            var currentVoteCounts = CountVotes(ballots, remainingCandidates);
            var winner = currentVoteCounts.FirstOrDefault(w => w.Value > currentVoteCounts.Sum(c => c.Value) / 2.0).Key;
            if (winner != null)
            {
                return winner;
            }

            EliminateCandidates(remainingCandidates, currentVoteCounts);;

            if (remainingCandidates.Count.Equals(0))
            {
                throw new ElectionException($"There is a tie between {currentVoteCounts.Count} candidates.");
            }
        }

        return remainingCandidates.First();
    }

    private static void EliminateCandidates(List<ICandidate> remainingCandidates, Dictionary<ICandidate, int> currentVoteCounts)
    {
        var votesToEliminate = currentVoteCounts.Values.Min();
        var candidatesToEliminate = currentVoteCounts.Where(v => v.Value == votesToEliminate).Select(c => c.Key).ToList();
        remainingCandidates.RemoveAll(candidatesToEliminate.Contains);
    }

    private static Dictionary<ICandidate, int> CountVotes(IReadOnlyList<IRankedBallot> ballots, IList<ICandidate> remainingCandidates)
    {
        var currentVoteCounts = remainingCandidates.ToDictionary(c => c, c => 0); 
        foreach (var ballot in ballots)
        {
            var highestRemainingChoice = ballot.Votes.OrderBy(v => v.Rank).FirstOrDefault(v => currentVoteCounts.ContainsKey(v.Candidate));
            if (highestRemainingChoice != null)
            {
                currentVoteCounts[highestRemainingChoice.Candidate]++;
            }
        }

        return currentVoteCounts;
    }
}
