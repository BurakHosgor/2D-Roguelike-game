using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Obsolete("This Asset type is deprecated. Please recreate it using Monster Survivors > Weapon Data.",false)]
[CreateAssetMenu(fileName = "WeaponScriptableObject",menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }
    // Base stats for weapon
    [SerializeField]
    float damage;
    public float Damage { get =>  damage; private set => damage = value; }
    [SerializeField]
    float speed;
    public float Speed { get =>  speed; private set => speed = value; }
    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration;private set => cooldownDuration = value; }
    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level; //Not meant to be modified in the game [only in editor]
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description; //What is the descripton of this weapon? [if it is an upgrade, place the description of the upgrades ]
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    [SerializeField]
    int evolvedUpgradeToRemove;
    public int EvolvedUpgradeToRemove { get => evolvedUpgradeToRemove; private set => evolvedUpgradeToRemove = value; }
}
