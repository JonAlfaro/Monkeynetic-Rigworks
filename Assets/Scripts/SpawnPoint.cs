using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject UnitPrefab;
    public float MaxSpawnRange;
    public List<int> CanSpawnGameTimeHours = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
    public int NumberToSpawn = 1;
    public int MaxConcurrentSpawnedCount = 10;
    public float SpawnCooldown = 50f;

    private float timeTillNextSpawn = 0f;
    private List<Unit> spawns = new List<Unit>();

    public void Spawn(float gameTime)
    {
        RemoveDeadSpawnsFromList();
        if (!ShouldSpawn(gameTime))
        {
            return;
        }
        timeTillNextSpawn = Time.time + SpawnCooldown;

        // Spawn unit(s)
        for (int i = 0; i < NumberToSpawn; i++)
        {
            // Stop spawning if we reach the limit to spawn
            if (spawns.Count >= MaxConcurrentSpawnedCount)
            {
                break;
            }

            // Get the random spawn position and instantiate the unit
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-MaxSpawnRange, MaxSpawnRange), 0, Random.Range(-MaxSpawnRange, MaxSpawnRange));
            GameObject go = Instantiate(UnitPrefab, transform.position + randomSpawnPosition, Quaternion.identity);
            Unit unit = go.GetComponent<Unit>();

            // Ensure we have a Unit component before adding it to the spawn list
            if (unit != null)
            {
                spawns.Add(unit);
            }
            else
            {
                Debug.LogError("Couldn't find Unit component on UnitPrefab. Make sure it's on the UnitPrefab directly.");
            }
        }
    }

    private bool ShouldSpawn(float gameTime) =>
        UnitPrefab != null 
        && CanSpawnGameTimeHours.Contains((int)System.Math.Floor(gameTime))
        && timeTillNextSpawn <= Time.time
        && spawns.Count < MaxConcurrentSpawnedCount;

    private void RemoveDeadSpawnsFromList()
    {
        spawns.RemoveAll(gameObject => gameObject.UnitStats.IsDead);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(MaxSpawnRange, 0.5f, MaxSpawnRange) * 2);
    }
}
