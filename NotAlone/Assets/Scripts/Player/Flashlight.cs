using UnityEngine;
using UnityEngine.Animations.Rigging;


public class Flashlight : Item
{
    [SerializeField] private TwoBoneIKConstraint _rigIK;

    public override void Equip()
    {
        this.gameObject.SetActive(true);
        _rigIK.weight = 1;
    }

    public override void UnEquip()
    {
        this.gameObject.SetActive(false);
        _rigIK.weight = 0;
    }
}
