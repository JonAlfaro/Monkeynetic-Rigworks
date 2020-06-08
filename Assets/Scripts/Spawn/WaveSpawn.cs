using System;
using UnityEngine;

[Serializable]
public class WaveSpawn
{
    public GameObject Unit;
    public float MinTimeUntilNextSpawn = 0.5f;
    public float MaxTimeUntilNextSpawn = 0.5f;
    public int SpawnWeight = 1;
    public int AmountToSpawn = 1;
    public double Probability = 1;
    public int MinSpawnsPerWave = 0;
    public int MaxSpawnsPerWave = 0;
    [HideInInspector] public int SpawnAmountInCurrentWave = 0;
    private static System.Random random = new System.Random();

    public bool ShouldSpawn()
    {
        // If we have reached the max number of spawns this wave, we should not spawn any more
        if (MaxSpawnsPerWave > 0 && SpawnAmountInCurrentWave >= MaxSpawnsPerWave)
        {
            return false;
        }

        // If we have less than the minimum number of spawns in this wave, we should spawn a unit
        if (MinSpawnsPerWave > 0 && MinSpawnsPerWave > SpawnAmountInCurrentWave)
        {
            return true;
        }

        // If you have a probability < 1, roll a random number to decide if we should spawn
        if (Probability < 1)
        {
            if (random.NextDouble() < Probability)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Otherwise spawn this unit
        return true;
    }
}
