using JetBrains.Annotations;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public GameObject entityPrefab; // Drag and drop the prefab in the Inspector
    public Transform spawnArea; // Define the area where entities will be spawned
    public float spawnInterval = 1f; // Set the time interval between spawns
    public int maxSpawnCount = 12; // Set the maximum number of spawns at a time

    private int currentSpawnCount = 0;

    private void Start()
    {
        // Start spawning entities after a delay (0 seconds in this case)
        for (int i = 1; i <= 12; i++)
            SpawnEntity();
        EventManager.EntityDestroyed += SpawnEntity2;
    }

    public void SpawnEntity2(TargetDummy targetDummy)
    {
        SpawnEntity();
    }

    public void SpawnEntity()
    {
        if (currentSpawnCount >= maxSpawnCount)
        {
            // Max spawn count reached, stop spawning
            //return;
        }

        // Get the half size of the entity's bounding box (assuming the entity has a MeshRenderer or Collider)
        Vector3 entitySize = entityPrefab.GetComponent<Renderer>().bounds.extents;

        // Calculate the spawn position based on the size of the entity
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2 + entitySize.x + 3.5f, spawnArea.position.x + spawnArea.localScale.x / 2 - entitySize.x -3.5f),
            spawnArea.position.y + 0.2f, // Y value set to the minimum Y value
            Random.Range(spawnArea.position.z - spawnArea.localScale.z / 2 + entitySize.z + 3.5f, spawnArea.position.z + spawnArea.localScale.z / 2 - entitySize.z - 3.5f)
        );

        // Instantiate the entity at the calculated spawn position
        Instantiate(entityPrefab, spawnPosition, Quaternion.identity);

        currentSpawnCount++;
    }
}
