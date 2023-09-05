using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    // Current stats
    
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;


    [HideInInspector]
    public GameObject currentCharacter;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //Check if the value changed
            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentHealthDisplay.text = "Health:" + currentHealth;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //Check if the value changed
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentRecoveryDisplay.text = "Recovery:" + currentRecovery;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //Check if the value changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentMoveSpeedDisplay.text = "Move Speed:" + currentMoveSpeed;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //Check if the value changed
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentMightDisplay.text = "Might:" + currentMight;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            //Check if the value changed
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
                //Add any additional logic here that needs to be executed when the value changes
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            //Check if the value changed
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.CurrentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
                //Add any additional logic here that needs to be executed when the value changes 
            }
        }
    }
    #endregion




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
    

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public Text levelText;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector._instance.DestroySingleton();


        inventory = GetComponent<InventoryManager>();

        // Assign the variables
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;
        currentCharacter = characterData.StartingCharacter;


        // Spawn the starting Weapon
        SpawnWeapon(characterData.StartingWeapon);
        // Spawn the starting Character
        SelectCharacterType(characterData.StartingCharacter);
    }

    private void Start()
    {
        // Initialize the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        // Set the current stats display
        GameManager.Instance.CurrentHealthDisplay.text = "Health:" + currentHealth;
        GameManager.Instance.CurrentRecoveryDisplay.text = "Recovery:" + currentRecovery;
        GameManager.Instance.CurrentMoveSpeedDisplay.text = "Move Speed:" + currentMoveSpeed;
        GameManager.Instance.CurrentMightDisplay.text = "Might:" + currentMight;
        GameManager.Instance.CurrentProjectileSpeedDisplay.text = "Projectile Speed:" + currentProjectileSpeed;
        GameManager.Instance.CurrentMagnetDisplay.text = "Magnet:" + currentMagnet;

        GameManager.Instance.AssignChosenCharacterUI(characterData); 

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

        UpdateExpBar();
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

            UpdateLevelText();

            GameManager.Instance.StartLevelUp();
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
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }

    public void Kill()
    {
        if (!GameManager.Instance.isGameOver)
        {
            GameManager.Instance.AssignLevelReachedUI(level);
            GameManager.Instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.Instance.GameOver(); 
        }
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
        
        
    }

    void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {

            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

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
        inventory.AddWeapon(weaponIndex,spawnedWeapon.GetComponent<WeaponController>()); //Add the weapon to it's inventory slot

        weaponIndex++;
       
        
    }
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //Checking if the slots are full, and returning if it is 
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1) //must be -1 beacuse a list starts from 0
        {
            Debug.Log("Inventory slots already full");
            return;
        }
        //Spawn the starting passiveItem
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Add the weapon to it's inventory slot

        passiveItemIndex++;
        

    }

    public void SelectCharacterType(GameObject character)
    {
        //Select Character Type 
        GameObject spawnedCharacter = Instantiate(character, transform.position, Quaternion.identity);
        spawnedCharacter.transform.SetParent(transform);
        Debug.Log("Karakter Doðdu");
        
    }
}
