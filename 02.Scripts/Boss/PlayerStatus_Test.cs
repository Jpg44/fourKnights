using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus_Test : MonoBehaviour
{
    
    public static PlayerStatus_Test Instance { get; private set; } // 싱글톤 인스턴스

    public int maxHealth = 1000;
    public int currentHealth;
    public int attackPower = 50;
    public int defense = 0;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("키누름");
            BossStatus.Instance.TakeDamage(attackPower);
        }


    }


    public void TakeDamage(int damage)
    {
        // int damageTaken = Mathf.Max(damage - defense, 0);
        currentHealth -= damage;

        Debug.Log(currentHealth);

    }
}
