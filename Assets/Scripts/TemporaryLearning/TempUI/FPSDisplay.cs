using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;

    private float pollingTime = 0.5f;
    private float time = 0f;
    private int frameCount = 0;

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        frameCount++;
        if(time <= 0)
        {
            tmp.text = "FPS: " + (frameCount / pollingTime).ToString();
            time = pollingTime;
            frameCount = 0;
        }
    }
}
