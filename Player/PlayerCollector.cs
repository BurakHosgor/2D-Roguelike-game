using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;


    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        playerCollector.radius = player.CurrentMagnet;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        
        //Check if the other game object has the ICollectible interface
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //Pulling Animation
            // Gets the Rigidbody2D component on the item
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            // Vector2 pointing  from the item to the player
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            // Applies force to the item in the forceDirecton with pullSpeed
            rb.AddForce(forceDirection * pullSpeed);

            // If it does, call the collector method
            collectible.Collect();
        }
    }

}
