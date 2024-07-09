using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClickColor : MonoBehaviour
{
    public Button WarriorBtn;
    public Button StellaBtn;
    public Color originalColor;
    public Color newColor; // 버튼이 눌렸을 때 변경할 색상

    void Start()
    {
        originalColor = WarriorBtn.GetComponent<Image>().color;
    }

    public void ChangeWarrior()
    {
        StellaBtn.GetComponent<Image>().color = originalColor;

        WarriorBtn.GetComponent<Image>().color = newColor;
    }

    public void ChangeStella()
    {
        WarriorBtn.GetComponent<Image>().color = originalColor;

        StellaBtn.GetComponent<Image>().color = newColor;
    }
}
