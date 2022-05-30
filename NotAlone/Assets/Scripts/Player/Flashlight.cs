using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Flashlight : Item
{
    [SerializeField] private TwoBoneIKConstraint _rigIK;

    public override void Equip()
    {
        base.Equip();
        _rigIK.weight = 1;
        this.gameObject.SetActive(true);
    }

    public override void UnEquip()
    {
        base.UnEquip();
        _rigIK.weight = 0;
        this.gameObject.SetActive(false);
    }
}
