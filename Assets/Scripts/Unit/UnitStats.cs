using System;

[Serializable]
public class UnitStats
{
    public float MaxHealth = 10;
    public float MovementSpeed = 3;
    public UnitType UnitType;
    public float OutgoingDamageMultiplier = 1;
    public float IncomingDamageMultiplier = 1;

    // Getters
    public bool IsDead => currentHealth <= 0;

    private float currentHealth;

    public UnitStats Init()
    {
        currentHealth = MaxHealth;

        return this;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage * IncomingDamageMultiplier;
    }
}
