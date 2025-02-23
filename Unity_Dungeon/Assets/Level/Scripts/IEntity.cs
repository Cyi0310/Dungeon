using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    int NowPosition { get; }

    void Execute();
    void SetNowPosition(int nowPosition);
    
    public event Action OnDieHandler;
}
