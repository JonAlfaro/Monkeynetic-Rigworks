using UnityEngine;
using UnityEngine.VFX;

public class UnitAttackProjectile : MonoBehaviour
{
    public float Damage = 2;
    public float ProjectileSpeed = 10;
    public GameObject OnDestroyEffect;
    public float ProjectileLifeTime = 0f;
    public Debuff Debuff;

    private Unit targetUnit;
    private Vector3 targetPosition;
    private float lifeTimeEnd;
    private Vector3 previousPosition;
    
    public void Init(Unit targetUnit, UnitStats unitStats)
    {
        this.targetUnit = targetUnit;
        targetPosition = targetUnit.transform.position;
        // Modify damage by the units outgoing damage multiplier
        Damage *= unitStats.OutgoingDamageMultiplier;
        Debuff.Damage *= unitStats.OutgoingDamageMultiplier;

        lifeTimeEnd = ProjectileLifeTime != 0 ? Time.fixedTime + ProjectileLifeTime : Mathf.Infinity;
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (HasHitTarget())
        {
            Trigger();
            return;
        }

        // If the target unit hasn't been destroyed, update the target position to its current position
        if (targetUnit != null && targetUnit.transform != null)
        {
            targetPosition = targetUnit.TargetTransform.position;
        }

        previousPosition = transform.position;
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime * ProjectileSpeed);

        Debug.DrawLine(previousPosition, transform.position);

        if (Time.fixedTime >= lifeTimeEnd)
        {
            Trigger(false);
        }
    }

    private void Trigger(bool reachedTarget = true)
    {
        if (reachedTarget)
        {
            targetUnit?.TakeDamage(Damage);
            targetUnit?.AddDebuff(Debuff);
        }

        // Instantiate the on destroy effect and then remove it from the scene shortly after
        if (OnDestroyEffect != null)
        {
            Destroy(Instantiate(OnDestroyEffect, transform.position, Quaternion.identity), 5f);
        }

        VisualEffect vfx = GetComponentInChildren<VisualEffect>();

        if (vfx != null)
        {
            vfx.Stop();
            vfx.transform.SetParent(transform.parent);
            vfx.SetFloat("KillTrails", -3);
            Destroy(vfx, 2f);
        }

        // Destroy self
        Destroy(gameObject);
    }

    private bool HasHitTarget()
    {
        // Check if we have reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            return true;
        }

        // Check if we have collided with the target unit
        Vector3 travelDirection = transform.position - previousPosition;
        RaycastHit[] raycastHits = Physics.RaycastAll(new Ray(previousPosition, travelDirection.normalized), travelDirection.magnitude);

        foreach (RaycastHit raycastHit in raycastHits)
        {
            Unit hitUnit = raycastHit.collider.GetComponent<Unit>();

            if (hitUnit != null && hitUnit.Equals(targetUnit))
            {
                return true;
            }
        }

        return false;
    }
}