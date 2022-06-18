using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthSystem : AScaleValue, IDamageable
{
    [SerializeField] private LayerMask _myLayer;
    private bool _canBeDamaged = true;

    public bool CanBeDamaged { get => _canBeDamaged; set => _canBeDamaged = value; }
    public LayerMask MyLayer => _myLayer;

    public void GetDamage(float value)
    {
        if (!_canBeDamaged)
            return;

        CurrentValue -= value;
        Debug.Log("Damagin" + value);
        if (CurrentValue <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("ded");
    }
}
