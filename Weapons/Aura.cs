using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An aura is a damage-over-time effect that applies to a specific area in timed intervals.
/// It is used to give the functionality of Garlic, and it can also be used to spawn holy
/// water effects as well.
/// </summary>
public class Aura : WeaponEffect
{
   

    Dictionary<EnemyStats, float> affectedTargets = new Dictionary<EnemyStats, float>();
    List<EnemyStats> targetsToUnaffect = new List<EnemyStats>();

    

    // Update is called once per frame
    void Update()
    {
        Dictionary<EnemyStats, float> affectedTargsCopy = new Dictionary<EnemyStats, float>(affectedTargets);
        // Loop through every target affected by the aura, and reduce the cooldown
        // of the aura for it. if the cooldown reaches 0, deal damage to it. 
        foreach(KeyValuePair<EnemyStats, float>pair  in affectedTargsCopy)
        {
            affectedTargets[pair.Key] -= Time.deltaTime;
            // Reset the cooldown.
            if(pair.Value <= 0)
            {
                if (targetsToUnaffect.Contains(pair.Key))
                {
                    // If the target is marked for removal, remove it
                    affectedTargets.Remove(pair.Key);
                    targetsToUnaffect.Remove(pair.Key);
                }
                else
                {
                    // Reset the cooldown and deal damage.
                    Weapon.Stats stats = weapon.GetStats();
                    affectedTargets[pair.Key] = stats.cooldown;
                    pair.Key.TakeDamage(GetDamage(), transform.position, stats.knockback);
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out EnemyStats es)) 
        {
            // If the target is not yet affected by this aura, add it
            // to our list of affected targets.
            if (!affectedTargets.ContainsKey(es))
            {
                //Always starts with an interval of 0, so that it wil get
                //damaged in the next update() tick.
                affectedTargets.Add(es, 0);
            }
            else
            {
                if (targetsToUnaffect.Contains(es))
                {
                    affectedTargets.Remove(es);
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyStats es))
        {
            // If an objectis leaving the aura, remove it from our
            // affected targets list.
            if (affectedTargets.ContainsKey(es))
            {
               targetsToUnaffect.Add(es);
            }

        }
    }
}
