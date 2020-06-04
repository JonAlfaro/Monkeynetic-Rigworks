using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnPoint : SpawnPoint
{
    [Header("Spawn List")]
    public List<WaveSpawn> SpawnList = new List<WaveSpawn>() { new WaveSpawn() };
    public bool RandomiseSpawnOrder = true;
    public int DesiredWaveWeight = 30;
    public int SpawnGameDay = 1;

    [Header("Target Override")]
    public Unit TargetOverride;
    public float LeashRangeOverride = 100;

    private List<WaveSpawn> activeSpawnList;
    private HashSet<int> overridenSpawns = new HashSet<int>();
    private int currentSpawnIndex = 0;
    private int currentWaveWeight = 0;

    private void Awake()
    {
        activeSpawnList = CreateActiveSpawnList();
        MaxConcurrentSpawnedCount = 999;
    }

    public override bool Spawn(float gameTime)
    {
        DespawnCheck(gameTime);
        if (SpawnGameDay != 0 && GameVariables.GameDay != SpawnGameDay)
        {
            return false;
        }

        if (currentSpawnIndex >= activeSpawnList.Count)
        {
            return false;
        }

        // Override spawn values with the values set in the current WaveSpawn
        UnitPrefab = activeSpawnList[currentSpawnIndex].Unit;
        NumberToSpawn = activeSpawnList[currentSpawnIndex].AmountToSpawn;
        SpawnCooldown = Random.Range(activeSpawnList[currentSpawnIndex].MinTimeUntilNextSpawn, activeSpawnList[currentSpawnIndex].MaxTimeUntilNextSpawn);

        if (base.Spawn(gameTime)) {
            ApplyTargetOverride();
            currentSpawnIndex++;
            return true;
        }

        return false;
    }

    private void ApplyTargetOverride()
    {
        if (TargetOverride == null)
        {
            return;
        }

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

    /* This method isn't accurate for a lot of reasons.
     * 
     * 1. The probability of each wave to be added to the list is affected by the minimum number that must be present in the wave.
     *    If you have a minimum of 1 and a probability of 0.5, 1 will always be added and then AFTER that you have an additional 50% chance of being added to the list more than once.
     * 2. The probability is affected by your order in the spawn list because it isn't shuffled first. If you are earlier in the spawn list, 
     *    you are more likely to be included than those later in the list since the for loop will encounter you more often.
     *    
     * I don't have the time to make the solution better and it doesn't bother me at all anyway so ¯\_(ツ)_/¯
     */
    private List<WaveSpawn> CreateActiveSpawnList()
    {
        currentWaveWeight = 0;
        // Copy SpawnList since we don't want to modify the original
        List<WaveSpawn> spawnList = new List<WaveSpawn>(SpawnList);
        List<WaveSpawn> newSpawnList = new List<WaveSpawn>();

        // Add the minimum number of spawns that must be present in this wave
        foreach (WaveSpawn waveSpawn in spawnList)
        {
            if (waveSpawn.MinSpawnsPerWave > 0)
            {
                for (int i = 0; i < waveSpawn.MinSpawnsPerWave; i++)
                {
                    newSpawnList.Add(waveSpawn);
                    waveSpawn.SpawnAmountInCurrentWave++;
                }
            }
        }

        // As long as the current wave weight is lower than the desired wave weight and we have things to spawn
        while (currentWaveWeight < DesiredWaveWeight && spawnList.Count > 0)
        {
            // Add other units based on their probability
            foreach (WaveSpawn waveSpawn in spawnList)
            {
                if (currentWaveWeight >= DesiredWaveWeight)
                {
                    break;
                }
                if (waveSpawn.ShouldSpawn())
                {
                    newSpawnList.Add(waveSpawn);
                    waveSpawn.SpawnAmountInCurrentWave++;
                    currentWaveWeight += waveSpawn.SpawnWeight;
                }
            }
        }

        if (RandomiseSpawnOrder)
        {
            newSpawnList.Shuffle();
        }

        return newSpawnList;
    }
}
