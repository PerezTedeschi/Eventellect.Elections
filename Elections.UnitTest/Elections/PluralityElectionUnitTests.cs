using Elections.Elections;
using Elections.Execptions;
using Elections.Interfaces;
using Elections.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elections.UnitTest.Elections;

[TestClass]
public class PluralityElectionUnitTests : ElectionTestBase
{
    private readonly PluralityElection _target = new PluralityElection();

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenOnlyOneBallot()
    {
        // Arrange
        var singleCandidateList = _candidates.Take(1).ToList();
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voters[0], new SimpleVote(_candidates[0]))
        };

        // Act
        var winner = _target.Run(ballots, singleCandidateList);

        // Assert
        Assert.AreEqual(_candidates[0], winner);
    }

    [TestMethod]
    public void Run_ShouldReturnWriteInWinner_WhenWinnerIsNotAnOfficialCandidate()
    {
        // Arrange
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voters[0], new SimpleVote(_candidates[0])),
            new SimpleBallot(_voters[1], new SimpleVote(new Candidate(100, "Candidate 100"))),
            new SimpleBallot(_voters[2], new SimpleVote(new Candidate(100, "Candidate 100")))
        };

        // Act
        var winner = _target.Run(ballots, _candidates);

        // Assert
        Assert.AreEqual(100, winner.Id);
    }

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenMultiplebBallots()
    {
        // Arrange
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voters[0], new SimpleVote(_candidates[0])),
            new SimpleBallot(_voters[1], new SimpleVote(_candidates[1])),
            new SimpleBallot(_voters[2], new SimpleVote(_candidates[1])),
            new SimpleBallot(_voters[3], new SimpleVote(_candidates[2]))
        };

        // Act
        var winner = _target.Run(ballots, _candidates);

        // Assert        
        Assert.AreEqual(_candidates[1], winner);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Run_ShouldThrowArgumentOutOfRangeException_WhenNoBallot()
    {
        // Arrange
        var ballots = new List<ISingleVoteBallot>();

        // Act
        var _ = _target.Run(ballots, _candidates);
    }

    [TestMethod]
    [ExpectedException(typeof(ElectionException))]
    public void Run_ShouldThrowElectionException_WhenAllHaveEqualVotes()
    {
        // Arrange
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voters[0], new SimpleVote(_candidates[0])),
            new SimpleBallot(_voters[1], new SimpleVote(_candidates[1])),
            new SimpleBallot(_voters[2], new SimpleVote(_candidates[2]))
        };

        // Act
        var _ = _target.Run(ballots, _candidates);
    }
}