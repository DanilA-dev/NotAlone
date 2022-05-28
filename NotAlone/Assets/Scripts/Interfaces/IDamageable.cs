public interface IDamageable 
{
    bool CanBeDamaged { get; set; }

    void GetDamage(float value);
}
