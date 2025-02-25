﻿using Elections.Interfaces;
using Elections.Models;

namespace Elections.Ballots;

public static class SingleVoteBallotFactory
{
    public static IReadOnlyList<ISingleVoteBallot> Create(IReadOnlyList<IVoter> voters, IReadOnlyList<ICandidate> candidates)
    {
        return voters.Select(x => CreateSimpleBallot(x, candidates)).ToList();
    }

    private static SimpleBallot CreateSimpleBallot(IVoter voter, IReadOnlyList<ICandidate> candidates)
    {
        var vote = CreateSimpleVote(voter, candidates);
        return new SimpleBallot(voter, vote);
    }

    private static SimpleVote CreateSimpleVote(IVoter voter, IReadOnlyList<ICandidate> candidates)
    {
        if (voter is ICandidate candidate)
            return new SimpleVote(candidate);

        var voterCandidate = Candidates.SelectRandom(candidates);
        return new SimpleVote(voterCandidate);
    }        
}