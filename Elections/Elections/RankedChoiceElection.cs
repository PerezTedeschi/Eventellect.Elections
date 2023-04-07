using Elections.Execptions;
using Elections.Interfaces;
using System.Diagnostics;

namespace Elections.Elections;

public class RankedChoiceElection : IElection<IRankedBallot>
{
    public ICandidate Run(IReadOnlyList<IRankedBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        ArgumentNullException.ThrowIfNull(ballots);
        ArgumentNullException.ThrowIfNull(candidates);

        if (ballots.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(ballots));
        
        var remainingCandidates = new List<ICandidate>(candidates);
        remainingCandidates.AddRange(ballots.SelectMany(ballot => ballot.Votes.Where(vote => !remainingCandidates.Contains(vote.Candidate)).Select(vote => vote.Candidate)));

        var round = 1;
        while(remainingCandidates.Count > 1)
        {
            Dictionary<ICandidate, int> currentVoteCounts = remainingCandidates.ToDictionary(c => c, c => 0);
            
            // Count the votes for each remaining candidate
            foreach (var ballot in ballots)
            {
                var highestRemainingChoice = ballot.Votes.OrderBy(v => v.Rank).FirstOrDefault(v => currentVoteCounts.ContainsKey(v.Candidate));
                if (highestRemainingChoice != null)
                {
                    currentVoteCounts[highestRemainingChoice.Candidate]++;
                }
            }

            Debug.WriteLine($"===================================================================================");
            Debug.WriteLine($"Round {round}");
            Debug.WriteLine($"Votes needed: {ballots.Count / 2.0 }");
            foreach (var currentVoteCount in currentVoteCounts.OrderByDescending(x => x.Value))
            {
                Debug.WriteLine($"{currentVoteCount.Key.Name} Votes:{currentVoteCount.Value}");
            }   

            // Check if any candidate has a majority of the votes
            var currentLeader = currentVoteCounts.FirstOrDefault(kv => kv.Value > currentVoteCounts.Sum(x => x.Value) / 2.0).Key;
            if (currentLeader != null)
            {
                Debug.WriteLine($"Candidate {currentLeader.Name} wins");
                return currentLeader;
            }

            Debug.WriteLine($"Total votes: {currentVoteCounts.Sum(x => x.Value)}");

            // Eliminate the candidate with the fewest votes
            var votesToEliminate = currentVoteCounts.OrderBy(kv => kv.Value).First().Value;            

            // Count the votes for each remaining candidate
            foreach (var currentVoteCount in currentVoteCounts)
            {
                if (currentVoteCount.Value.Equals(votesToEliminate))
                {
                    Debug.WriteLine($"Candidate {currentVoteCount.Key.Name} eliminated");
                    remainingCandidates.Remove(currentVoteCount.Key);
                }
            }            

            Debug.WriteLine($"===================================================================================");
            Debug.WriteLine($"");
            round++;

            if (remainingCandidates.Count.Equals(0))
            {
                throw new ElectionException($"There is a tie between {currentVoteCounts.Count} candidates.");
            }
        }

        return remainingCandidates.First();
    }
}
