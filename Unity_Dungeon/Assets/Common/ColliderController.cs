using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private OnHitDelegater[] onHitDelegaters;
    public event Action<BaseEntityView<IEntity>> OnHitHandler;
    public void Init(IHealth health)
    {
        foreach (var item in onHitDelegaters)
        {
            item.Init(health);
        }
    }
}
