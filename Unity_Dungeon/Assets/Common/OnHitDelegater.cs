using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitDelegater : MonoBehaviour, IHealthAdapter
{
    private IHealth health;
    public void Init(IHealth health)
    {
        this.health = health;
    }

    public bool OnHeal(int value)
    {
        return health.OnHeal(value);
    }

    public bool OnHit(int value)
    {
        return health.OnHit(value);
    }
}
