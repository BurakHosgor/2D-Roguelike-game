using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[System.Obsolete("Replaced by Projectile Weapon")]
public class KnifeBehaviour : ProjectileWeaponBehaviour
{



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    private void Update()
    {
        transform.position += direction * currentSpeed * Time.deltaTime;
    }
}
