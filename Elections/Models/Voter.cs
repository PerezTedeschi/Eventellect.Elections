using Elections.Interfaces;

namespace Elections.Models;

public record Voter(int Id, string Name) : IVoter;
