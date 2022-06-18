using UnityEngine;

public interface IDamageable 
{
    LayerMask MyLayer { get; }

    bool CanBeDamaged { get; set; }

    void GetDamage(float value);
}
