using System.Collections;
using UnityEngine;
using Photon.Pun;

public class SeedPlantingController : MonoBehaviourPunCallbacks
{
    public GameObject seedPrefab; // 스폰할 씨앗의 프리팹
    public Vector3 spawnAreaCenter; // 스폰 범위의 중심
    public float spawnRadius; // 스폰할 원의 반경
    public int numberOfSeeds; // 스폰할 씨앗 수

    private float plantingTimer = 7.0f;

    void Update()
    {
        if (TimeController.Instance.GetCurrentLightAngleX() >= 200)
        {
            plantingTimer -= Time.deltaTime;
            if (plantingTimer <= 0)
            {
                SpawnSeedsInCircle();
                plantingTimer = 5.0f; // 타이머 재설정
                Debug.Log("씨앗 생성");
            }
        }
    }

    void SpawnSeedsInCircle()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < numberOfSeeds; i++)
            {
                Vector3 spawnPosition = GetRandomPositionInCircle(spawnAreaCenter, spawnRadius);
                PhotonNetwork.Instantiate("seed", spawnPosition, seedPrefab.transform.rotation); // Resources 폴더의 "seed" 프리팹을 사용
            }
        }
    }

    // 주어진 중심과 반경 내에서 랜덤 위치를 반환하는 메서드
    Vector3 GetRandomPositionInCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(0, radius);
        return center + new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);
    }
}
