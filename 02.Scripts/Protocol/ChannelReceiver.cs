using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using TMPro;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun;
using System.Threading.Tasks;

public class ChannelReceiver : MonoBehaviour
{
    public string myId;
    public int channelNum;

    public TextMeshProUGUI channel1;
    public TextMeshProUGUI channel2;
    public TextMeshProUGUI channel3;
    public TextMeshProUGUI channel4;
    public TextMeshProUGUI channel5;
    public TextMeshProUGUI channel6;
    public class DataToSend
    {
        public string userId;
        public int channel;
    }

    // void Awake()
    // {
    //     channel1.text = $"channel 1 ( 0 / 20 )";
    //     channel2.text = $"channel 2 ( 0 / 20 )";
    //     channel3.text = $"channel 3 ( 0 / 20 )";
    //     channel4.text = $"channel 4 ( 0 / 20 )";
    //     channel5.text = $"channel 5 ( 0 / 20 )";
    //     channel6.text = $"channel 6 ( 0 / 20 )";
    // }

    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            myId = args[1]; // 첫 번째 인수 (인덱스 1)를 가져옵니다.
            Debug.Log("Received userId: " + myId);
        }

        // myId = "jhtest1";
        myId = "testcase1";

        //InvokeRepeating("CheckChannelPeople", 0f, 5f);
    }

    public void PickChannel(int num)
    {
        channelNum = num;
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


    // 채널 연결 시 인원을 + 해줄 post 요청
    public void ChannelConnect()
    {
        // 데이터 객체 생성
        DataToSend data = new DataToSend
        {
            userId = myId,
            channel = channelNum
        };

        Debug.Log("커넥트시 userId" + myId + ":" + data.userId);
        Debug.Log("커넥트시 채널" + channelNum + ":" + data.channel);

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // abc 주소로 POST 요청 생성
        string url = "http://k10b208.p.ssafy.io:8081/channel/entrance";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // HTTP 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        request.SendWebRequest().completed += (asyncOperation) =>
        {
            // 요청 완료 후 처리
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text); ;
            }
        };
    }

    // 채널 연결 끊길 시 - 해줄 post 요청
    public void ChannelDisconnect()
    {

        // 데이터 객체 생성
        DataToSend data = new DataToSend
        {
            userId = myId,
            channel = channelNum
        };

        Debug.Log("디스커넥트시 userId" + myId + ":" + data.userId);
        Debug.Log("디스커넥트시 채널" + channelNum + ":" + data.channel);

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // abc 주소로 POST 요청 생성
        string url = "http://k10b208.p.ssafy.io:8081/channel/exit";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // HTTP 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        request.SendWebRequest().completed += (asyncOperation) =>
        {
            // 요청 완료 후 처리
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text); ;
            }
        };
    }
}
