using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class channelTracker : MonoBehaviour
{
    public TextMeshProUGUI channel1;
    public TextMeshProUGUI channel2;
    public TextMeshProUGUI channel3;
    public TextMeshProUGUI channel4;
    public TextMeshProUGUI channel5;
    public TextMeshProUGUI channel6;
    void Awake()
    {
        channel1.text = $"Channel 1 ( 0 / 20 )";
        channel2.text = $"Channel 2 ( 0 / 20 )";
        channel3.text = $"Channel 3 ( 0 / 20 )";
        channel4.text = $"Channel 4 ( 0 / 20 )";
        channel5.text = $"Channel 5 ( 0 / 20 )";
        channel6.text = $"Channel 6 ( 0 / 20 )";
    }

    void Start()
    {
        InvokeRepeating("CheckChannelPeople", 0f, 5f);
    }

    void Update()
    {

    }

    public void CheckChannelPeople()
    {
        StartCoroutine(Check());
    }

    public IEnumerator Check()
    {
        string url = "http://k10b208.p.ssafy.io:8081/channel/count";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // 성공적으로 데이터를 받았을 때 처리
                Debug.Log(request.downloadHandler.text);

                CheckChannelData channelData = JsonUtility.FromJson<CheckChannelData>(request.downloadHandler.text);


                for (int i = 0; i < channelData.userCounts.Length; i++)
                {
                    switch (channelData.userCounts[i].channel)
                    {
                        case 1: channel1.text = $"Channel 1 ( {channelData.userCounts[i].userCount} / 20 )"; break;
                        case 2: channel2.text = $"Channel 2 ( {channelData.userCounts[i].userCount} / 20 )"; break;
                        case 3: channel3.text = $"Channel 3 ( {channelData.userCounts[i].userCount} / 20 )"; break;
                        case 4: channel4.text = $"Channel 4 ( {channelData.userCounts[i].userCount} / 20 )"; break;
                        case 5: channel5.text = $"Channel 5 ( {channelData.userCounts[i].userCount} / 20 )"; break;
                        case 6: channel6.text = $"Channel 6 ( {channelData.userCounts[i].userCount} / 20 )"; break;
                        default: break;
                    }
                }

                if (channelData.userCounts.Length == 0)
                {
                    channel1.text = $"Channel 1 ( 0 / 20 )";
                    channel2.text = $"Channel 2 ( 0 / 20 )";
                    channel3.text = $"Channel 3 ( 0 / 20 )";
                    channel4.text = $"Channel 4 ( 0 / 20 )";
                    channel5.text = $"Channel 5 ( 0 / 20 )";
                    channel6.text = $"Channel 6 ( 0 / 20 )";
                }
            }
        }
    }


    [System.Serializable]
    public class CheckChannelData
    {
        public string message;
        public int statusCode;
        public UserCounts[] userCounts;
    }

    [System.Serializable]
    public class UserCounts
    {
        public int userCount;
        public int channel;
    }
}
