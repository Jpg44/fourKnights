using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaFlower : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem mp_effect;

    void OnEnable()
    {
        // 0초 후에 시작해서 5초마다 IncreaseMana 함수를 반복 호출
        mp_effect.Stop();
        InvokeRepeating("IncreaseMana", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(DryadStatus.Instance.currentMana);
    }

    void IncreaseMana()
    {
        // DryadStatus의 인스턴스에 접근하여 currentMana를 1 증가
        if (DryadStatus.Instance != null)
        {
            DryadStatus.Instance.currentMana += 2;
        }

        StartCoroutine(effect());
        
    }

    IEnumerator effect()
    {
        mp_effect.Play();
        Debug.Log("이펙트");
        yield return new WaitForSeconds(1.5f);
        mp_effect.Stop();
    }
}
