using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthSystem : AScaleValue, IDamageable
{
    private bool _canBeDamaged = true;
    public bool CanBeDamaged { get => _canBeDamaged; set => _canBeDamaged = value; }

    public void GetDamage(float value)
    {
        if (!_canBeDamaged)
            return;

        CurrentValue -= value;
        if (CurrentValue <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("ded");
    }
}
