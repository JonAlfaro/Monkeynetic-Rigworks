using System;

[Serializable]
public class UnitStats
{
    public float MaxHealth = 10;
    public float MovementSpeed = 3;
    public float MinimumMovementSpeed = 0.5f;
    public UnitType UnitType;
    public float OutgoingDamageMultiplier = 1;
    public float IncomingDamageMultiplier = 1;
    // Not shown in inspector
    public float CurrentMovementSpeed { get; private set; }
    public float CurrentHealth { get; private set; }

    // Getters
    public bool IsDead => CurrentHealth <= 0;


    public UnitStats Init()
    {
        CurrentHealth = MaxHealth;
        CurrentMovementSpeed = MovementSpeed;
        return this;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage * IncomingDamageMultiplier;
    }

    public void ModifyMovementSpeed(float amount)
    {
        CurrentMovementSpeed = MovementSpeed + amount;
        if (CurrentMovementSpeed < MinimumMovementSpeed)
        {
            CurrentMovementSpeed = MinimumMovementSpeed;
        }
    }
}
