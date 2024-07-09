using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossSkill_Rock : MonoBehaviourPunCallbacks
{
    public float knockbackForce = 10f; // 플레이어를 밀어낼 힘의 크기
    public float knockbackDuration = 0.5f; // 플레이어가 날아가는 지속 시간

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audioClipPunch;

    private AudioSource audioSource;

    public CharacterManager characterManager;

    public GameObject rock;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {

            CharacterController playerController = other.GetComponent<CharacterController>();
            PhotonView photonView = other.GetComponent<PhotonView>();
            //PlayerStatus_Test playerStatus_Test = other.GetComponent<PlayerStatus_Test>();
            if (playerController != null)
            {
                Debug.Log("충돌확인");

                audioSource.clip = audioClipPunch;
                audioSource.Play();
                StartCoroutine(KnockbackPlayer(playerController, knockbackDuration));               
                //playerStatus_Test.TakeDamage(BossStatus.Instance.attackPower);
                if (photonView != null && photonView.IsMine)
                {
                    characterManager.SetHP(200);
                }
                StartCoroutine(Delete());
                
            }

        }
    }

    private IEnumerator KnockbackPlayer(CharacterController playerController, float duration)
    {
        
        Debug.Log("밀어내기");
        float timer = 0;
        Vector3 forceDirection = playerController.transform.position - transform.position;
        forceDirection.y = 3; // 수직 방향으로는 힘을 가하지 않음
        forceDirection.Normalize(); // 방향 벡터의 길이를 1로 정규화

        while (timer < duration)
        {
            playerController.Move(forceDirection * knockbackForce * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.15f);
        rock.SetActive(false);
    }
}
