using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Flashlight : Item
{
    [SerializeField] private TwoBoneIKConstraint _rigIK;
    [SerializeField] private DamageCollider _defaultLightBeam;
    [SerializeField] private DamageCollider _chargedLightBeam;


    private PlayerInput _input;

    public override void Equip(PlayerController player)
    {
        _defaultLightBeam.gameObject.SetActive(true);
        _chargedLightBeam.gameObject.SetActive(false);
        _input = player.Input;
        _input.Player.FlashLightCharge.performed += PerformChargeLight;
        _input.Player.FlashLightCharge.canceled += CancelChargeLight;
        this.gameObject.SetActive(true);
        _rigIK.weight = 1;
    }

   
    public override void UnEquip()
    {
        _input.Player.FlashLightCharge.performed -= PerformChargeLight;
        _input.Player.FlashLightCharge.canceled -= CancelChargeLight;
        this.gameObject.SetActive(false);
        _rigIK.weight = 0;
    }

    private void PerformChargeLight(InputAction.CallbackContext obj)
    {
        Debug.Log("Charge");
    }

    private void CancelChargeLight(InputAction.CallbackContext obj)
    {
        Debug.Log("Cancel");
    }

}
