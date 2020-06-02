using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;

// TODO target enemies at random, closest to the core, with the lowest health etc.
public enum UnitAttackTargetingType { Closest }

[Serializable]
public class UnitAttackTargeting
{
    public UnitType TargetUnitType;
    public UnitAttackTargetingType UnitAttackTargetingType;
    public float CheckForValidTargetInterval = 0.1f;
    public float TargetingRange = 0f;
    public float LeashRange = 0f;

    private Unit Unit;
    private Unit currentTarget;
    private UnityEvent<Unit> onNewAttackTarget = new UnityEvent<Unit>();

    public IEnumerator GetAttackTargetCoroutine(Unit unit, UnityAction<Unit> onNewAttackTarget)
    {
        Unit = unit;
        // Set range and leash range to at least the units attack range
        if (TargetingRange < Unit.AttackRange)
        {
            TargetingRange = Unit.AttackRange;
        }
        if (LeashRange < TargetingRange)
        {
            LeashRange = TargetingRange;
        }

        // Set the event to call when a new target is found
        this.onNewAttackTarget.RemoveAllListeners();
        this.onNewAttackTarget.AddListener(onNewAttackTarget);

        while (true)
        {
            if (!IsTargetValidAndInRange(currentTarget, LeashRange))
            {
                // TODO Add a layerMask to make it more performant
                Collider[] targets = Physics.OverlapSphere(Unit.transform.position, TargetingRange);

                // Based on the unit targeting type, select a target
                switch (UnitAttackTargetingType)
                {
                    case UnitAttackTargetingType.Closest:
                        SelectClosestTarget(targets);
                        break;
                }
            }

            yield return new WaitForSeconds(CheckForValidTargetInterval);
        }
    }

    public bool IsTargetValidAndInRange(Unit target, float range)
    {
        return target != null
            && target.IsValidTarget
            && target.UnitType == TargetUnitType
            && Vector3.Distance(Unit.transform.position, target.transform.position) <= range;
    }

    public void SetTarget(Unit target)
    {
        currentTarget = target;
    }

    private void SelectClosestTarget(Collider[] targets)
    {
        Unit closestUnit = null;
        float closestUnitDistance = Mathf.Infinity;

        foreach (Collider collider in targets)
        {
            Unit targetUnit = collider.GetComponent<Unit>();
            // If this is a valid unit, check how close to us it is
            if (IsTargetValidAndInRange(targetUnit, TargetingRange))
            {
                float distanceToTarget = Vector3.Distance(Unit.transform.position, collider.transform.position);
                if (distanceToTarget < closestUnitDistance)
                {
                    closestUnitDistance = distanceToTarget;
                    closestUnit = targetUnit;
                }
            }
        }

        if (closestUnit != currentTarget)
        {
            // Target the closest unit
            currentTarget = closestUnit;
            // Emit that we have a new target
            onNewAttackTarget.Invoke(closestUnit);
        }
    }
}