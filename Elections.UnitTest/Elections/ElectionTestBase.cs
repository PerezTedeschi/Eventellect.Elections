using Elections.Interfaces;
using Elections.Models;

namespace Elections.UnitTest.Elections;

public abstract class ElectionTestBase
{
    protected readonly IReadOnlyList<ICandidate> _candidates = new List<ICandidate>
    {
        new Candidate(1, "Candidate 1"),
        new Candidate(2, "Candidate 2"),
        new Candidate(3, "Candidate 3")
    };
    protected readonly IVoter _voter = new Voter(1, "Voter 1");
}
