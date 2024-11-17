using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HealthComponents
{
    public record Data
    {
        public int MaxHealth { get; init; }
        public int Health { get; init; }
    }

    private bool wasDie;

    private readonly int maxHealth;
    private int nowHealth;

    public event Action<int> OnTakeHeal;
    public event Action<int> OnTakeDamage;
    public event Action OnDieHandler;
    public HealthComponents(Data data)
    {
        maxHealth = data.MaxHealth;
        nowHealth = data.Health;
    }

    public void TakeHeal(int value)
    {
        if (wasDie)
        {
            return;
        }
        nowHealth += value;
        nowHealth = Math.Min(nowHealth, maxHealth);
        OnTakeHeal?.Invoke(nowHealth);
    }

    public void TakeDamage(int value)
    {
        Debug.Log($"TakeDamage NOW: {nowHealth}");
        nowHealth -= value;
        nowHealth = Math.Max(0, nowHealth);
        OnTakeDamage?.Invoke(nowHealth);
        Debug.Log($"TakeDamage NOW: {nowHealth}");

        if (nowHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDieHandler?.Invoke();
        wasDie = true;
    }

}
