using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SphereCollider))]
public class PickableItem : MonoBehaviour, IInteractable
{
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
        _interactor.IsFocusingInteractable = true;
        _interactor.FocusToInteractable(this.transform);
    }

    public void DestroyItemOnScene(float time)
    {
        Destroy(gameObject, time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractor interactor))
        {
            _pickUpText.gameObject.SetActive(true);
            _pickUpText.DOFade(1, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IInteractor interactor))
            _pickUpText.DOFade(0, 1).OnComplete(() => _pickUpText.gameObject.SetActive(false));
    }
}
