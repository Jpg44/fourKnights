using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIResources : MonoBehaviour
{
    public static PlayerUIResources instance;

    // Start is called before the first frame update
    void Awake()
    {
        // 리소스에서 프리팹 로드
        GameObject playerUIPrefab = Resources.Load<GameObject>("UI/PlayerUI");

        // 프리팹을 씬에 생성
        GameObject playerUIInstance = Instantiate(playerUIPrefab);

        // 부모 객체의 자식으로 설정
        playerUIInstance.transform.SetParent(transform); // 여기서 transform은 부모 객체의 Transform을 가리킵니다.

    }

    // Update is called once per frame
    void Update()
    {

    }
}
