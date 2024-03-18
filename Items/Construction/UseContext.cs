using System;
using System.Collections.Generic;
using Godot;

public record UseContext
{
    public IBattleActor User { get; init; }

    public Node Target { get; init; }

    public Vector3 UseDirection { get; init; }
}