using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted, //* 반전 시켜 보기
        CameraForward,
        CameraForwardInverted, //* 반전 시켜 보기
    }

    public Camera cam;

    void Update()
    {
        if (cam == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                cam = player.GetComponentInChildren<Camera>(); // Camera 컴포넌트를 찾아 할당
            }
        }
        if (cam != null)
        {
            transform.rotation = cam.transform.rotation;
        }
    }

    [SerializeField] private Mode mode;
    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(cam.transform);
                transform.Rotate(0, 180, 0);
                break;
            case Mode.LookAtInverted:
                //* 카메라 방향을 알아내서 그 방향 만큼 돌려줘서 반전시키기
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                //* 카메라 방향으로 Z축 (앞뒤)을 바꿔주기
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                //* 카메라 방향으로 Z축 (앞뒤)을 바꿔주고 반전시키기
                transform.forward = -Camera.main.transform.forward;
                break;
            default :
                
                break;
        }
    }
}