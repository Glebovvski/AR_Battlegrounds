using System;
using Missiles;
using UnityEngine;

public class DamageReceiver : IDamageReceiver
{
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public DamageReceiver(int Health)
    {
        CurrentHealth = Health;
    }

    public event Action OnTakeDamage;
    public event Action OnDeath;

    public void TakeDamage(int value)
    {
        CurrentHealth -= value;
        if (CurrentHealth <= 0)
            OnDeath?.Invoke();
        else
            OnTakeDamage?.Invoke();
    }

    public void OnCollision(Collider other) 
    {
        other.TryGetComponent<Missile>(out var missile);
        if(missile == null) return;

        Debug.LogError("DAMAGE");
        TakeDamage(missile.Damage);
    }
}
