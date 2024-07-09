using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPart : MonoBehaviour
{
    public void TakeDamage(int damage) 
    {
        Debug.Log("보스부위 맞추기");
        BossStatus.Instance.TakeDamage(damage);
    }
}
