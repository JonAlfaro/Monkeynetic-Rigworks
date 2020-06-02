using System;
using UnityEngine;

[Serializable]
public class WaveSpawn
{
    public GameObject Unit;
    public float TimeUntilNextSpawn = 0.5f;
    public int AmountToSpawn = 1;
}
