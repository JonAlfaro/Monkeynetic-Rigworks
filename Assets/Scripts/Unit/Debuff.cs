using System;
using UnityEngine;

[Serializable]
public class Debuff
{
    public bool Enabled;
    public float Duration;
    public float Damage;
    public float MovementSpeedModifier;
    public GameObject ParticleEffect;
    [Header("Change the targets target unit type (charm type effect)")]
    public bool ShouldModifyAttackTarget;
    public UnitType ModifiedAttackTarget;
    // Not shown in inspector
    public float EndTime { get; set; }

    public float GetDamageInstance(float checkDebuffInterval)
    {
        if (Damage == 0)
        {
            return 0;
        }

        if (Duration == 0)
        {
            return Damage;
        }

        return Damage / (Duration / checkDebuffInterval);
    }
}