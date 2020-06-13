using UnityEngine;
using UnityEngine.AI;

public enum UnitMovementType { Passive, Aggressive, None }
public enum UnitType { Enemy, Tower, Player, Core, None }

public class Unit : MonoBehaviour
{
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
    public bool IsDead => UnitStats.IsDead;
    public UnitType TargetUnitType => UnitAttackTargeting.TargetUnitType;

    private Vector3 movementTarget;
    private Coroutine movementCoroutine;
    private NavMeshAgent navMeshAgent;
    private DebuffHandler debuffHandler = new DebuffHandler();
    public void AddDebuff(Debuff debuff) => debuffHandler.AddDebuff(debuff);
    public void SetTargetType(UnitType? targetType) => UnitAttackTargeting.SetTargetUnitType(targetType);

    // Lifecycle functions
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        UpdateNavMeshAgentSpeed();
        if (TargetTransform == null)
        {
            TargetTransform = transform;
        }
        UnitStats.Init();
        UnitAttack.Init(this);
        SetMovementType(StartingUnitMovementType);
        // Start coroutine to check for targets in the area
        StartCoroutine(UnitAttackTargeting.GetAttackTargetCoroutine(this, OnNewAttackTarget));
        // Start coroutine to check and apply debuffs at a set interval
        StartCoroutine(debuffHandler.GetDebufsCheckedCoroutine(this, OnDebuffsChecked));
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
            debuffHandler.StopAllDebuffAnimations();
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
        }
    }

    public void ForceSetTarget(Unit target, float newLeashRange = 0f)
    {
        if (newLeashRange > 0)
        {
            UnitAttackTargeting.LeashRange = newLeashRange;
            UnitAttackTargeting.TargetingRange = newLeashRange;
        }
        UnitAttackTargeting.TargetUnitType = target.UnitType;
        UnitAttackTargeting.SetTargetUnitType(target.UnitType);
        UnitAttackTargeting.SetTarget(target);
    }

    public void UpdateNavMeshAgentSpeed()
    {
        if (navMeshAgent)
        {
            navMeshAgent.speed = UnitStats.CurrentMovementSpeed;
        }
    }

    // Called whenever the GetAttackTargetCoroutine finds a new attack target
    void OnNewAttackTarget(Unit attackTarget)
    {
        UnitAttack.Target = attackTarget;
        
        if (attackTarget == null)
        {
            if (UnitMovementPassive.Enabled)
            {
                SetMovementType(UnitMovementType.Passive);
            }
            else
            {
                SetMovementType(UnitMovementType.None);
            }
        }
        else if (UnitMovementAggressive.Enabled)
        {
            SetMovementType(UnitMovementType.Aggressive);
        }
    }

    // Called whenever debuffs are checked, after they are applied
    void OnDebuffsChecked()
    {
        // TODO may want to do stuff
    }

    // Draws unit range in the editor only
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, UnitAttack.Range);
    }
}