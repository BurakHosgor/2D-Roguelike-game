using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Obsolete("Replaced by Projectiles Weapon")]
public class KnifeController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attackt()
    {
        base.Attackt();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position;
        spawnedKnife.GetComponent<KnifeBehaviour>().DirectionChecker(pm.lastMovedVector);
    }

}
