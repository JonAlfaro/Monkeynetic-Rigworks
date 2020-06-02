using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnPoint : SpawnPoint
{
    [Header("Spawn List")]
    public List<WaveSpawn> SpawnList;
    public bool RandomiseSpawnOrder = true;

    [Header("Target Override")]
    public Unit TargetOverride;
    public float LeashRangeOverride = 0;

    private HashSet<int> overridenSpawns = new HashSet<int>();
    private int currentSpawnIndex = 0;

    private void Awake()
    {
        if (RandomiseSpawnOrder)
        {
            SpawnList.Shuffle();
        }
    }

    public override bool Spawn(float gameTime)
    {
        if (currentSpawnIndex >= SpawnList.Count)
        {
            return false;
        }

        // Override spawn values with the values set in the current WaveSpawn
        UnitPrefab = SpawnList[currentSpawnIndex].Unit;
        NumberToSpawn = SpawnList[currentSpawnIndex].AmountToSpawn;
        SpawnCooldown = SpawnList[currentSpawnIndex].TimeUntilNextSpawn;

        if (base.Spawn(gameTime)) {
            ApplyTargetOverride();
            currentSpawnIndex++;
            return true;
        }

        return false;
    }

    private void ApplyTargetOverride()
    {
        // Loop through all spawned units
        foreach (Unit spawn in spawns)
        {
            // If this unit hasn't had it's target overridden, override the target and add it to the set
            if (!overridenSpawns.Contains(spawn.GetInstanceID())) {
                spawn.SetTarget(TargetOverride, LeashRangeOverride);
                overridenSpawns.Add(spawn.GetInstanceID());
            }
        }
    }

    protected override void RemoveDeadSpawnsFromList()
    {
        // Remove all dead unit ids from the overridenSpawns set
        foreach (Unit spawn in spawns)
        {
            if (spawn.UnitStats.IsDead)
            {
                overridenSpawns.Remove(spawn.GetInstanceID());
            }
        }

        base.RemoveDeadSpawnsFromList();
    }
}
