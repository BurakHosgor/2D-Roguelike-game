using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    EnemyStats enemy;

    Vector2 knockbackVelocity;
    float knockbackDuration;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //  If we are currently being knocked back, then process the knockback.
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        }
        
    }

    // This is meant to be called from other scripts to create knockback.
    public void Knockback(Vector2 velocity, float duration)
    {
        // Ignore the knockback if the duration is greater than 0
        if (knockbackDuration > 0) return;

        // Begins the knockback.
        knockbackVelocity = velocity;
        knockbackDuration = duration;

    }
}
