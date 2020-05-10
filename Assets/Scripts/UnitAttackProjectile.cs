using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackProjectile : MonoBehaviour
{
    public float Damage = 2;
    public float ProjectileSpeed = 10;
    public GameObject OnDestroyEffect;
    private Unit targetUnit;

    public void Init(Unit targetUnit, UnitStats unitStats)
    {
        this.targetUnit = targetUnit;
        Damage *= unitStats.OutgoingDamageMultiplier;
    }

    private void FixedUpdate()
    {
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetUnit.transform.position, Time.fixedDeltaTime * ProjectileSpeed);

        // TODO use raycast instead so it triggers when it hits their body not the center
        if (Vector3.Distance(transform.position, targetUnit.transform.position) < 0.1f)
        {
            Trigger();
        }
    }

    private void Trigger()
    {
        targetUnit.TakeDamage(Damage);
        // Instantiate the on destroy effect and then remove it from the scene shortly after
        if (OnDestroyEffect != null)
        {
            Destroy(Instantiate(OnDestroyEffect, transform.position, Quaternion.identity), 5f);
        }

        // Destroy self
        Destroy(gameObject);
    }
}