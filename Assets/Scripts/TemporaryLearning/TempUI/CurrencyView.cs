using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyView : MonoBehaviour
{
    public TextMeshProUGUI text;
    int goldValue = 0;

    void Start()
    {
        text.text = goldValue.ToString();
    }

    public void OnPickupGold(int amount)
    {
        goldValue += amount;
        text.text = goldValue.ToString();
    }
}
