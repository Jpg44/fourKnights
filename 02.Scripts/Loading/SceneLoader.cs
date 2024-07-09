using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // CharacterManager characterManager;

    // public GameObject clearBoard;
    public GameObject dieBoard;
    // void Awake()
    // {
    //     if (clearBoard == null)
    //     {
    //         Debug.LogError("dieBoard가 할당되지 않았습니다!");
    //     }
    // }

    public void LoadVillageScene()
    {
        Debug.Log("귀환");

        if (dieBoard != null)
        {
            dieBoard.SetActive(false);
        }
        else
        {
            Debug.LogError("dieBoard가 할당되지 않았습니다!");
        }
;

        // GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        // if (characterManagerObject != null)
        // {
        //     characterManager = characterManagerObject.GetComponent<CharacterManager>();
        // }
        // if (characterManager != null)
        CharacterManager.Instance.currentHP = CharacterManager.Instance.maxHP;
        CharacterManager.Instance.SetHP(0);
        CharacterManager.Instance.isAlive = true;

        // 어디서 죽든지 마을로 갈 것이므로 일단 해당 씬의 room 에서 떠남
        NetworkManager.Instance.isDied = true;
        NetworkManager.Instance.LeaveRoom();
        //SceneManager.LoadScene("VillageScene");
        Cursor.visible = false;                     // 마우스 커서를 보이지 않게 설정
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정
    }

    public void LoadPreviousScene()
    {
        // 현재 씬 인덱스
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int previousSceneIndex = currentSceneIndex - 1;

        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
    }

    public void LoadStartScene()
    {
        NetworkManager.Instance.SetStart();
    }
}
