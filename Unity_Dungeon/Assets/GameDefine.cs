using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDoAction
{
    public readonly Action Action;
    public readonly float WaitDuration;
    public WaitDoAction(Action action, float waitDuration)
    {
        Action = action;
        WaitDuration = waitDuration;
    }
}

public record SpawnEntityComponent(EntityData EntityData, IEntity tileEntity);

public enum ActiveType
{
    Left,
    Front,
    Right,
}

public enum EntityType
{
    Empty,

    Player,

    Monster,
}
