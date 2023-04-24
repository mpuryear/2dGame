using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAbilityButton: MonoBehaviour
{
    [SerializeField]
    private Slider cooldownSlider;
    [SerializeField]
    private Slider effectSlider;
    [SerializeField]
    private TextMeshProUGUI cooldownText;
    [SerializeField]
    private float cooldownProgress;
    [SerializeField]
    private Ability ability;


    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        effectSlider.gameObject.SetActive(false);
        ability.OnCooldownProgressUpdate += UpdateCooldownProgress;
        ability.OnEffectProgressUpdate += OnEffectProgressUpdate;
        ability.OnEffectEnd += OnEffectProgressEnd;
        UpdateCooldownProgress(0, 0);
    }

    void OnDestroy()
    {
        ability.OnCooldownProgressUpdate -= UpdateCooldownProgress;
        ability.OnEffectProgressUpdate -= OnEffectProgressUpdate;
        ability.OnEffectEnd -= OnEffectProgressEnd;
    }

    void UpdateCooldownProgress(float percent, float remainingTime)
    {
        cooldownSlider.value = percent;
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
        if (timeSpan.Seconds >= 1)
        {
            cooldownText.text = string.Format("{0:D1}", timeSpan.Seconds + 1);
        }
        else if (timeSpan.Milliseconds > 0)
        {
            cooldownText.text = string.Format("1");
        }
        else
        {
            cooldownText.text = "";
        }
    }

    void OnEffectProgressUpdate(float percent, float remainingTime)
    {
        if(percent > 0)
        {
            effectSlider.gameObject.SetActive(true);
        }
        effectSlider.value = percent;
    }

    void OnEffectProgressEnd()
    {
        effectSlider.gameObject.SetActive(false);
    }

}
