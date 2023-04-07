using Elections.Interfaces;

namespace Elections.Models;

public record RankedChoiceBallot(IVoter Voter, IReadOnlyList<IRankedVote> Votes) : IRankedBallot;
