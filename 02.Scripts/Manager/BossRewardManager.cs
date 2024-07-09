using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRewardManager : MonoBehaviour
{
    public static BossRewardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
public void ProcessRewards(BossManager.BossData boss, Difficulty difficulty)
    {
        var difficultyRewardStats = boss.difficultyRewards.Find(drs => drs.difficulty == difficulty);
        if (difficultyRewardStats != null)
        {
            foreach (var reward in difficultyRewardStats.rewards)
            {
                if (Random.Range(0f, 100f) <= reward.dropRate)
                {
                    int quantity = Random.Range(reward.minQuantity, reward.maxQuantity + 1);
                    if (quantity > 0)
                    {
                        // 보상을 플레이어에게 즉시 추가하는 로직
                        // 예: PlayerInventory.Add(reward.type, quantity);
                    }
                }
            }
        }
    }
}