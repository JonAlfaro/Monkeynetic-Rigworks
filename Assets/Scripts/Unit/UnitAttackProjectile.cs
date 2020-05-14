using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackProjectile : MonoBehaviour
{
    public float Damage = 2;
    public float ProjectileSpeed = 10;
    public GameObject OnDestroyEffect;
    public float ProjectileLifeTime = 0f;

    private Unit targetUnit;
    private Vector3 targetPosition;
    private float lifeTimeEnd;

    public void Init(Unit targetUnit, UnitStats unitStats)
    {
        this.targetUnit = targetUnit;
        targetPosition = targetUnit.transform.position;
        // Modify damage by the units outgoing damage multiplier
        Damage *= unitStats.OutgoingDamageMultiplier;

        lifeTimeEnd = ProjectileLifeTime != 0 ? Time.fixedTime + ProjectileLifeTime : Mathf.Infinity;
    }

    private void FixedUpdate()
    {
        // If the target unit hasn't been destroyed, update the target position to its current position
        if (targetUnit != null && targetUnit.transform != null)
        {
            targetPosition = targetUnit.TargetTransform.position;
        }

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime * ProjectileSpeed);

        // TODO use raycast instead so it triggers when it hits their body not the center
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Trigger();
            return;
        }

        if (Time.fixedTime >= lifeTimeEnd)
        {
            Trigger(false);
        }
    }

    private void Trigger(bool reachedTarget = true)
    {
        if (reachedTarget)
        {
            targetUnit?.TakeDamage(Damage);
        }

        // Instantiate the on destroy effect and then remove it from the scene shortly after
        if (OnDestroyEffect != null)
        {
            Destroy(Instantiate(OnDestroyEffect, transform.position, Quaternion.identity), 5f);
        }

        // Destroy self
        Destroy(gameObject);
    }
}