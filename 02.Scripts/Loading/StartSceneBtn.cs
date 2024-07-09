using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartSceneBtn : MonoBehaviour
{
    public void LoadCharacterScene()
    {
        SceneManager.LoadScene("CharacterScene");
    }
}
