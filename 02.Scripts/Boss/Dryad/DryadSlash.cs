using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryadSlash : MonoBehaviour
{
    public ParticleSystem slash;
    public GameObject slash_colider;

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audioslash;

    private AudioSource audioSource;
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.Stop();
        slash.Stop();
        StartCoroutine(ActivateSlash());
    }

    IEnumerator ActivateSlash()
    {
        yield return new WaitForSeconds(1f);
        slash.Play();
        audioSource.clip = audioslash;
        audioSource.Play();
        slash_colider.SetActive(true);
        yield return new WaitForSeconds(1f);
        slash_colider.SetActive(false);
        slash.Stop();
        BossAttackController.Instance.isActionInProgress = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        
    }
}
