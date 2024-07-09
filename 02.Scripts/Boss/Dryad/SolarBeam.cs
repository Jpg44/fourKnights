using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBeam : MonoBehaviour
{
    
    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audiosolar;

    private AudioSource audioSource;
    public BoxCollider beamCollider; // BoxCollider를 인스펙터에서 할당
    public ParticleSystem solarbeam_effect;
    public int damage = 100; // 솔라빔이 입힐 피해량
    public float activationDelay = 2.5f; // 솔라빔이 활성화되기까지의 지연 시간

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.Stop();
        
        solarbeam_effect.Stop();
        StartCoroutine(ActivateBeamAfterDelay(activationDelay));
    }


    IEnumerator ActivateBeamAfterDelay(float delay)
    {
        
        DryadStatus.Instance.currentMana -=5;
        yield return new WaitForSeconds(delay);
        if (beamCollider != null)
        {
            solarbeam_effect.Play();
            beamCollider.enabled = true; // 지연 후 콜라이더 활성화
        }
        audioSource.clip = audiosolar;
        audioSource.Play();

        yield return new WaitForSeconds(4f);
        solarbeam_effect.Stop();
        beamCollider.enabled = false;
        BossAttackController.Instance.isActionInProgress = false;
        gameObject.SetActive(false);


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus_Test playerStatus = other.GetComponent<PlayerStatus_Test>();
            if (playerStatus != null)
            {
                Debug.Log("Player hit by Solar Beam.");
                playerStatus.TakeDamage(damage);
            }
        }
    }
}
