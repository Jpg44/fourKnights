using System.Collections;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public ParticleSystem vine_particle;

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audiovine;

    private AudioSource audioSource;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        LaunchVine();
        // vine_particle.Stop();
    }

    void LaunchVine()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameObject vine_cube = ObjectPoolManager.Instance.GetVineCube(); // ObjectPoolManager에서 vine_cube 가져오기
            vine_cube.transform.position = transform.position;  // 발사 위치 설정
            Vector3 direction = (player.transform.position - transform.position).normalized;
            vine_cube.GetComponent<Rigidbody>().velocity = direction * 10;
        }
        StartCoroutine(effect());
    }

    IEnumerator effect()
    {
        Debug.Log("덩굴 이펙트");
        vine_particle.Play();
        audioSource.clip = audiovine;
        audioSource.Play();
        yield return new WaitForSeconds(1.5f);
        // vine_particle.Stop();
        BossAttackController.Instance.isActionInProgress = false;
        gameObject.SetActive(false);
    }
}
