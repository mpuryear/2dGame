using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Slider rewindProgress;

    RewindableCommandManager rewinder;

    void Start() 
    {
        rewinder = GameObject.FindGameObjectWithTag("Player").GetComponent<KeyboardController>().rewinder;
        rewinder.OnRewindProgressUpdate += OnCommandProgressUpdate;
        rewindProgress.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        rewinder.OnRewindProgressUpdate -= OnCommandProgressUpdate;
    }

    public void OnValueChanged(float value)
    {
        textMesh.text = value.ToString() + "x";
        rewinder.SetPlaybackSpeed(value);
    }

    public void OnCommandProgressUpdate(float completionPercent)
    {
        rewindProgress.gameObject.SetActive(completionPercent < 1);
        rewindProgress.value = completionPercent;
    }
}
