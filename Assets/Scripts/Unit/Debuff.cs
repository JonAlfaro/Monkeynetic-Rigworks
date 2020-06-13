using System;
using UnityEngine;
using UnityEngine.VFX;

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
    public GameObject InstantiatedParticleEffect { get; set; }

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

    public void StopParticleEffect()
    {
        if (InstantiatedParticleEffect != null)
        {
            VisualEffect debuffParticleVFX = InstantiatedParticleEffect.GetComponent<VisualEffect>();
            if (debuffParticleVFX != null)
            {
                debuffParticleVFX.SetVector2("Spawn Rate", new Vector2(0, 0));
            }

            InstantiatedParticleEffect.transform.SetParent(null, true);

            // Destroy the object 2 seconds after
            GameObject.Destroy(InstantiatedParticleEffect, 2f);
        }
    }
}