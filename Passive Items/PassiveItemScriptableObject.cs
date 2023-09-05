using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="PassiveItemScriptableObject",menuName ="ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get =>  multiplier; set => multiplier = value; }

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
}
