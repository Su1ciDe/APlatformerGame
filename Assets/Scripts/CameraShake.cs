using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCam;
    float shakeAmount = 0;

    private void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.localPosition;
            //float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            //float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            float offsetY = Random.Range(-shakeAmount, shakeAmount);

            camPos.x = offsetX;
            camPos.y = offsetY;
            camPos.z = 0f;

            mainCam.transform.localPosition = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
