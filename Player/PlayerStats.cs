using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // Current stats
    
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;
    [HideInInspector]
    public GameObject currentCharacter;

    public List<GameObject> spawnedWeapons;

    // Experience and level of the player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class for defining a level range and the corresponding experience cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;

    }

    // I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        // Assign the variables
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;
        currentCharacter = characterData.StartingCharacter;


        // Spawn the starting Weapon
        SpawnWeapon(characterData.StartingWeapon);
    }

    private void Start()
    {
        // Initialize the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease; 
    }

    private void Update()
    {
        if (invincibilityTimer > 0 )
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }
    public void TakeDamage(float dmg)
    {
        //If the player is not currently invincible, reduce health and start invinciblity
        if (!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth < 0)
            {
                Kill();
            }
        }
       
    }

    public void Kill()
    {
        Debug.Log("Player Is Dead");
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
        
        
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {

            currentHealth += currentRecovery * Time.deltaTime;

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        //Spawn the starting weapon
        GameObject spawnedWeapon =Instantiate(weapon,transform.position,Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        spawnedWeapons.Add(spawnedWeapon);
    }
}
