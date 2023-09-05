using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // A list  of groups of enemies to spawn in this wave
        public int waveQuota; // The total number of enemies to spawn in this wave
        public float spawnInterval; //The interval at which to spawn enemies 
        public int spawnCount; // The number of enemies already spawned in this wave
    }
    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //The number of enemies to spawn in this wave
        public int spawnCount;//The number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; // A list of all the waves in the game
    public int currentWaveCount; // The index of the current wave 

    [Header("Spawner Attributes")]
    float spawnTimer; //Timer use to determine when to spawn the next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval; //The interval between each wave
    bool isWaveActive = false;


    [Header("Spawn Position")]
    public List<Transform> relativeSpawnPoints; //A list to store all the relative spawn points of enemies

    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive) //Check if the wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;

        //Wave for "waveInterval" seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        //If there are more waves to start after the current wave, move on to the next wave
        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota() ;
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }
    /// <summary>
    /// This method will stop spawning enemies if the amount of enemies on the map is maximum
    /// The method will only spawn enemies in a particular wave until it is time for the next wave's enemies to be spawned 
    /// </summary>
    void SpawnEnemies()
    {
        //Check if the minumum number of enemies in the wave have been spawned 
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //Spawn each type of enemy until the quota is filled 
            foreach (var enemygroup in waves[currentWaveCount].enemyGroups)
            {
                //Check if the minumum number of enemies of this type have been spawned 
                if (enemygroup.spawnCount < enemygroup.enemyCount)
                {
                   

                    Instantiate (enemygroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0,relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemygroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    //limit the number of enemies that can be spawned at once 
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
        
    }
    public void OnEnemyKilled()
    {
        enemiesAlive--;

        // Reset the maxEnemiesREached falg if the number of enemies alive has dropped below the maximum amount
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}
