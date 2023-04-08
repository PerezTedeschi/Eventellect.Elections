using Elections;
using Elections.Ballots;
using Elections.Execptions;
using Elections.Elections;
using Elections.Interfaces;
using System.Diagnostics;

const int numVoters = 100_000;
var voters = Voters.Create(numVoters, Candidates.Official);

RunElection(new PluralityElection(), SingleVoteBallotFactory.Create(voters, Candidates.Official));
RunElection(new RankedChoiceElection(), RankedBallotFactory.Create(voters, Candidates.Official));

Console.WriteLine("Press any key to run again...");
Console.ReadKey();

static void RunElection<T>(IElection<T> election, IReadOnlyList<T> ballots) where T : IBallot
{
    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine($"========== {election.GetType().Name} ==========");
    Console.WriteLine();

    try
    {
        var winner = election.Run(ballots);
        Console.WriteLine(FormatMessage($"Winner is {winner?.Name}"));
    }
    catch (ElectionException electionException)
    {
        Console.WriteLine(FormatMessage(electionException.Message));
    }
    catch (Exception ex)
    {
        Console.WriteLine(FormatMessage(ex.ToString()));
    }

    Console.WriteLine();
    Console.WriteLine($"============================================");
    Console.WriteLine();

    string FormatMessage(string prefix)
        => $"{prefix} [{stopwatch!.Elapsed.TotalMilliseconds} ms]";
}