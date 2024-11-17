using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public void SetView(BaseEntityView view);
    void Execute();

    public event Action OnDieHandler;
}
