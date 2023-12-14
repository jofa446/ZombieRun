using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public List<GameObject> grounds; // Una lista para almacenar los diferentes grounds
    Vector3 nextSpawnPoint;
    int lastSpawnedIndex = -1;
    int consecutiveCount = 0;
    int maxConsecutive = 3; // Se requieren al menos 3 diferentes antes de repetir

    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, grounds.Count);
        } while (randomIndex == lastSpawnedIndex && consecutiveCount >= maxConsecutive);

        GameObject prefabToSpawn = grounds[randomIndex];

        GameObject temp = Instantiate(prefabToSpawn, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(0).transform.position;

        if (randomIndex == lastSpawnedIndex)
        {
            consecutiveCount++;
        }
        else
        {
            lastSpawnedIndex = randomIndex;
            consecutiveCount = 1;
            maxConsecutive = 0;
        }

    }
}
