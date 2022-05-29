using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public abstract class AScaleValue : MonoBehaviour
{
    [SerializeField, Range(0, 100)] protected float _topValue;
    [SerializeField] private bool _autoRegen;
    [Space]
    [ShowIf("_autoRegen")]
    [SerializeField, Range(0, 10)] protected float _regenAfterTime;
    [ShowIf("_autoRegen")]
    [SerializeField, Range(0, 100)] protected float _regenTime;
    [ShowIf("_autoRegen")]
    [SerializeField] protected float _regenAmount;

    protected float _currentRegenTime;
    protected float _currentValue;
    private bool _isRegenerating;

    public event Action<float, float> OnValueChanged;


    public float CurrentValue
    {
        get => _currentValue;
        set
        {
            _currentValue = Mathf.Clamp(value, 0, _topValue);
            OnValueChanged?.Invoke(_currentValue, _topValue);
        } 
    }

    private void Awake()
    {
        _currentValue = _topValue;
    }

    public void RestRegenTimer()
    {
        _currentRegenTime = _regenAfterTime;
    }

    private void Update()
    {
        if (!_autoRegen)
            return;

        if (_currentRegenTime <= 0)
            RegenValue();
        else
            _currentRegenTime -= Time.deltaTime;
    }

    private void RegenValue()
    {
        if(_currentRegenTime <= 0 && CurrentValue < _regenAmount)
            CurrentValue += Time.deltaTime * _regenTime;
    }
}
