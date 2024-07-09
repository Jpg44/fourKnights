using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    public Camera cam;
    public Image healthBarImage;
    public MonsterState monsterState;
    void Start()
    {
        //맨처음 이름 적어주기
        Transform monsterNameSpace = transform.Find("MonsterName");
        monsterState = transform.parent.parent.parent.parent.GetComponent<MonsterState>();
       // monsterState = transform.parent.parent.parent.parent.GetComponent<MonsterState>();
        print(monsterState.monsterName);
        if (monsterNameSpace != null)
        {
            switch (transform.parent.name)
            {
                case "Rato": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "라토"; break;
                case "Anto": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "엔토"; break;
                case "Scopionto": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "스콜피온토"; break;
                case "Wormo": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "워모"; break;
                case "Beeto": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "비토"; break;
                case "Chicky": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "치키"; break;
                case "Sandga": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "샌드가"; break;
                case "Spider": monsterNameSpace.GetComponent<TextMeshProUGUI>().text = "스파이더"; break;
            }
        }
    }

    void Update()
    {
        if (cam == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                cam = player.GetComponentInChildren<Camera>(); // Camera 컴포넌트를 찾아 할당
            }
        }
        if (cam != null)
        {
            transform.rotation = cam.transform.rotation;
            SetHealth();
            StartCoroutine(MonsterHit());
        }
    }
    private void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
        }
    }

    public void SetHealth()
    {
        healthBarImage.fillAmount = (float)monsterState.monsterHp / monsterState.monsterMaxHp;
    }
    //데미지 영수증
    IEnumerator MonsterHit()
    {
        if (monsterState.monsterIsHit)
        {
            monsterState.monsterIsHit = false;
            print("이민큼 맞음" + monsterState.monsterHitDamage);
            Transform monsterNameSpace = transform.Find("Damage");
            print(monsterState.monsterHitDamage);
            if (monsterNameSpace != null )
            {
                monsterNameSpace.gameObject.SetActive(true);
                monsterNameSpace.GetComponent<TextMeshProUGUI>().text = monsterState.monsterHitDamage.ToString();
                monsterState.hitAudioSource.Play();
                yield return new WaitForSeconds(0.3f);
                monsterNameSpace.gameObject.SetActive(false);
            }
        }
    }
}
