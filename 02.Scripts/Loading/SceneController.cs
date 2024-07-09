using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬 전환 시 오브젝트가 삭제되지 않도록 설정
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "StartScene" || scene.name == "CharacterScene")
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.startSceneBGM);
        }
        else if (scene.name == "VillageScene")
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.villageSceneBGM);
        }
        else if (scene.name == "Field1-1")
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.field1SceneBGM1);
        }
        else if (scene.name == "Field2-1")
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.field1SceneBGM2);
        }
        else if (scene.name == "Golem")
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.golemSceneBGM);
        }
        else if (scene.name == "Dryad")
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.dryadSceneBGM);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
