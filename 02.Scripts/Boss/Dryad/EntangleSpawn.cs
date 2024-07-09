using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntangleSpawn : MonoBehaviour
{
 
    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audiotangle;

    private AudioSource audioSource;
    
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(LaunchEntangle());
    }

    IEnumerator LaunchEntangle()
    {
        audioSource.clip = audiotangle;
        audioSource.Play();
        
        // 게임 오브젝트가 활성화된 후 잠시 대기
        yield return new WaitForSeconds(0);  // 여기서 필요한 만큼 대기 시간 조정 가능

        // Player 태그를 가진 모든 오브젝트를 찾음
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 각 플레이어 위치에 entangle 오브젝트 생성
        foreach (GameObject player in players)
        {
            GameObject entangle = ObjectPoolManager.Instance.GetEntangle();  // 풀에서 entangle 오브젝트 가져오기
            entangle.transform.position = player.transform.position;  // 플레이어 위치에 entangle 위치 설정
            entangle.SetActive(true);  // entangle 오브젝트 활성화
        }
        BossAttackController.Instance.isActionInProgress = false;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
