using Elections.Execptions;
using Elections.Interfaces;

namespace Elections.Elections;

public class PluralityElection : IElection<ISingleVoteBallot>
{
    public ICandidate Run(IReadOnlyList<ISingleVoteBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        ArgumentNullException.ThrowIfNull(ballots);
        ArgumentNullException.ThrowIfNull(candidates);

        if (ballots.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(ballots));

        var candidateVotes = ballots
                        .GroupBy(ballot => ballot.Vote.Candidate)
                        .ToDictionary(group => group.Key, group => group.Count());
        var maxVotes = candidateVotes.Max(v => v.Value);
        var winners = candidateVotes.Where(v => v.Value == maxVotes).Select(v => v.Key).ToList();

        if (!winners.Any())
            throw new ElectionException($"There no winners.");

        if (!winners.Count.Equals(1))
            throw new ElectionException($"There is a tie between {winners.Count} candidates.");

        return winners.First();
    }
}
