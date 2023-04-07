using Elections.Interfaces;

namespace Elections.Models;

public record RankedChoiceVote(ICandidate Candidate, int Rank) : IRankedVote;
