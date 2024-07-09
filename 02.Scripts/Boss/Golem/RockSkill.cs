using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSkill : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip rock_sound;

    private AudioSource audioSource;

    public ParticleSystem rock;

    public GameObject rock2;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        rock.Stop();
        StartCoroutine(RockActivate());
    }

    // Update is called once per frame
    IEnumerator RockActivate()
    {
        yield return new WaitForSeconds(1f);
        rock.Play();
        audioSource.clip = rock_sound;
        audioSource.Play();
        rock2.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        audioSource.Stop();

        gameObject.SetActive(false);
    }
}
