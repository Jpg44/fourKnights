using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayerMovement : MonoBehaviourPunCallbacks
{
    // 해당 스크립트에서는 실질적인 함수 관련된 코드를 다룸
    // 구동되는 함수를 위해서 조작과 관련된 변수도 이 스크립트에서 선언

    [SerializeField]
    private float moveSpeed = 7; // 이동 속도를 저장할 private 필드 추가
    private float jumpForce = 3.0f;  // 뛰는 힘
    private float gravity = -9.81f;  // 중력 계수
    private Vector3 moveDirection;  // 이동 방향
    private CharacterController controller;
    private PhotonPlayerAnimator playerAnimator;
    private PhotonPlayerController playerController;
    GameObject nearObject;


    // 확인 bool
    public bool isJump { get; private set; }
    public bool isDash { get; private set; }

    public GameObject skill2Prefab; // Skill2 프리팹을 참조할 변수
    public Transform skillSpawnPos; // 스킬이 발사될 위치

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<PhotonPlayerAnimator>();
        playerController = GetComponent<PhotonPlayerController>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // Debug.Log("PhotonPlayerMovement : 내 캐릭터 아님");
            return;
        }

        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
        // 바닥에 안닿아 있으면 중력 physics 적용
        if (controller.isGrounded == false)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }
        // Debug.Log(moveDirection);
        if(!playerController.isInteract){
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
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
        Debug.Log(nearObject);
        if (nearObject != null)
        {
            if (nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(controller);
                playerController.isInteract = true;
                //playerController.isAttack = true;
            }
        }
    }
    public void Skill2()
        {
            Quaternion rotation = skillSpawnPos.rotation * Quaternion.Euler(0, 90, 0);

            // 스킬 프리팹을 생성하고 발사
            GameObject skill2 = Instantiate(skill2Prefab, skillSpawnPos.position, rotation);
            Rigidbody rigid = skill2.GetComponent<Rigidbody>();

            rigid.velocity = skillSpawnPos.forward * 15f; // 원하는 속도로 스킬 발사
        }
    public void Die()
    {
        playerAnimator.Die();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(DisableAfterSeconds(4f));
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Transform playerUITransform = GameObject.Find("Canvas").transform.Find("PlayerUI");
        GameObject dieGameObject = playerUITransform.Find("Die").gameObject;
        dieGameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
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
