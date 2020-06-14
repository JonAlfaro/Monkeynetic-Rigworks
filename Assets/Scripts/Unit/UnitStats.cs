using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UnitStats
{
    public float MaxHealth = 10;
    public float MovementSpeed = 3;
    public float MinimumMovementSpeed = 0.5f;
    public UnitType UnitType;
    public float OutgoingDamageMultiplier = 1;
    public float IncomingDamageMultiplier = 1;
    public Text uiTextHealth;
    public Text uiCoreTextHealth;
    public bool CanModifyAttackTarget;


    public bool isPlayer = false;

    public bool isCore = false;
    // Not shown in inspector
    public float CurrentMovementSpeed { get; private set; }
    public float CurrentHealth { get; set; }

    // Getters
    public bool IsDead => CurrentHealth <= 0;

    public void UpdateHealth()
    {
        if (isPlayer)
        {
            uiTextHealth.text = $"Health {CurrentHealth} / {MaxHealth}";
        } else if (isCore)
        {
            uiCoreTextHealth.text = $"Health {CurrentHealth} / {MaxHealth}";
        }
    }

    public UnitStats Init()
    {
        CurrentHealth = MaxHealth;
        CurrentMovementSpeed = MovementSpeed;
        if (isPlayer)
        {
            uiTextHealth.text = $"Health {CurrentHealth} / {MaxHealth}";
        } else if (isCore)
        {
            uiCoreTextHealth.text = $"Health {CurrentHealth} / {MaxHealth}";
        }
        return this;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage * IncomingDamageMultiplier;
        if (isPlayer)
        {
            uiTextHealth.text = $"Health {CurrentHealth} / {MaxHealth}";
        } else if (isCore)
        {
            uiCoreTextHealth.text = $"Core Health {CurrentHealth} / {MaxHealth}";
        }
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
