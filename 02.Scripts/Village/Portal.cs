using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public static int fieldNum;
    public static int startFieldNum;
    public int call;
    void Start()
    {
        call = 0;
    }
    void Update()
    {
        if (GameObject.Find("Player(Clone)") != null && call == 0)
        {
            call = 1;
            StartTransform();

        }
    }
    public void StartTransform()
    {
        print(fieldNum+"-----"+startFieldNum);
        if (fieldNum == 1 && startFieldNum == 2)
        {
            GameObject.Find("Player(Clone)").GetComponent<CharacterController>().Move(new Vector3(-43.37438f, 4.821928f, 23.59526f));
            
            startFieldNum = 1;
        }
        else if (fieldNum == 0 && startFieldNum == 1)
        {
            GameObject.Find("Player(Clone)").GetComponent<CharacterController>().Move(new Vector3(-38.66685f, 0f, 0.7869617f));
            
            startFieldNum = 0;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // 'Player' 태그를 가진 객체가 포탈에 닿았을 경우
        if (other.CompareTag("Player"))
        {
            NetworkManager.Instance.isPotal = true;
            print(transform.parent.name);
            if (SceneManager.GetActiveScene().name == "Field1-1")
            {
                NetworkManager.Instance.LeaveRoom();
                if (transform.parent.name == "PortalF")
                {
                    startFieldNum = 1;
                    fieldNum = 2;
                }
                else if (transform.parent.name == "Portal")
                {
                    startFieldNum = 1;
                    fieldNum = 0;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Field2-1")
            {
                NetworkManager.Instance.LeaveRoom();
                if (transform.parent.name == "PortalF")
                {
                    startFieldNum = 2;
                    fieldNum = 3;
                }
                else if (transform.parent.name == "Portal")
                {
                    startFieldNum = 2;
                    fieldNum = 1;
                }
            }
            else if (SceneManager.GetActiveScene().name == "VillageScene")
            {
                startFieldNum = 0;
                fieldNum = 1;
                NetworkManager.Instance.LeaveRoom();
            }
        }
    }
}
