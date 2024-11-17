using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : IEntity, IHealth
{
    public class Ability
    {
        public int WalkCount;
    }

    private HealthComponents healthComponents;

    public int NowPosition { get; private set; }
    public Ability MainAability { get; }

    private BaseEntityView view;

    public event Action OnDieHandler;

    public Character()
    {
        MainAability = new Ability()
        {
            WalkCount = 1,
        };

        healthComponents = new HealthComponents(new HealthComponents.Data()
        {
            MaxHealth = 100,
            Health = 100
        });
        healthComponents.OnDieHandler += OnDie;
    }

    public void SetData(int defaultPosition)
    {
        NowPosition = defaultPosition;
    }

    public void SetView(BaseEntityView view)
    {
        this.view = view;
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

    private void OnDie()
    {
        view.Die();
        OnDieHandler?.Invoke();
    }

    public void Shield()
    {
        Debug.Log("Shield");
    }

    public void Move()
    {
        /*��H���P���c�l�����P���B��*/
        Debug.Log($"Move: {NowPosition}");
        NowPosition++;
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

    public void Execute()
    {
        Debug.Log("���a����TILE�W�F");
    }

    public void ResetToDefault()
    {
        NowPosition = 0;
    }

}
