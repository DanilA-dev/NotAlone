using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

[RequireComponent(typeof(SphereCollider))]
public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItem _itemToPickUp;
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
        _interactor.FocusToInteractable(this.transform, AddToInventory);
    }

    private void AddToInventory()
    {
        if (_interactor == null)
            return;

        var equipmentSystem = _interactor.Interactor.GetComponentInChildren<EquipmentSystem>();
        equipmentSystem.AddItemToInventory(_itemToPickUp);
    }

    public async void DisableItemOnScene()
    {
        await Task.Delay(1000);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement interactor))
        {
            _pickUpText.gameObject.SetActive(true);
            _pickUpText.DOFade(1, 1).From(0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement interactor))
            _pickUpText.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovement interactor))
            _pickUpText.DOFade(0, 1).OnComplete(() => _pickUpText.gameObject.SetActive(false));
    }
}
