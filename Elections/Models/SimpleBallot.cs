using Elections.Interfaces;

namespace Elections.Models;

public record SimpleBallot(IVoter Voter, IVote Vote) : ISingleVoteBallot;
