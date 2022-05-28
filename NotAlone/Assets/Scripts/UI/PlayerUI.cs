using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Texture2D _gamePlayCursor;
    [SerializeField] private Image _healthFill;
    [SerializeField] private Image _staminaFill;

    private HealthSystem _playerHealth;
    private StaminaSystem _playerStamina;

    private void Awake()
    {
        Cursor.SetCursor(_gamePlayCursor, new Vector2(_gamePlayCursor.width /2 , _gamePlayCursor.height/ 2), CursorMode.Auto);
    }

    private void OnDestroy()
    {
        if(_playerHealth)
            _playerHealth.OnValueChanged -= OnHealthChanged;

        if (_playerStamina)
            _playerStamina.OnValueChanged -= OnStaminaChanged;
    }

    public void InitPlayerValues(HealthSystem playerHealthSytem, StaminaSystem playerStaminaSystem)
    {
        _playerHealth = playerHealthSytem;
        _playerStamina = playerStaminaSystem;
        _playerHealth.OnValueChanged += OnHealthChanged;
        _playerStamina.OnValueChanged += OnStaminaChanged;
    }

    private void OnStaminaChanged(float curValue, float maxValue)
    {
        _staminaFill.fillAmount = curValue / maxValue;
    }

    private void OnHealthChanged(float curValue, float maxValue)
    {
        _healthFill.fillAmount = curValue / maxValue;
    }
}
