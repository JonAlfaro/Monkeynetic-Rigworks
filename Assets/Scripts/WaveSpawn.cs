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
    public float Probability = 1f;
    public int MinSpawnsPerWave = 0;
    public int MaxSpawnsPerWave = 0;
    [HideInInspector] public int SpawnAmountInCurrentWave = 0;
}
