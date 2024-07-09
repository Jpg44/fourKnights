using UnityEngine;
using UnityEngine.SceneManagement;

public class DieModalScript : MonoBehaviour
{
    public CharacterManager characterManager;
    void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null)
        {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "VillageScene")
        {
            Debug.Log("VillageScene detected, disabling DieModal");
            characterManager.currentHP = characterManager.maxHP;
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        Debug.Log("DieModal enabled, showing cursor");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        Debug.Log("DieModal disabled, hiding cursor");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
