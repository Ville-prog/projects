using UnityEngine;
using System.Collections;

/*
 * This class is responsible for managing the spawning of projectiles.
 * Spawns projectiles at random x-coordinates within the grid size.
 * The spawn interval decreases over time logarithmically.
 */
public class PorojectileManagerScript : MonoBehaviour
{
    // "[SerializeField]" allows private fields to be visible and editable in the Unity Inspector.
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab
    [SerializeField] private BlockScript blockScript; // Reference to blockScript class

    [SerializeField] private float initialSpawnInterval = 3; 
    [SerializeField] private float minSpawnInterval = 0.17f; 
    [SerializeField] private float spawnIntervalDecreaseRate = 1.1f; 
    [SerializeField] private int spawnHeight = 300;
    private float currentSpawnInterval;

    /*
     * Start is called before the first frame update
     */ 
    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnProjectileCoroutine());
    }

    /*
     * Coroutine to spawn projectiles
     */ 
    IEnumerator SpawnProjectileCoroutine()
    {
        while (true)
        {
            float randomX = Random.Range(0, blockScript.getGridSize());

            Vector2 spawnPosition = new Vector2(randomX, spawnHeight);
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Wait for the current spawn interval before spawning the next projectile
            yield return new WaitForSeconds(currentSpawnInterval);

             // Decrease the spawn interval logarithmically
            float timeFactor = Mathf.Log(Time.timeSinceLevelLoad + 1, 10);

            currentSpawnInterval = Mathf.Max(minSpawnInterval,
                                    initialSpawnInterval - timeFactor * spawnIntervalDecreaseRate);
        }
    }
}
