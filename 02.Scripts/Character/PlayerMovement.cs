using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 해당 스크립트에서는 실질적인 함수 관련된 코드를 다룸
    // 구동되는 함수를 위해서 조작과 관련된 변수도 이 스크립트에서 선언

    [SerializeField]
    private float moveSpeed = 7; // 이동 속도를 저장할 private 필드 추가
    private float jumpForce = 3.0f;  // 뛰는 힘
    private float gravity = -9.81f;  // 중력 계수
    private Vector3 moveDirection;  // 이동 방향
    private CharacterController controller;
    private PlayerController playerController;

    GameObject nearObject;
    public GameObject settingsPanel;

   
    // 확인 bool
    public bool isJump { get; private set; }
    public bool isDash { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 바닥에 안닿아 있으면 중력 physics 적용
        if (controller.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }
        // Debug.Log(moveDirection);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    // 캐릭터가 움직일 때마다 vector값 저장
    public void MoveTo(Vector3 direction)
    {
        moveDirection = new Vector3(direction.x, moveDirection.y, direction.z);
    }
    public void Jump()
    {
        moveDirection.y = jumpForce;
    }
    public void Dash()
    {
        moveSpeed *= 1.5f;
        isDash = true;

        Invoke("DashOut", 0.5f);
    }

    void DashOut()
    {
        moveSpeed /= 1.5f;
        isDash = false;
    }
    public void Interaction()
    {
        if (nearObject != null)
        {
            if (nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(controller);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
        }
    }
}
