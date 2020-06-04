using UnityEngine;
using UnityEngine.AI;

public enum UnitMovementType { Passive, Aggressive, FollowPath, None }
public enum UnitType { Enemy, Tower, Player, None }

public class Unit : MonoBehaviour
{
    // Unit stat stuff
    [Header("Stats")]
    public UnitStats UnitStats = new UnitStats();
    [Header("Movement")]
    public UnitMovementType StartingUnitMovementType = UnitMovementType.None;
    public UnitMovementPassive UnitMovementPassive = new UnitMovementPassive();
    public UnitMovementAggressive UnitMovementAggressive = new UnitMovementAggressive();
    [Header("Attack")]
    public UnitAttack UnitAttack = new UnitAttack();
    public UnitAttackTargeting UnitAttackTargeting = new UnitAttackTargeting();
    
    public Transform TargetTransform; // This is the transform that projectiles will target. If left null it will default to the units transform

    // Getters
    public UnitType UnitType => UnitStats.UnitType;
    public bool IsValidTarget => isActiveAndEnabled && !UnitStats.IsDead;
    public float AttackRange => UnitAttack.Range;

    private Vector3 movementTarget;
    private Coroutine movementCoroutine;
    private NavMeshAgent navMeshAgent;

    // Lifecycle functions
    private void Awake()
    {
        InitNavMeshAgent();
        if (TargetTransform == null)
        {
            TargetTransform = transform;
        }
        UnitStats.Init();
        UnitAttack.Init(this);
        SetMovementType(StartingUnitMovementType);
        // Start coroutine to check for targets in the area
        StartCoroutine(UnitAttackTargeting.GetAttackTargetCoroutine(this, OnNewAttackTarget));
    }

    private void Update()
    {
        UpdateState();
        if (UnitStats.IsDead)
        {
            return;
        }

        // If the unit has a valid attack target look towards it, otherwise the unit will look towards its movement target (done automatically using NavMeshAgent)
        if (UnitAttackTargeting.IsTargetValidAndInRange(UnitAttack.Target, UnitAttack.Range))
        {
            // Perform rotation
            // Ignores y axis (it wont look up or down at the target)
            transform.LookAt(new Vector3(UnitAttack.Target.TargetTransform.position.x, transform.position.y, UnitAttack.Target.TargetTransform.position.z));

            // Perform attack
            if (UnitAttack.IsReady)
            {
                UnitAttack.Attack();
            }
        }
    }

    // Public functions
    public void TakeDamage(float damage) => UnitStats.TakeDamage(damage);

    // Private functions
    private void UpdateState()
    {
        if (UnitStats.IsDead)
        {
            Destroy(gameObject);
        }
        // TODO: Monkey 🐒🐒🐒🐒
    }
    // Called whenever the GetMovementTargetCoroutine is called. Assigns a new movement target and starts moving towards it
    void OnNewMovementTarget(Vector3 newMovementTarget)
    {
        movementTarget = newMovementTarget;

        // Set navMeshAgent target
        navMeshAgent?.SetDestination(movementTarget);
    }

    public void SetMovementType(UnitMovementType unitMovementType)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        switch (unitMovementType)
        {
            case UnitMovementType.None:
                break;
            case UnitMovementType.Passive:
                // Start a couroutine to get a new random movement target at a pre-determined interval.
                movementCoroutine = StartCoroutine(UnitMovementPassive.GetMovementTargetCoroutine(OnNewMovementTarget, this));
                break;
            case UnitMovementType.Aggressive:
                movementCoroutine = StartCoroutine(UnitMovementAggressive.GetMovementTargetCoroutine(OnNewMovementTarget, this, UnitAttack.Target.transform));
                break;
            case UnitMovementType.FollowPath:
                // TODO
                break;
        }
    }

    public void SetTarget(Unit target, float newLeashRange = 0f)
    {
        if (newLeashRange > 0)
        {
            UnitAttackTargeting.LeashRange = newLeashRange;
        }
        UnitAttackTargeting.TargetUnitType = target.UnitType;
        UnitAttackTargeting.SetTarget(target);
    }

    // Called whenever the GetAttackTargetCoroutine finds a new attack target
    void OnNewAttackTarget(Unit attackTarget)
    {
        UnitAttack.Target = attackTarget;
        
        if (attackTarget == null)
        {
            SetMovementType(StartingUnitMovementType);
        }
        else if (UnitMovementAggressive.Enabled)
        {
            SetMovementType(UnitMovementType.Aggressive);
        }
    }

    private void InitNavMeshAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent)
        {
            navMeshAgent.speed = UnitStats.MovementSpeed;
        }
    }

    // Draws unit range in the editor only
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, UnitAttack.Range);
    }
}