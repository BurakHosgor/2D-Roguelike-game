using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicController : WeaponController
{
    public Vector3 offset;
    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    
    protected override void Attackt()
    {
        base.Attackt();
        GameObject spawnedGarlic = Instantiate(weaponData.Prefab);
        spawnedGarlic.transform.position = transform.position + offset;
        spawnedGarlic.transform.parent = transform;
    }
    
}
