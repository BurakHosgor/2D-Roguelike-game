using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRingWeapon : ProjectileWeapon
{
    List<EnemyStats> allSelectedEnemies = new List<EnemyStats>();

    public override bool CanAttack()
    {
        Debug.Log(string.Format("Lighting Ring can attack: {0}", base.CanAttack()));
        return base.CanAttack();
    }
    protected override bool Attack(int attackCount = 1)
    {
        // If there is no projectile assigned, set the weapon on cooldown.
        if (!currentStats.hitEffect || !CanAttack())
        {
            currentCooldown = data.baseStats.cooldown;
            return false;
        }

        // If the cooldown is less than 0, this is the first firing of the weapon.
        // Refresh the array of selected enemies.
        if (currentCooldown <= 0)
        {
            allSelectedEnemies = new List<EnemyStats>(FindObjectsOfType<EnemyStats>());
            currentCooldown = currentStats.cooldown;
            currentAttackCount = attackCount;
        }
            

        // Find an enemy in the map to strike lightning.
        EnemyStats target = PickEnemy();

        if (target)
        {
            
            DamageArea(target.transform.position, currentStats.area, currentStats.damage + Random.Range(0, currentStats.damageVariance));
            Instantiate(currentStats.hitEffect, target.transform.position, Quaternion.identity);
        }
        // If we have more than 1 attack count.
        if (attackCount > 0)
        {
            currentAttackCount = attackCount - 1;
            currentAttackInterval = currentStats.projectileInterval;
        }
        
        
        return true;
    }

    // Randomly picks an enemy on screen.
    EnemyStats PickEnemy()
    {
        EnemyStats target = null;
        while (!target && allSelectedEnemies.Count > 0)
        {
            // Check if the enemy is on screen.
            target = allSelectedEnemies[Random.Range(0, allSelectedEnemies.Count)];
            Renderer r = target.GetComponent<Renderer>();
            if (r && !r.isVisible)
            {
                allSelectedEnemies.Remove(target);
                continue;

            }
        }
        allSelectedEnemies.Remove(target); 
        return target;
    }
    // Deals damage in an area.
    void DamageArea(Vector2 position, float radius, float damage)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D t in targets)
        {
            EnemyStats es = t.GetComponent<EnemyStats>();
            if (es) es.TakeDamage(damage, transform.position); 
            
        }
    }
}
