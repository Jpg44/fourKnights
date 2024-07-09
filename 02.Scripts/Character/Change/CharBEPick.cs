using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBEPick : MonoBehaviour
{
    public BackEndDataReceiver backEndDataReceiver;
    void Update()
    {
        if (backEndDataReceiver == null)
        {
            backEndDataReceiver = FindObjectOfType<BackEndDataReceiver>();
        }
    }

    public void characterPick(int num)
    {
        backEndDataReceiver.characterPick(num);
    }
}
