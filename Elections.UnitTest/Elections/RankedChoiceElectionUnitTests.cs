using Elections.Elections;
using Elections.Execptions;
using Elections.Interfaces;
using Elections.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elections.UnitTest.Elections;

[TestClass]
public class RankedChoiceElectionUnitTests : ElectionTestBase
{
    private readonly RankedChoiceElection _target = new();

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenOnlyOneCandidate()
    {
        // Arrange
        var singleCandidateList = _candidates.Take(1).ToList();
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voters[0], new List<IRankedVote> { new RankedChoiceVote(singleCandidateList[0], 1) } )
        };

        // Act
        var winner = _target.Run(ballots, singleCandidateList);

        // Assert
        Assert.AreEqual(singleCandidateList[0], winner);
    }

    [TestMethod]
    public void Run_ShouldReturnWriteInWinner_WhenWinnerIsNotAnOfficialCandidate()
    {
        // Arrange
        var writeIn = new Candidate(100, "Candidate 100");
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voters[0], new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voters[1], new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),
            new RankedChoiceBallot(_voters[2], new List<IRankedVote> { new RankedChoiceVote(writeIn, 1) }),
            new RankedChoiceBallot(_voters[3], new List<IRankedVote> { new RankedChoiceVote(writeIn, 1) })
        };

        // Act
        var winner = _target.Run(ballots, _candidates);

        // Assert
        Assert.AreEqual(writeIn, winner);
    }

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenMultipleCandidates()
    {
        // Arrange
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voters[0], new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voters[1], new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),
            new RankedChoiceBallot(_voters[2], new List<IRankedVote> { new RankedChoiceVote(_candidates[2], 1) }),
            new RankedChoiceBallot(_voters[3], new List<IRankedVote> { new RankedChoiceVote(_candidates[2], 1) })
        };

        // Act
        var winner = _target.Run(ballots, _candidates);

        // Assert
        Assert.AreEqual(_candidates[2], winner);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Run_ShouldThrowArgumentOutOfRangeException_WhenNoBallots()
    {
        // Act
        var ballots = new List<IRankedBallot>();

        var _ = _target.Run(ballots, _candidates);
    }

    [TestMethod]
    [ExpectedException(typeof(ElectionException))]
    public void Run_ShouldThrowElectionException_WhenAllRemainingCandidatesHaveEqualVotes()
    {
        // Arrange
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voters[0], new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voters[1], new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voters[2], new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),
            new RankedChoiceBallot(_voters[3], new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) })
        };

        // Act
        var _ = _target.Run(ballots, _candidates);
    }
}