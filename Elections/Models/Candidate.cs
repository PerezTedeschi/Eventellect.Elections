using Elections.Interfaces;

namespace Elections.Models;

public record Candidate(int Id, string Name): ICandidate, IVoter;
