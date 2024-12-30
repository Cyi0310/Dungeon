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
        OnDieHandler?.Invoke();
    }

    public void Shield()
    {
        Debug.Log("Shield");
    }

    public void Move()
    {
        /*能以不同的鞋子走不同的步數*/
        Debug.Log($"Move: {NowPosition}");
        NowPosition++;
    }

    public void Execute()
    {
        Debug.Log("玩家站到TILE上了");
    }

    public void ResetToDefault()
    {
        NowPosition = 0;
    }

}
