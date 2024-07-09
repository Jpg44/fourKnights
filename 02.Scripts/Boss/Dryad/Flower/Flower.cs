using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Flower : MonoBehaviourPunCallbacks
{
    public GameObject flower1;
    public GameObject flower2;
    public GameObject flower3;
    public ParticleSystem flower_effect;

    private Renderer seedRenderer; // 씨앗의 Renderer 컴포넌트
    private bool isBlossom = false;

    void Start()
    {
        seedRenderer = GetComponent<Renderer>(); // 씨앗의 Renderer 컴포넌트를 가져옴
        flower_effect.Stop();
    }

    void Update()
    {
        float currentAngle = TimeController.Instance.GetCurrentLightAngleX();
        // Debug.Log($"현재 각도: {currentAngle}, isBlossom: {isBlossom}");

        if (PhotonNetwork.IsMasterClient)
        {
            if (currentAngle >= 50 && currentAngle <= 90 && !isBlossom)
            {
                Debug.Log("꽃 피우기 조건 만족");
                GrowRandomFlower();
                isBlossom = true;
            }
            else if (currentAngle < 50 || currentAngle > 90)
            {
                Debug.Log("꽃 피울 수 있는 상태로 리셋");
                isBlossom = false;
            }
        }
    }


    void GrowRandomFlower()
    {
        // 모든 꽃을 비활성화
        flower1.SetActive(false);
        flower2.SetActive(false);
        flower3.SetActive(false);

        // 랜덤하게 하나의 꽃을 활성화
        int randomIndex = Random.Range(0, 3); // 0, 1, 2 중 하나 랜덤 선택
        switch(randomIndex)
        {
            case 0:
                flower1.SetActive(true);
                break;
            case 1:
                flower2.SetActive(true);
                break;
            case 2:
                flower3.SetActive(true);
                break;
        }

        Debug.Log("꽃 피우기");
        // 씨앗의 렌더러를 비활성화하여 씨앗을 숨김
        if (seedRenderer != null)
            seedRenderer.enabled = false;

        photonView.RPC("GrowRandomFlowerRPC", RpcTarget.Others, randomIndex);
    }



    [PunRPC]
    void GrowRandomFlowerRPC(int randomIndex)
    {
        // 모든 꽃을 비활성화
        flower1.SetActive(false);
        flower2.SetActive(false);
        flower3.SetActive(false);
        switch(randomIndex)
        {
            case 0:
                flower1.SetActive(true);
                break;
            case 1:
                flower2.SetActive(true);
                break;
            case 2:
                flower3.SetActive(true);
                break;
        }
        Debug.Log("꽃 피우기RPC");
        // 씨앗의 렌더러를 비활성화하여 씨앗을 숨김
        if (seedRenderer != null)
            seedRenderer.enabled = false;
    }



    IEnumerator Effect()
    {
        flower_effect.Play();
        yield return new WaitForSeconds(2);
        flower_effect.Stop();
    }
}
