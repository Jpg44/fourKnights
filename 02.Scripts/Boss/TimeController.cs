using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }

    public Transform directionalLightTransform;
    public float lightAngleX = 0f;

    void Awake() {
        if (Instance == null) 
        {
            Instance = this;
        } 
    }

    void Update()
    {
        // 매 초마다 x축에 대해  회전
        lightAngleX += 10f * Time.deltaTime;
        directionalLightTransform.localRotation = Quaternion.Euler(lightAngleX, 0, 0);
    }

    public float GetCurrentLightAngleX()
    {
        return lightAngleX%360;
    }
}
