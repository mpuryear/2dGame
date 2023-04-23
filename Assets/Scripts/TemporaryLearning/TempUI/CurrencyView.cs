using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyView : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI timer;

    int goldValue = 0;
    int enemyCounter = 0;

    void Start()
    {
        goldText.text = goldValue.ToString() + "g";
        killText.text = "Kills: " + enemyCounter.ToString();
    }

    void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.time);
        timer.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void OnPickupGold(int amount)
    {
        goldValue += amount;
        goldText.text = goldValue.ToString() + "g";
    }

    public void OnKillEnemy()
    {
        enemyCounter++;
        killText.text = "Kills: " + enemyCounter.ToString();
    }   
}
