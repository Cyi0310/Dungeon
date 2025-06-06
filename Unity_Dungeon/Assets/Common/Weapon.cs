using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IDanger
{
    public event Action OnTriggerEvent;
    private GameObject parent;
    private int atkValue;

    [SerializeField] private Collider myCollider;
    public void Init(GameObject parent, int atkValue)
    {
        parent = this.parent;
        this.atkValue = atkValue;
        SetColliderEnable(true);
    }

    public void SetColliderEnable(bool enable)
    {
        myCollider.enabled = enable;
    }

    public void Trigger()
    {
        OnTriggerEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<OnHitDelegater>(out var onHitDelegater) && parent != onHitDelegater)
        {
            onHitDelegater.OnHit(atkValue);
        }
    }
}
