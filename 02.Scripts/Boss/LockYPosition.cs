using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockYPosition : MonoBehaviour
{
    public Transform target; // 부모 오브젝트 또는 따라갈 대상
    public float fixedY = 1.0f; // 고정될 Y 좌표값

    void Update()
    {
        Vector3 position = transform.position;
        position.x = target.position.x; // 부모의 X 좌표를 따라갑니다.
        position.z = target.position.z; // 부모의 Z 좌표를 따라갑니다.
        position.y = fixedY; // Y 좌표는 고정된 값으로 유지
        transform.position = position;
    }
}

