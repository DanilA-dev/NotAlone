using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Flashlight : Item
{
    [SerializeField] private TwoBoneIKConstraint _rigIK;
    [SerializeField] private DamageCollider _defaultLightBeam;
    [SerializeField] private DamageCollider _chargedLightBeam;


    public override void Equip(PlayerMovement player)
    {
        _defaultLightBeam.gameObject.SetActive(true);
        _chargedLightBeam.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        _rigIK.weight = 1;
    }

   
    public override void UnEquip()
    {
        this.gameObject.SetActive(false);
        _rigIK.weight = 0;
    }


}
