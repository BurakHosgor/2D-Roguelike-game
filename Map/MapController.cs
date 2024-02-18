using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChuck;
    Vector3 playerLastPosition;
    
    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    // Start is called before the first frame update
    void Start()
    {
       playerLastPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChuck)
        {
            return;
        }

        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
        }

        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }

        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }

        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }

    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChuck.transform.Find(direction).position,checkerRadius,terrainMask))
        {
            SpawnChunk(currentChuck.transform.Find(direction).position);
        }
    }

    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //Moving horizontally more than vertically
            if (direction.y > 0.5f)
            {
                //Also moving upwards
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (direction.y < -0.5f)
            {
                //Also moving downwards
                return direction.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                //Moving htraight horizontally
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            //Moving vertically more than horizontally
            if (direction.x > 0.5f)
            {
                //Also moving right
                return direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if (direction.x < -0.5f)
            {
                //also moving left
                return direction.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                //moving straight vertically
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range (0 , terrainChunks.Count);
        latestChunk =Instantiate(terrainChunks[rand] ,spawnPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;
        if (optimizerCooldown < 0)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);

            if (opDist > maxOpDist) 
            {
                chunk.SetActive(false);           
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
