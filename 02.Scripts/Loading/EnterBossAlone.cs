using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterBossAlone : MonoBehaviour
{
     public Button enterBossAloneBtn; // Scene 전환을 위한 버튼 참조

    private void Start()
    {
        // 버튼이 할당되어 있는지 확인하고, 이벤트 리스너를 추가합니다.
        if (enterBossAloneBtn != null)
        {
            enterBossAloneBtn.onClick.AddListener(ChangeToBossScene);
        }
    }

    // 버튼 클릭 시 실행될 메서드
    void ChangeToBossScene()
    {
        LoadingSceneController.LoadScene("Boss");
        Cursor.visible = false;                     // 마우스 커서를 보이지 않게 설정
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정
    }
}
