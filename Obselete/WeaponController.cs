using System;
using UnityEngine;


/// <summary>
/// Base script for all weapon controllers. WeaponControllers are responsible for managing the cooldowns 
/// of the weapons and firing them when the cooldown expires.
/// </summary>

[Obsolete("Replaced by the Weapon Component, as this script uses the old WeaponScriptable Object class for its weapon data. ",false)]
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
  
    float currentCooldown;
    public WeaponScriptableObject weaponData;
    protected PlayerMovement pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentCooldown = weaponData.CooldownDuration;
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown < 0)
        {
            Attackt();
        }
    }

    protected virtual void Attackt()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
} 
