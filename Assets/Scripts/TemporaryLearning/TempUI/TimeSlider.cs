using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Slider rewindProgress;

    void Start() 
    {
        CommandManager.Instance.OnRewindProgressUpdate += OnCommandProgressUpdate;
        rewindProgress.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        CommandManager.Instance.OnRewindProgressUpdate -= OnCommandProgressUpdate;
    }

    public void OnValueChanged(float value)
    {
        textMesh.text = value.ToString() + "x";
        CommandManager.Instance.SetPlaybackSpeed(value);
    }

    public void OnCommandProgressUpdate(float completionPercent)
    {
        rewindProgress.gameObject.SetActive(completionPercent < 1);
        rewindProgress.value = completionPercent;
    }
}
