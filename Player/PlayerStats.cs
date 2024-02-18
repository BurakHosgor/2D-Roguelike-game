using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;

    float health;
    

    [HideInInspector]
    public GameObject currentCharacter;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return health; }
        // If we try and set the current health, the UI interface
        // on the pause screen will also be updated.
        set
        {
            //Check if the value changed
            if (health != value)
            {
                health= value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                        );
                    
                }
                
            }
        }
    }
    public float MaxHealth
    {
        get { return actualStats.maxHealth; }

        // If we try and set the max health, the UI interface
        // on the pause screen will also be updated.

        set
        {
            // Check if the value has changed
            if (actualStats.maxHealth != value)
            {
                actualStats.maxHealth= value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = string.Format(
                        "Health: {0} / {1}",
                        health, actualStats.maxHealth
                        );
                }
                // Update the real time value of the stat
                // Add any additional logic here that needs to be executed when the value changes
            }
        }
    }
    public float CurrentRecovery
    {
        get { return Recovery; }
        set { Recovery = value; }
    }
    public float Recovery
    {
        get { return actualStats.recovery; }
        set
        {
            //Check if the value changed
            if (actualStats.recovery != value)
            {
                actualStats.recovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery:" + actualStats.recovery;
                }
                
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return MoveSpeed; }
        set {  MoveSpeed = value; }
    }
    public float MoveSpeed
    {
        get { return actualStats.moveSpeed; }
        set
        {
            //Check if the value changed
            if (actualStats.moveSpeed != value)
            {
                actualStats.moveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed:" + actualStats.moveSpeed;
                }
                
            }
        }
    }
    public  float CurrentMight
    {
        get { return Might; }
        set { Might = value; }
    }
    public float Might
    {
        get { return actualStats.might; }
        set
        {
            //Check if the value changed
            if (actualStats.might != value)
            {
                actualStats.might = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might:" + actualStats.might;
                }
               
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return Speed; }
        set { Speed= value; }
    }
    public float Speed
    {
        get { return actualStats.speed; }
        set
        {
            //Check if the value changed
            if (actualStats.speed != value)
            {
                actualStats.speed= value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + actualStats.speed;
                }
                
            }
        }
    }
    public float CurrentMagnet
    {
        get { return Magnet; }
        set { Magnet = value; }
    }
    public float Magnet
    {
        get { return actualStats.magnet; }
        set
        {
            //Check if the value changed
            if (actualStats.magnet != value)
            {
                actualStats.magnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + actualStats.magnet;
                }
                
            }
        }
    }
    #endregion

    public ParticleSystem damageEffect;


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
    

    PlayerInventory inventory;
    PlayerCollector collector;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;

    
    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();


        inventory = GetComponent<PlayerInventory>();
        collector = GetComponentInChildren<PlayerCollector>();

        // Assign the variables
        baseStats = actualStats = characterData.stats;
        collector.SetRadius(actualStats.magnet);
        health = actualStats.maxHealth;
        
    }

    private void Start()
    {
        // Spawn the starting weapon
        inventory.Add(characterData.StartingWeapon);
        // Initialize the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        // Set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Health:" + CurrentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery:" + CurrentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed:" + CurrentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might:" + CurrentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed:" + CurrentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet:" + CurrentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData); 

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();

        
    }

    private void Update()
    {
        if (invincibilityTimer > 0 )
        {
            invincibilityTimer -= Time.deltaTime;
        }
        // If the invincibility timer has reached 0, set the invincibility flag to false
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualStats += p.GetBoosts();
            }
        }
        // Update the PlayerCollector's Radius.
        collector.SetRadius(actualStats.magnet);
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;
 
        LevelUpChecker();

        UpdateExpBar();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            // Level up the player and reduce their experience by the cap
            level++;
            experience -= experienceCap;

            // Find the experience cap increase for the current level range
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

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
            
        }
    }

    void UpdateExpBar()
    {
        //Update exp bar fill amount
        expBar.fillAmount = (float)experience / experienceCap;
    }

    void UpdateLevelText()
    {
        //update level text 
        levelText.text = "LV " + level.ToString();
    }

    public void TakeDamage(float dmg)
    {
        //If the player is not currently invincible, reduce health and start invinciblity
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            // if there is a damage effect assigned, play it.
            
            if(damageEffect) Destroy(Instantiate(damageEffect,transform.position,Quaternion.identity), 5f);

            
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            

            if (CurrentHealth < 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
     
    }


    void UpdateHealthBar()
    {
        //Update health bar
        healthBar.fillAmount = CurrentHealth / actualStats.maxHealth;
    }

    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
           
            GameManager.instance.GameOver(); 
        }
    }

    public void RestoreHealth(float amount)
    {
        // Only heal the player if their current health is less than their maximum health
        if (CurrentHealth < actualStats.maxHealth)
        {
            CurrentHealth += amount;
            // Make sure the player's health doesn't exceed their maximum health
            if(CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
        
        
    }

    void Recover()
    {
        if (CurrentHealth < actualStats.maxHealth)
        {

            CurrentHealth += CurrentRecovery * Time.deltaTime;

            // Make sure the player's health doesn't exceed their maximum health
            if (CurrentHealth > actualStats.maxHealth)
            {
                CurrentHealth = actualStats.maxHealth;
            }
        }
    }
    [System.Obsolete("Old function that is kept to maintain compatibility with the InventoryManager. Will be removed soon.")]
    public void SpawnWeapon(GameObject weapon)
    {
        //Checking if the slots are full, and returning if it is 
        if ( weaponIndex >= inventory.weaponSlots.Count -1 ) //must be -1 beacuse a list starts from 0
        {
            Debug.Log("Inventory slots already full");
            return;
        }
        //Spawn the starting weapon
        GameObject spawnedWeapon =Instantiate(weapon,transform.position,Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        //inventory.AddWeapon(weaponIndex,spawnedWeapon.GetComponent<Weapon>()); //Add the weapon to it's inventory slot

        weaponIndex++;
       
        
    }
    [System.Obsolete("No need to spawn passive items directly now.")]
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Checking if the slots are full, and returning if it is 
        if (passiveItemIndex >= inventory.passiveSlots.Count - 1) //must be -1 beacuse a list starts from 0
        {
            Debug.Log("Inventory slots already full");
            return;
        }
        //Spawn the starting passiveItem
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        //inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Add the weapon to it's inventory slot

        passiveItemIndex++;
        

    }

    public void SelectCharacterType(GameObject character)
    {
        //Select Character Type 
        GameObject spawnedCharacter = Instantiate(character, transform.position, Quaternion.identity);
        spawnedCharacter.transform.SetParent(transform);
        
    }
}
