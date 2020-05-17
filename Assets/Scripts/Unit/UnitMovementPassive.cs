using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitMovementPassive
{
    public bool Enabled = false;
    public float RetargetMinTime;
    public float RetargetMaxTime;
    public float TargetDistanceMin;
    public float TargetDistanceMax;
    public Vector3 MovementTarget { get; private set; }
    private Unit unit;
    private UnityEvent<Vector3> onNewMovementTarget = new UnityEvent<Vector3>();

    public IEnumerator GetMovementTargetCoroutine(UnityAction<Vector3> onNewMovementTarget, Unit unit)
    {
        // Set the event to call when this coroutine is triggered
        this.onNewMovementTarget.RemoveAllListeners();
        this.onNewMovementTarget.AddListener(onNewMovementTarget);
        this.unit = unit;

        while (true)
        {
            SetNewMovementTarget();
            this.onNewMovementTarget.Invoke(MovementTarget);

            // Wait this long before finding a new target
            float nextTargetTime = Random.Range(RetargetMinTime, RetargetMaxTime);
            yield return new WaitForSeconds(nextTargetTime);
        }
    }

    // TODO add a field for the area that this guy has to stay within
    private void SetNewMovementTarget()
    {
        float randomX = Random.Range(TargetDistanceMin, TargetDistanceMax);
        float randomZ = Random.Range(TargetDistanceMin, TargetDistanceMax);

        MovementTarget = unit.transform.position + new Vector3(randomX, 0, randomZ);
    }
}
