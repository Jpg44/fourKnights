using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossNameSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Bossname();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Bossname()
    {
        if (SceneManager.GetActiveScene().name == "Golem")
        {
            transform.GetComponent<Text>().text = "골 렘";
        }
        else if (SceneManager.GetActiveScene().name == "Dryad")
        {
            transform.GetComponent<Text>().text = "드라이어드";
        }
    }
}
