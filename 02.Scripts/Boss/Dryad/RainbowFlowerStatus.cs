using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RainbowFlowerStatus : MonoBehaviourPunCallbacks
{
    public int maxHealth = 150;
    public int defense = 30;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // void Update()
    // {
    //     if (currentHealth <= 0)
    //     {
    //         Die();
    //     }
    // }

    public void TakeDamage(int damage)
    {
        Debug.Log("아파요!");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            photonView.RPC("DieRPC", RpcTarget.Others);
        }
    }

    void Die()
    {
        // 꽃이 사망할 때의 처리 로직을 여기에 작성
        Debug.Log("Flower died!");
        RainbowFlower.Instance.FlowerDestroyed(gameObject);
        gameObject.SetActive(false);
    }

    [PunRPC]
    void DieRPC()
    {
        // 꽃이 사망할 때의 처리 로직을 여기에 작성
        Debug.Log("Flower died!");
        RainbowFlower.Instance.FlowerDestroyed(gameObject);
        gameObject.SetActive(false);
    }
}
