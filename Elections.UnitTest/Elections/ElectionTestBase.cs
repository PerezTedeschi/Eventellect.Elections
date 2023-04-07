using Elections.Interfaces;
using Elections.Models;

namespace Elections.UnitTest.Elections
{
    public abstract class ElectionTestBase
    {
        protected readonly IReadOnlyList<ICandidate> _candidates = new List<ICandidate>
        {
            new Candidate(1, "Candidate 1"),
            new Candidate(2, "Candidate 2"),
            new Candidate(3, "Candidate 3")
        };

        protected readonly IReadOnlyList<IVoter> _voters = new List<IVoter>
        {
            new Voter(1, "Voter 1"),
            new Voter(2, "Voter 2"),
            new Voter(3, "Voter 3"),
            new Voter(4, "Voter 4")
        };
    }
}
