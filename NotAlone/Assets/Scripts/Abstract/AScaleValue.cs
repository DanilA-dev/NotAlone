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
    [SerializeField, Range(0, 10)] protected float _regenTime;
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

        if (_autoRegen)
            OnValueChanged += AutoRegen;
    }

    private void OnDestroy()
    {
        OnValueChanged -= AutoRegen;
    }

    private void AutoRegen(float value, float maxValue) // subscribe to Sprint input and DashInput to reset regenTimer
    {
        if(!_isRegenerating)
            StartCoroutine(LastValueChangeTimer());
    }

    private IEnumerator LastValueChangeTimer()
    {
        while(_currentRegenTime > 0)
        {
            _currentRegenTime -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(RegenValue(_regenAfterTime, _regenTime, _regenAmount));
    }

    protected IEnumerator RegenValue(float regetAfterTime,float time, float regenTime)
    {
        _isRegenerating = true;
        yield return new WaitForSeconds(regetAfterTime);
        for (float i = 0; i < time; i+= Time.deltaTime)
        {
            CurrentValue++;
            if(CurrentValue >= regenTime)
                break;

            yield return new WaitForEndOfFrame();
        }
        _isRegenerating = false;
    }
}
