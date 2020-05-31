using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPoint[] SpawnPoints { get; set; }

    private void Start()
    {
        SpawnPoints = FindObjectsOfType<SpawnPoint>();
    }

    public void OnTimeChanged(float time)
    {
        foreach (SpawnPoint spawnPoint in SpawnPoints)
        {
            spawnPoint.Spawn(time);
        }
    }
}
