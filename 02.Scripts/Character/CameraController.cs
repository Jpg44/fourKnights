using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    [SerializeField]
    private Transform target;               // 카메라가 추적하는 대상
    [SerializeField]
    private float minDistance = 3;          // 카메라와 target의 최소 거리
    [SerializeField]
    private float maxDistance = 10;         // 카메라와 target의 최대 거리
    [SerializeField]
    private float wheelSpeed = 300;         // 마우스 휠 스크롤 속도
    [SerializeField]
    private float xMoveSpeed = 500;         // 카메라의 y축 회전 속도
    [SerializeField]
    private float yMoveSpeed = 250;         // 카메라의 x축 회전 속도
    private float yMinLimit = 0;            // 카메라의 x축 회전 제한 속도 최소 값
    private float yMaxLimit = 50;           // 카메라의 x축 회전 제한 속도 최대 값
    private float x, y;                     // 마우스 이동 방향 값
    private float distance;                 // 카메라와 target의 거리

    private void Awake() 
    {
        // 최초 설정된 target과 카메라의 위치 기준으로 distance 값 초기화
        distance = Vector3.Distance(transform.position, target.position);

        // 최초 카메라의 회전 값을 x, y 변수에 저장
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }
    
    private void Update() 
    {
        // target이 존재해야 실행
        if (target == null ) return;

        // 마우스 x, y 움직임
        x += Input.GetAxis("Mouse X") * xMoveSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * yMoveSpeed * Time.deltaTime;

        // 오브젝트 위, 아래(x축) 한계 범위 설정
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // 카메라의 회전 정보 갱신
        transform.rotation = Quaternion.Euler(y, x, 0);

        // 마우스 휠 스크롤을 이용해 거리 값 조절
        distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime;
        // 최소, 최대 거리값 설정
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void LateUpdate() 
    {
        // target이 존재해야 실행
        if (target == null ) return;

        // 카메라의 위치 정보 갱신
        // target 위치 기준으로 쫓아가기
        transform.position = transform.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle,min,max);
    }
}
