using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlowerStatus : MonoBehaviourPunCallbacks
{
    public int maxHealth = 150;
    public int defense = 30;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {

        Debug.Log("아파");
        photonView.RPC("SendDamageToMaster", RpcTarget.MasterClient, damage);
    }

    // 마스터 클라이언트가 피해량을 받아서 처리
    [PunRPC]
    public void SendDamageToMaster(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트에서 피해 수신: " + damage);
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

        

            // 다른 클라이언트들에게 현재 체력 동기화
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    // 체력 및 UI 갱신 메서드
   

    // 다른 클라이언트에서 호출될 RPC 메서드
    [PunRPC]
    public void UpdateHealthRPC(int newHealth)
    {
        currentHealth = newHealth;
        
    }



    void Die()
    {
        // 꽃이 사망할 때의 처리 로직을 여기에 작성
        Debug.Log("Flower died!");
        Destroy(gameObject);
    }
}
