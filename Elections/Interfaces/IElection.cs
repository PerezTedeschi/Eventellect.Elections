﻿namespace Elections.Interfaces;

public interface IElection<TBallot>
    where TBallot : IBallot
{
    ICandidate Run(IReadOnlyList<TBallot> ballots);
}