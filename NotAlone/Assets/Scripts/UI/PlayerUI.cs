using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image _healthFill;
    [SerializeField] private Image _staminaFill;
    [SerializeField] private CanvasGroup _statsGroup;
    [SerializeField] private TMP_Text _pickUpItemDescription;
    [SerializeField] private GameObject _notePanel;

    private HealthSystem _playerHealth;
    private StaminaSystem _playerStamina;

    public void InitPlayerValues(HealthSystem playerHealthSytem, StaminaSystem playerStaminaSystem)
    {
        _notePanel.SetActive(false);
        _playerHealth = playerHealthSytem;
        _playerStamina = playerStaminaSystem;
        _playerHealth.OnValueChanged += OnHealthChanged;
        _playerStamina.OnValueChanged += OnStaminaChanged;
        EquipmentSystem.OnItemCollect += OnItemEquip;
    }
    

    private void OnDestroy()
    {
        EquipmentSystem.OnItemCollect -= OnItemEquip;
        _playerHealth.OnValueChanged -= OnHealthChanged;
        _playerStamina.OnValueChanged -= OnStaminaChanged;
    }


    private void OnStaminaChanged(float curValue, float maxValue)
    {
        ShowStats();
        _staminaFill.fillAmount = curValue / maxValue;
    }

    private void OnHealthChanged(float curValue, float maxValue)
    {
        ShowStats();
        _healthFill.fillAmount = curValue / maxValue;
    }

    private void ShowStats()
    {
        var seq = DOTween.Sequence();
        seq.Append(_statsGroup.DOFade(1, 0.25f));
        seq.AppendInterval(5);
        seq.Append(_statsGroup.DOFade(0, 1));
    }

    private async void OnItemEquip(Item someItem)
    {
        if (someItem.ItemType == ItemType.Note)
        {
            await Task.Delay(1000);
            _notePanel.SetActive(true);
        }
        else
            CollectTween(someItem);
    }

    private void CollectTween(Item someItem)
    {
        _pickUpItemDescription.SetText(someItem.PickUpDescription);
        var seq = DOTween.Sequence();
        seq.Append(_pickUpItemDescription.DOFade(1, 1).From(0));
        seq.AppendInterval(3f);
        seq.Append(_pickUpItemDescription.DOFade(0, 1).From(0));
    }
    
}
