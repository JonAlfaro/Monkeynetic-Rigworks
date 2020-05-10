using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitMovementPassive
{
    public float RetargetMinTime;
    public float RetargetMaxTime;
    public float TargetDistanceMin;
    public float TargetDistanceMax;
    public Vector3 MovementTarget { get; private set; }
    private UnityEvent<Vector3> onNewMovementTarget = new UnityEvent<Vector3>();

    public IEnumerator GetMovementTargetCoroutine(UnityAction<Vector3> onNewMovementTarget)
    {
        // Set the event to call when this coroutine is triggered
        this.onNewMovementTarget.RemoveAllListeners();
        this.onNewMovementTarget.AddListener(onNewMovementTarget);

        while (true)
        {
            SetNewMovementTarget();
            this.onNewMovementTarget.Invoke(MovementTarget);

            // Wait this long before finding a new target
            float nextTargetTime = Random.Range(RetargetMinTime, RetargetMaxTime);
            yield return new WaitForSeconds(nextTargetTime);
        }
    }

    private void SetNewMovementTarget()
    {
        float randomX = Random.Range(TargetDistanceMin, TargetDistanceMax);
        float randomZ = Random.Range(TargetDistanceMin, TargetDistanceMax);

        MovementTarget = new Vector3(randomX, 0, randomZ);
    }
}
