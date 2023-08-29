using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) // if it gets too close the player destroy it no need for any fancy code.
        {
            Destroy(gameObject);
        }
    }

}
