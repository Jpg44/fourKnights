using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenHole : MonoBehaviour
{
    public float pullStrength = 0.1f; // 끌어당기는 힘의 강도
    public float pullRadius = 500f; // 끌어당기는 반경

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audiohole;

    private AudioSource audioSource;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        StartCoroutine(Deactive());
    }

    void Update()
    {
        // 주변의 모든 Collider를 pullRadius 반경 내에서 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);

        // 각 Collider에 대하여
        foreach (Collider collider in colliders)
        {
            // 태그가 "Player"인지 확인
            if (collider.tag == "Player")
            {
                // CharacterController 컴포넌트를 가져오기
                CharacterController controller = collider.GetComponent<CharacterController>();

                // CharacterController가 있다면
                if (controller != null)
                {
                    // 오브젝트를 블랙홀 중심으로 천천히 이동시키기
                    Vector3 forceDirection = transform.position - collider.transform.position;
                    Vector3 moveDirection = forceDirection.normalized * pullStrength * Time.deltaTime;
                    controller.Move(moveDirection);
                }
            }
        }
    }

    IEnumerator Deactive()
    {
        
        audioSource.clip = audiohole;
        audioSource.Play();
        yield return new WaitForSeconds(3.5f);
        BossAttackController.Instance.isActionInProgress = false;
        gameObject.SetActive(false);
    }
}
