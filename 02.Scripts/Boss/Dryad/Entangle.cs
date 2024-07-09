using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entangle : MonoBehaviour
{
    public ParticleSystem entangle_effect;
    public GameObject entangle_colider;

    void OnEnable()
    {
        
        entangle_effect.Stop();
        StartCoroutine(StartEntangle());
    }

    // Update is called once per frame
    IEnumerator StartEntangle()
    {
        yield return new WaitForSeconds(1);
        entangle_effect.Play();
        // yield return new WaitForSeconds(0.2f);
        entangle_colider.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        entangle_colider.SetActive(false);
        ObjectPoolManager.Instance.ReturnEntangle(gameObject);
        // yield return new WaitForSeconds(0.5f);

    }
}
