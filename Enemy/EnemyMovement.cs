using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    EnemyStats enemy;
    float distance;
    float minDistance = 0.3f;

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
        distance = Vector2.Distance(transform.position, player.position);

        //  If we are currently being knocked back, then process the knockback.
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else if (distance > minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
            // Flip the character based on player position
            if (transform.position.x < player.transform.position.x)
            {
                // If the character is to the left of the player, flip to face right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // If the character is to the right of the player, flip to face left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
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
