using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    Image progressBar;
    static string nextScene;
    string[] addressableAssets = new string[]
    {
        "CropField_01",
        "Flowers_Field_Mat_01",
        "Flowers_Field_Variation",
        "Branches_02",
        "Clover_Flat_Mat_01",
        "Flowers_Flat_Mat_01",
        "Flowers_Flat_Mat_02",
        "Flowers_Flat_Mat_03",
        "Flowers_Flat_Mat_04",
        "Flowers_Plant_Mat_01",
        "Grass_Med_Mat_01",
        "Grass_Short_Mat_01",
        "Grass_Tall_Mat_01",
        "Grass_Tall_Mat_02",
        "Ground_Cover_Mat_01",
        "Mat_Lillies_01",
        "Reeds_Mat_01",
        "Tree_Birch_Mat_01",
        "Tree_Mat_01",
        "Tree_Mat_01_Small",
        "Tree_Mat_02",
        "Tree_Mat_03",
        "Tree_Mat_04",
        "Trunk_Birch_01",
        "Wheat_Mat_01",
        "WildFlowers_01",
        "WildFlowers_02",
        "WildFlowers_03"
    };

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        if (progressBar == null)
        {
            Debug.LogError("Progress bar is not assigned.");
            return;
        }

        if (addressableAssets == null || addressableAssets.Length == 0)
        {
            Debug.LogError("Addressable assets are not assigned.");
            return;
        }

        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        // 어드레서블 리소스 로딩
        foreach (var address in addressableAssets)
        {
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogError("Addressable asset address is null or empty.");
                continue;
            }

            // 리소스 타입에 맞게 LoadAssetAsync 호출
            AsyncOperationHandle<Material> handle = Addressables.LoadAssetAsync<Material>(address);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to load addressable asset: {address}");
                yield break;
            }
        }

        // 씬 비동기 로딩
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
