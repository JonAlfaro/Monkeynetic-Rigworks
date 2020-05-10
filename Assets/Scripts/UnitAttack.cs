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

    private Unit unit;

    public void Init(Unit unit)
    {
        this.unit = unit;
    }

    public void Attack()
    {
        if (AttackProjectile)
        {
            GameObject projectileGO = GameObject.Instantiate(AttackProjectile, unit.transform.position, Quaternion.identity);
            projectileGO.GetComponent<UnitAttackProjectile>().Init(Target, unit.UnitStats);
        }

        NextAttackTime = Time.time + TimeBetweenAttacks;
    }
}
