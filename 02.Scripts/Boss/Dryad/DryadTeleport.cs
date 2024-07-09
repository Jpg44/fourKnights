using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DryadTeleport : MonoBehaviour
{
    public ParticleSystem teleport_effect;
    public GameObject dryad;
    public NavMeshAgent navMeshAgent;

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audioteleport;

    private AudioSource audioSource;
    public float offset = 2.0f; // 플레이어 뒤로 이동할 거리

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        teleport_effect.Stop();
        StartCoroutine(ActivateTeleport());
    }

    IEnumerator ActivateTeleport()
    {
        teleport_effect.Play();
        audioSource.clip = audioteleport;
        audioSource.Play();
        Debug.Log("슝슝");
        yield return new WaitForSeconds(0.7f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            // 랜덤 플레이어 선택
            GameObject selectedPlayer = players[Random.Range(0, players.Length)];
            Debug.Log("쓩쓩!");
            // 플레이어 배후 위치 계산
            Vector3 behindPlayerPosition = selectedPlayer.transform.position - selectedPlayer.transform.forward * offset;
            Debug.Log(selectedPlayer.transform.position);
            // 보스몬스터 위치를 플레이어 배후로 이동
            dryad.transform.position = behindPlayerPosition;

            // 텔레포트 효과 재생
            teleport_effect.Stop();
        }
        yield return new WaitForSeconds(2f);
        BossAttackController.Instance.isActionInProgress = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
