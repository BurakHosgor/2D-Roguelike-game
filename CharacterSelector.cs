using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
   public static CharacterSelector _instance;
    public CharacterScriptableObject characterData;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Debug.LogWarning("Extra" + this + "deleted");
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObject GetData()
    {
        return _instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        characterData = character;
    }
    public void DestroySingleton()
    {
        _instance = null;
        Destroy(gameObject);
    }
}
