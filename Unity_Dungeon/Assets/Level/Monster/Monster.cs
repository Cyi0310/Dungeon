using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : IEntity, IHealth
{
    private readonly HealthComponents healthComponents;

    public int atk = 100;

    private BaseEntityView view;

    public event Action OnDieHandler;
    public Monster()
    {
        healthComponents = new HealthComponents(new HealthComponents.Data()
        {
            MaxHealth = 100,
            Health = 100,
        });
        healthComponents.OnDieHandler += Die;
    }

    public void SetView(BaseEntityView view)
    {
        this.view = view;
    }

    public void Execute()
    {

    }

    public bool OnHeal(int value)
    {
        healthComponents.TakeHeal(value);
        return true;
    }

    public bool OnHit(int value)
    {
        healthComponents.TakeDamage(value);
        return true;
    }

    private void Die()
    {
        view.Die();
        OnDieHandler?.Invoke();
        Debug.Log("Die call view boom");
    }
}