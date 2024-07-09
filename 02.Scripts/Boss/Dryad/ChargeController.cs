using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeController : MonoBehaviour
{
    public ParticleSystem charge;

    void Awake()
    {
        charge.Stop();
    }

    public void StartEffectWithCallback(System.Action callback)
    {
        StartCoroutine(EffectCoroutine(callback));
    }

    private IEnumerator EffectCoroutine(System.Action callback)
    {
        charge.Play();
        Debug.Log("ddddd");
        yield return new WaitForSeconds(2.5f);
        charge.Stop();
        callback?.Invoke();
    }
}
