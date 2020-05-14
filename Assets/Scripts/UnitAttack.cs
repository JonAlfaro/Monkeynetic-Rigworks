using UnityEngine;
using System;

[Serializable]
public class UnitAttack
{
    public GameObject AttackProjectile;
    public float TimeBetweenAttacks = 3f;
    public float Range = 3;
    public Unit Target { get; set; }
    public float NextAttackTime { get; private set; } = 0f;
    public bool IsReady => Time.time >= NextAttackTime;
    public Transform ProjectileSpawnTransform; // This is the transform that projectiles will spawn from. If left null it will default to the units transform

    private Unit unit;

    public void Init(Unit unit)
    {
        this.unit = unit;

        if (ProjectileSpawnTransform == null)
        {
            ProjectileSpawnTransform = unit.GetComponent<Transform>();
        }
    }

    public void Attack()
    {
        if (AttackProjectile)
        {
            GameObject projectileGO = GameObject.Instantiate(AttackProjectile, ProjectileSpawnTransform.position, Quaternion.identity);
            projectileGO.GetComponent<UnitAttackProjectile>().Init(Target, unit.UnitStats);
        }

        NextAttackTime = Time.time + TimeBetweenAttacks;
    }
}
