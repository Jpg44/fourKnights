using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThornNeedle : MonoBehaviour
{
    public ParticleSystem needle;
    public GameObject needle_colider;
    public NavMeshAgent navMeshAgent;

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audioneedle;

    private AudioSource audioSource;


    void Awake ()
    {
    }
    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.Stop();
        
        needle.Stop();
        StartCoroutine(LaunchNeedle());
    }

    IEnumerator LaunchNeedle()
    {
        BossAttackController.Instance.isActionInProgress = true;
        navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(1f);
        needle.Play();
        audioSource.clip = audioneedle;
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        needle_colider.SetActive(true);
        yield return new WaitForSeconds(2.1f);
        navMeshAgent.speed = 2f;
        needle.Stop();
        needle_colider.SetActive(false);
        BossAttackController.Instance.isActionInProgress = false;
        gameObject.SetActive(false);

    }
}
