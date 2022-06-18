using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    [SerializeField] private LayerMask _whoIsGetDamaged;
    [SerializeField] private float _damagePerTick;
    [SerializeField] private float _timeBeforeDamage;

    private float _currentDamageTime;

    private void Start()
    {
        _currentDamageTime = _timeBeforeDamage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) 
            && damageable.MyLayer == _whoIsGetDamaged.value)
            Damage(damageable);
    }

    private void Damage(IDamageable damageable)
    {
        if (_currentDamageTime <= 0)
        {
            damageable.GetDamage(_damagePerTick);
            _currentDamageTime = _timeBeforeDamage;
        }    
        else
            _currentDamageTime -= Time.fixedDeltaTime;
    }
}
