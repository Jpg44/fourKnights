using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHooks : MonoBehaviour
{
    [SerializeField] Text healthText = default; 
    [SerializeField] Image healthBar = default;
    [SerializeField] Image healthBarEffect = default;
    // [SerializeField] Image fadeImage;

    public void SetHealth(int current, int total)
    {
        
        Debug.Log("체력바 수정");
        healthText.text = $"{current}/{total}";
        healthBar.fillAmount = current / (float)total;
        // healthBarEffect.fillAmount = current / (float)total;
    }
}
