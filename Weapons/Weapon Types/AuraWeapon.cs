using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraWeapon : Weapon
{
    protected Aura currentAura;
    bool isEquip = false;

    // Update is called once per frame
    protected override void Update()
    {
        if (isEquip)
        {
            float area = GetArea();
            currentAura.transform.localScale = new Vector3(area, area, area);
        }
    }

    public override void OnEquip()
    {
        // Try to replace the aura the weapon has with a new one.
        if (currentStats.auraPrefab)
        {
            if (currentAura) Destroy(currentAura);
            currentAura = Instantiate(currentStats.auraPrefab, transform);
            currentAura.weapon = this;
            currentAura.owner = owner;
            isEquip = true;
            
        }
    }

    public override void OnUnequip()
    {
        if (!currentAura) Destroy(currentAura);
    }

    public override bool DoLevelUp()
    {
        if (!base.DoLevelUp()) return false;

        // If there is an aura attached to this weapon, we update the aura.
        if (currentAura)
        {
            currentAura.transform.localScale = new Vector3(currentStats.area, currentStats.area, currentStats.area);
        }
        return true;
    }
}
