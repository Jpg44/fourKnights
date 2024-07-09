using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceName : MonoBehaviour
{
    public Image img; // Image 컴포넌트를 할당
    public float fadeDuration; // 페이드 아웃 시간
    public string sceneName;
    public TextMeshProUGUI tmpg;
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        PlaceNameSet(sceneName);
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlaceNameSet(string sceneName)
    {
        if (sceneName == "VillageScene")
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "시작하는 마을";
        }
        else if (sceneName == "Field1-1")
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "버려진 외곽";
        }
        else if (sceneName == "Field2-1")
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "잃어버린 숲";
        }
        else if (sceneName == "Golem")
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "견고한 사원";
        }
        else if (sceneName == "Dryad")
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "춤추는 서곡";
        }
    }
    IEnumerator FadeOut()
    {
        fadeDuration=5.0f;
        img = transform.GetComponent<Image>();
        tmpg = transform.GetComponentInChildren<TextMeshProUGUI>();
        Color imageStartColor = img.color;
        Color tmpgStartColor = tmpg.color;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            Color imgTempColor = imageStartColor;
            Color tmpgTempColor = tmpgStartColor;
            imgTempColor.a = Mathf.Lerp(imageStartColor.a, 0, progress);
            tmpgTempColor.a = Mathf.Lerp(tmpgStartColor.a, 0, progress);
            img.color = imgTempColor;
            tmpg.color=tmpgTempColor;
            progress += rate * Time.deltaTime;
            yield return null;
        }

        Color imgFinalColor = imageStartColor;
        Color tmpgFinalColor = tmpgStartColor;
        imgFinalColor.a = 0;
        tmpgFinalColor.a = 0;
        img.color = imgFinalColor;
        tmpg.color = tmpgFinalColor;
        gameObject.SetActive(false);
    }
}
