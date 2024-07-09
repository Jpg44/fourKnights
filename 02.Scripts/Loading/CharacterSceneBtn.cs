using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneBtn : MonoBehaviour
{
    public CharacterManager characterManager;

    public void LoadVillageScene()
    {
        if (characterManager.characterClass == "전사" || characterManager.characterClass == "마법사")
        {
            LoadingSceneController.LoadScene("VillageScene");
            Cursor.visible = false;                     // 마우스 커서를 보이지 않게 설정
            Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정
        }
        else
        {
            Debug.Log("미구현된 직업!");
        }
    }
}
