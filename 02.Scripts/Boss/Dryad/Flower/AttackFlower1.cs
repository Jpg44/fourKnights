using UnityEngine;

public class AttackFlower1 : MonoBehaviour
{
    public float launchInterval = 5.0f;  // 발사 간격

    void Start()
    {
        InvokeRepeating("LaunchMissile", launchInterval, launchInterval);
    }

    void LaunchMissile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            GameObject missile = ObjectPoolManager.Instance.GetMissile();
            missile.transform.position = transform.position;  // 발사 위치 설정
            Vector3 direction = (player.transform.position - transform.position).normalized;
            missile.GetComponent<Rigidbody>().velocity = direction * 20;
        }
    }
}
