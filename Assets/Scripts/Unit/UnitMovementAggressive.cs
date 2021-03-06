﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitMovementAggressive
{
    public bool Enabled = false;
    public float MinDistanceFromTarget = 1f;
    public float MaxDistanceFromTarget = 1f;

    private Transform target;
    private Unit unit;
    private Vector3 targetPosition;
    private Vector3 movementTarget;
    private Vector3 previousMovementTarget;
    private UnityEvent<Vector3> onNewMovementTarget = new UnityEvent<Vector3>();

    float closeEnoughAmount = 0.05f; // When checking distance, we don't need to be perfectly precise so we fudge it by this amount

    public IEnumerator GetMovementTargetCoroutine(UnityAction<Vector3> onNewMovementTarget, Unit unit, Transform target)
    {
        // Set the event to call when this coroutine is triggered
        this.onNewMovementTarget.RemoveAllListeners();
        this.onNewMovementTarget.AddListener(onNewMovementTarget);
        this.unit = unit;
        this.target = target;

        while (!unit.UnitStats.IsDead)
        {
            SetNewMovementTarget();

            if (previousMovementTarget != movementTarget)
            {
                this.onNewMovementTarget.Invoke(movementTarget);
            }

            // Wait for the next frame before continuing
            yield return null;
        }
    }

    // Sets movementTarget
    private void SetNewMovementTarget()
    {
        previousMovementTarget = movementTarget;
        Vector3 myPosition = unit.transform.position;

        if (target != null)
        {
            targetPosition = target.position;
        }

        float distanceToTarget = Vector3.Distance(myPosition, targetPosition);

        if (distanceToTarget > (MaxDistanceFromTarget + closeEnoughAmount))
        {
            movementTarget = targetPosition;
        }
        else if (distanceToTarget < (MinDistanceFromTarget + closeEnoughAmount))
        {
            Vector3 direction = myPosition - targetPosition;
            movementTarget = myPosition + direction.normalized;
        }
        else
        {
            movementTarget = myPosition;
        }
    }
}
