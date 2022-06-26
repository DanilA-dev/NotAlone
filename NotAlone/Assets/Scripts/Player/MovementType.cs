using System;
using UnityEngine;

[Serializable]
public class MovementType
{
    public enum SpeedType
    {
        Walk,
        Run,
        Sprint
    }

    [field: SerializeField] public SpeedType Type { get; private set; }
    [field : SerializeField, Range(0,10000)] public float Acceleration { get; private set; }
    [field: SerializeField, Range(0, 100)] public float StaminaPerSec { get; private set; }
    public float Friction { get; private set; } = 500;

    public void ReduceStamina(StaminaSystem stamina)
    {
        if (StaminaPerSec <= 0)
            return;

        stamina.CurrentValue -= StaminaPerSec * Time.deltaTime;
    }
}
