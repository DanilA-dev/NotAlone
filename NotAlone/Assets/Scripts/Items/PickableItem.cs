using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(SphereCollider))]
public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _itemToEquipment;
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private TMP_Text _pickUpText;

    private IInteractor _interactor;

    public float PickUpDistance => _pickUpDistance;

    private void Start()
    {
        _pickUpText.gameObject.SetActive(false);
    }

    public void Interact(IInteractor interactor)
    {
        _interactor = interactor;
        _interactor.FocusToInteractable(this.transform);
        AddItemToEquipment(interactor);
    }

    private void AddItemToEquipment(IInteractor interactor)
    {
        var equipmentSystem = interactor.GameObject.GetComponentInChildren<EquipmentSystem>();
        if (equipmentSystem)
            equipmentSystem.AddItem(_itemToEquipment, true);
    }

    public async void DisableItemOnScene()
    {
        await Task.Delay(1000);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractor interactor))
        {
            _pickUpText.gameObject.SetActive(true);
            _pickUpText.DOFade(1, 1).From(0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IInteractor interactor))
            _pickUpText.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractor interactor))
            _pickUpText.DOFade(0, 1).OnComplete(() => _pickUpText.gameObject.SetActive(false));
    }
}
