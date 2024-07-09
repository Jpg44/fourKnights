using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpFlower : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem hp_effect;
    void OnEnable()
    {
        // Start 함수에서 InvokeRepeating을 호출하여 0초 후에 Heal을 시작하고, 이후 1초마다 반복 호출
        hp_effect.Stop();
        InvokeRepeating("Heal", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(DryadStatus.Instance.currentHealth);
    }

    void Heal()
    {
        // DryadStatus의 인스턴스에 접근하여 currentHealth를 1 증가시키기
        
        if (DryadStatus.Instance != null)
        {
            DryadStatus.Instance.currentHealth += 3;

            // 현재 체력이 최대 체력을 초과하지 않도록 제한
            DryadStatus.Instance.currentHealth = Mathf.Min(DryadStatus.Instance.currentHealth, DryadStatus.Instance.maxHealth);
        }
        hp_effect.Play();
    }
}
