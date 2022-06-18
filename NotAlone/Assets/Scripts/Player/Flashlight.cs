using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Flashlight : Item
{
    [SerializeField] private TwoBoneIKConstraint _rigIK;
    [SerializeField] private GameObject _defaultLightBeam;
    [SerializeField] private GameObject _chargedLightBeam;


    private PlayerInput _input;

    public override void Equip(PlayerController player)
    {
        _defaultLightBeam.SetActive(true);
        _chargedLightBeam.SetActive(false);
        _input = player.Input;
        _input.Player.FlashLightCharge.performed += PerformChargeLight;
        Equiped = true;
        this.gameObject.SetActive(true);
        _rigIK.weight = 1;
    }

    public override void UnEquip()
    {
        Equiped = false;
        _input.Player.FlashLightCharge.performed -= PerformChargeLight;
        this.gameObject.SetActive(false);
        _rigIK.weight = 0;
    }

    private void PerformChargeLight(InputAction.CallbackContext obj)
    {

    }
}
