using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitStats UnitStats = new UnitStats();
    public UnitMovementPassive UnitMovement = new UnitMovementPassive();
    private Vector3 movementTarget;
    private bool hasMovmentTarget = false;

    void Start()
    {
        // Start a couroutine to get a new random movement target at a pre-determined interval.
        StartCoroutine(UnitMovement.GetMovementTargetCoroutine(OnNewMovementTarget));
    }

    // Called whenever the GetMovementTargetCoroutine is called. Assigns a new movement target and starts moving towards it
    void OnNewMovementTarget(Vector3 newMovementTarget)
    {
        movementTarget = transform.position + newMovementTarget;
        hasMovmentTarget = true;
    }

    void FixedUpdate()
    {
        // Perform movement
        if (hasMovmentTarget)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, movementTarget, Time.fixedDeltaTime * UnitStats.movementSpeed);

            // If target is reached (ignoring y axis), stop moving
            Vector3 currentPositionWithoutY = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPositionWithoutY = new Vector3(movementTarget.x, 0, movementTarget.z);

            if (Vector3.Distance(currentPositionWithoutY, targetPositionWithoutY) < 0.1f)
            {
                hasMovmentTarget = false;
            }
        }
    }
}
