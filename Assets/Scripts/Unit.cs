﻿using UnityEngine;

public enum UnitMovementType { Passive, Aggressive, FollowPath, None }
public enum UnitType { Enemy, Tower, Player, None }

public class Unit : MonoBehaviour
{
    // Unit stat stuff
    public UnitStats UnitStats = new UnitStats();
    public UnitMovementType StartingUnitMovementType = UnitMovementType.None;
    public UnitMovementPassive UnitMovementPassive = new UnitMovementPassive();
    public UnitMovementAggressive UnitMovementAggressive = new UnitMovementAggressive();
    public UnitAttackTargeting UnitAttackTargeting = new UnitAttackTargeting();
    public UnitAttack UnitAttack = new UnitAttack();
    
    public Transform TargetTransform; // This is the transform that projectiles will target. If left null it will default to the units transform

    // Getters
    public UnitType UnitType => UnitStats.UnitType;
    public bool IsValidTarget => isActiveAndEnabled && !UnitStats.IsDead;
    public float AttackRange => UnitAttack.Range;

    private Vector3 movementTarget;
    private bool hasMovementTarget = false;
    private Coroutine movementCoroutine;

    // Lifecycle functions
    private void Awake()
    {
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
        // Perform attack
        if (UnitAttack.Target != null && UnitAttack.IsReady)
        {
            UnitAttack.Attack();
        }
    }

    private void FixedUpdate()
    {
        // Perform movement
        if (hasMovementTarget)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, movementTarget, Time.fixedDeltaTime * UnitStats.MovementSpeed);

            // If target is reached (ignoring y axis), stop moving
            Vector3 currentPositionWithoutY = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPositionWithoutY = new Vector3(movementTarget.x, 0, movementTarget.z);

            if (Vector3.Distance(currentPositionWithoutY, targetPositionWithoutY) < 0.1f)
            {
                hasMovementTarget = false;
            }
        }

        // Perform rotation
        // If the unit has a valid attack target look towards it, otherwise look towards the movement target
        if (UnitAttack.Target != null)
        {
            // Ignores y axis (it wont look up or down at the target)
            transform.LookAt(new Vector3(UnitAttack.Target.TargetTransform.position.x, transform.position.y, UnitAttack.Target.TargetTransform.position.z));
        }
        else if (hasMovementTarget)
        {
            transform.LookAt(movementTarget);
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
        hasMovementTarget = true;
    }

    public void SetMovementType(UnitMovementType unitMovementType)
    {
        switch (unitMovementType)
        {
            case UnitMovementType.None:
                if (movementCoroutine != null)
                {
                    StopCoroutine(movementCoroutine);
                }
                break;
            case UnitMovementType.Passive:
                // Start a couroutine to get a new random movement target at a pre-determined interval.
                movementCoroutine = StartCoroutine(UnitMovementPassive.GetMovementTargetCoroutine(OnNewMovementTarget, this));
                break;
            case UnitMovementType.Aggressive:
                movementCoroutine = StartCoroutine(UnitMovementAggressive.GetMovementTargetCoroutine(OnNewMovementTarget, this));
                break;
            case UnitMovementType.FollowPath:
                // TODO
                break;
        }
    }

    // Called whenever the GetAttackTargetCoroutine finds a new attack target
    void OnNewAttackTarget(Unit attackTarget)
    {
        UnitAttack.Target = attackTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, UnitAttack.Range);
    }
}