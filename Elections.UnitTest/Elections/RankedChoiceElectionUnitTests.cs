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
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) } )
        };

        // Act
        var winner = _target.Run(ballots);

        // Assert
        Assert.AreEqual(_candidates[0], winner);
    }

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenMultipleCandidatesWithRank1Only()
    {
        // Arrange
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[2], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[2], 1) })
        };

        // Act
        var winner = _target.Run(ballots);

        // Assert
        Assert.AreEqual(_candidates[2], winner);
    }

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenMultipleCandidatesWithMultiRankChoise()
    {
        // Arrange
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),            
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),            
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[2], 1), new RankedChoiceVote(_candidates[1], 2) })
        };

        // Act
        var winner = _target.Run(ballots);

        // Assert
        Assert.AreEqual(_candidates[1], winner);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Run_ShouldThrowArgumentOutOfRangeException_WhenNoBallots()
    {
        // Act
        var ballots = new List<IRankedBallot>();

        var _ = _target.Run(ballots);
    }

    [TestMethod]
    [ExpectedException(typeof(ElectionException))]
    public void Run_ShouldThrowElectionException_WhenAllRemainingCandidatesHaveEqualVotes()
    {
        // Arrange
        var ballots = new List<IRankedBallot>
        {
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[0], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) }),
            new RankedChoiceBallot(_voter, new List<IRankedVote> { new RankedChoiceVote(_candidates[1], 1) })
        };

        // Act
        var _ = _target.Run(ballots);
    }
}