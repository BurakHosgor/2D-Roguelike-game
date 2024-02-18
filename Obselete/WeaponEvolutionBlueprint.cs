using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName =("WeaponEvolutionBlueprint"), menuName =("ScriptableObjects/WeaponEvolutionBlueprint"))]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    //public WeaponScriptableObject baseWeaponData;
    public PassiveData catalystPassiveItemData;
    //public WeaponScriptableObject evolvedWeaponData;
    public GameObject evolvedWeapon;
}
