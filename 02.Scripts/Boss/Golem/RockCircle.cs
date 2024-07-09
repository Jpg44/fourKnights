using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCircle : MonoBehaviour
{
    public GameObject objectToSpawn; // 스폰할 스킬의 프리팹
    public Vector3 spawnAreaCenter; // 스폰 범위의 중심
    public float spawnRadius; // 스폰할 원의 반경
    public int numberOfObjects; // 스폰할 오브젝트 수

    // Start is called before the first frame update
    void Start()
    {
        SpawnObjectsInCircle();
    }

    // 원형 범위 내에서 여러 개의 오브젝트를 스폰하는 메서드
    void SpawnObjectsInCircle()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInCircle(spawnAreaCenter, spawnRadius);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    // 주어진 중심과 반경 내에서 랜덤 위치를 반환하는 메서드
    Vector3 GetRandomPositionInCircle(Vector3 center, float radius)
    {
        Debug.Log("오브젝트 소환");
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(0, radius);
        return center + new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);
    }
}
