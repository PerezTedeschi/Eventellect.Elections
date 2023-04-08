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
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voter, new SimpleVote(_candidates[0]))
        };

        // Act
        var winner = _target.Run(ballots);

        // Assert
        Assert.AreEqual(_candidates[0], winner);
    }

    [TestMethod]
    public void Run_ShouldReturnWinner_WhenMultiplebBallots()
    {
        // Arrange
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voter, new SimpleVote(_candidates[0])),
            new SimpleBallot(_voter, new SimpleVote(_candidates[1])),
            new SimpleBallot(_voter, new SimpleVote(_candidates[1])),
            new SimpleBallot(_voter, new SimpleVote(_candidates[2]))
        };

        // Act
        var winner = _target.Run(ballots);

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
        var _ = _target.Run(ballots);
    }

    [TestMethod]
    [ExpectedException(typeof(ElectionException))]
    public void Run_ShouldThrowElectionException_WhenAllHaveEqualVotes()
    {
        // Arrange
        var ballots = new List<ISingleVoteBallot>
        {
            new SimpleBallot(_voter, new SimpleVote(_candidates[0])),
            new SimpleBallot(_voter, new SimpleVote(_candidates[1])),
            new SimpleBallot(_voter, new SimpleVote(_candidates[2]))
        };

        // Act
        var _ = _target.Run(ballots);
    }
}