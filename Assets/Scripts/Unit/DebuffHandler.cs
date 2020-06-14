using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.VFX;

[Serializable]
public class DebuffHandler
{
    private Unit Unit;
    private List<Debuff> debuffs = new List<Debuff>();
    private float checkDebuffInterval = 0.1f;
    private UnityEvent onDebuffsChecked = new UnityEvent();

    public IEnumerator GetDebufsCheckedCoroutine(Unit unit, UnityAction onDebuffsChecked)
    {
        Unit = unit;
        // Set the event to call when a debuffs are checked
        this.onDebuffsChecked.RemoveAllListeners();
        this.onDebuffsChecked.AddListener(onDebuffsChecked);

        while (true)
        {
            // If the unit is dead, stop
            if (Unit.IsDead)
            {
                yield break;
            }

            float totalMovementSpeedModifier = 0f;
            // First reset all debuffs from the last interval
            ResetUnitStats();

            // Loop through each debuff and apply them
            foreach (Debuff debuff in debuffs)
            {
                if (!debuff.Enabled) continue;

                if (debuff.Damage != 0)
                {
                    Unit.TakeDamage(debuff.GetDamageInstance(checkDebuffInterval));
                }

                if (debuff.MovementSpeedModifier != 0)
                {
                    totalMovementSpeedModifier += debuff.MovementSpeedModifier;
                }

                if (debuff.ShouldModifyAttackTarget)
                {
                    Unit.SetTargetType(debuff.ModifiedAttackTarget);
                }
            }

            // Apply movement speed modifier after it has been calculated
            UpdateMovementSpeed(totalMovementSpeedModifier);

            // Remove any debuffs that have finished
            List<Debuff> debuffsToRemove = debuffs.FindAll(debuff => debuff.EndTime <= Time.time);

            debuffsToRemove.ForEach((debuff) =>
            {
                debuff.StopParticleEffect();
                debuffs.Remove(debuff);
            });

            this.onDebuffsChecked.Invoke();

            yield return new WaitForSeconds(checkDebuffInterval);
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        // If you can't charm this guy, return
        if (debuff.ShouldModifyAttackTarget && !Unit.UnitStats.CanModifyAttackTarget)
        {
            return;
        }

        // Set the debuff end time
        debuff.EndTime = Time.time + debuff.Duration;
        // Instantiate the particle effect if we have provided one. Destroy it 5 seconds later
        if (debuff.ParticleEffect != null)
        {
            debuff.InstantiatedParticleEffect = GameObject.Instantiate(debuff.ParticleEffect, Unit.TargetTransform);
        }
        debuffs.Add(debuff);
    }

    public void StopAllDebuffAnimations()
    {
        debuffs.ForEach((debuff) => debuff.StopParticleEffect());
    }

    private void ResetUnitStats()
    {
        Unit.UnitStats.ModifyMovementSpeed(0);
        Unit.UpdateNavMeshAgentSpeed();
        Unit.SetTargetType(null);
    }

    private void UpdateMovementSpeed(float amount)
    {
        Unit.UnitStats.ModifyMovementSpeed(amount);
        Unit.UpdateNavMeshAgentSpeed();
    }
}