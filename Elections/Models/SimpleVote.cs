using Elections.Interfaces;

namespace Elections.Models;

public record SimpleVote(ICandidate Candidate) : IVote;
