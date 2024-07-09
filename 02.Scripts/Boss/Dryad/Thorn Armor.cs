using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornArmor : MonoBehaviour
{
    
    public ParticleSystem shield;
    public ParticleSystem thorn;
    public static bool isArmorActive = false; 

    void Awake()
    {
    }

    private void OnEnable()
    {
        shield.Stop();
        thorn.Stop();
        StartCoroutine(ActivateArmor());
    }

    // Update is called once per frame
    IEnumerator ActivateArmor()
    {
        DryadStatus.Instance.defense += 2500;
        shield.Play();
        yield return new WaitForSeconds(2.5f);
        shield.Stop();
        isArmorActive = true;
        thorn.Play();
        yield return new WaitForSeconds(10f);  // 가시 효과 지속 시간
        DryadStatus.Instance.defense -= 2500;
        thorn.Stop();
        isArmorActive = false;  // 가시갑옷 비활성화
        // BossAttackController.Instance.isActionInProgress = false;
        gameObject.SetActive(false);
    }
}
