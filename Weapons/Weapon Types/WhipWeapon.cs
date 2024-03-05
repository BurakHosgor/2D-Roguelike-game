using UnityEngine;


public class WhipWeapon : ProjectileWeapon
{
    int currentSpawnCount; // How many times the whip has been attacking in this iteration.
    float currentSpawnYOffset; // If there are more than 2 whips, we will start offsetting it 
    protected override bool Attack(int attackCount = 1)
    {
        // if no projectile prefab is assigned, leave a warning message.
        if (!currentStats.projectilePrefab)
        {
            Debug.LogWarning(string.Format("Projectile prefab has not been set for {0}", name));
            ActivateCooldown(true);
            currentCooldown = data.baseStats.cooldown;
            return false;
        }

        // If this is the first time the attack has been fired,
        // We reset the currentSpawnCount.
        if (currentCooldown <= 0)
        {
            currentSpawnCount = 1;
            currentSpawnYOffset = 0;
        }
        // Otherwise, calculate the angle and offset of our spawned projectile.
        float spawnDir = Mathf.Sign(movement.lastMovedVector.x) * (currentSpawnCount % 2 == 0 ? -1 : 1);
        float spawnDirY = Mathf.Sign(movement.lastMovedVector.y);
        Vector2 spawnOffset = new Vector2(Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax), currentSpawnYOffset);

        // And spawn a copy of the projectile 
        Projectile prefab = Instantiate(
            currentStats.projectilePrefab,
            owner.transform.position + (Vector3)(spawnOffset),
            Quaternion.identity
        );
        prefab.owner = owner; // Set ourselves to be the owner.

        // Flip the projectile's particle system sprite.
        if (spawnDir < 0)
        {
            ParticleSystem ps = prefab.GetComponent<ParticleSystem>();
            if (ps)
            {
                
                ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
                psr.flip = new Vector3(1, 0, 0);
                psr.pivot = new Vector3(-0.4f, 0, 0);
                
            }
            else
            {
                
                prefab.transform.localScale = new Vector3(
                    -Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
            }
        }
        if (spawnDirY > 0)
        {
            ParticleSystem ps = prefab.GetComponent<ParticleSystem>();
            ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
            psr.pivot = new Vector3(psr.pivot.x, 0.1f, 0f);
        }
        else if (spawnDirY < 0)
        {
            ParticleSystem ps = prefab.GetComponent<ParticleSystem>();
            ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
            psr.pivot = new Vector3(psr.pivot.x, -0.1f, 0f);
        }

        // Assign the stats.
        prefab.weapon = this;
        ActivateCooldown(true);
        attackCount--;

        // Determine where the next projectile should spawn.
        currentSpawnCount++;
        if (currentSpawnCount > 1 && currentSpawnCount % 2 == 0)
            currentSpawnYOffset += 1;


        // Do we perform another attack?
        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = data.baseStats.projectileInterval;
        }

        return true;
    }
    
}
