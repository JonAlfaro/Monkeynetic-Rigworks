using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitMovementAggressive
{
    public float MinDistanceFromTarget = 1f;
    public float MaxDistanceFromTarget = 1f;

    public Transform target;
    private Unit unit;
    private Vector3 targetPosition;
    private Vector3 movementTarget;
    private Vector3 previousMovementTarget;
    private UnityEvent<Vector3> onNewMovementTarget = new UnityEvent<Vector3>();

    float closeEnoughAmount = 0.05f; // When checking distance, we don't need to be perfectly precise so we fudge it by this amount

    //Unit targetUnit
    public IEnumerator GetMovementTargetCoroutine(UnityAction<Vector3> onNewMovementTarget, Unit unit)
    {
        // Set the event to call when this coroutine is triggered
        this.onNewMovementTarget.RemoveAllListeners();
        this.onNewMovementTarget.AddListener(onNewMovementTarget);
        this.unit = unit;

        while (true)
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

    // Sets movementTarget. Returns true if there is a new movement target, false if not.
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
