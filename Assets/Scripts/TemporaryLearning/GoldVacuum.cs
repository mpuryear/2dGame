using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoldVacuum : MonoBehaviour
{
    private string GOLD_TAG = "Gold";
    public AudioClip coinSound;
    public CurrencyView currencyView;
    private AudioSource audioSouce;

    void Start() 
    {
        currencyView = GameObject.FindGameObjectWithTag("CurrencyView").GetComponent<CurrencyView>();
        audioSouce = GetComponent<AudioSource>();
        audioSouce.clip = coinSound;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag != GOLD_TAG) { return; }

        currencyView.OnPickupGold(1);
        PlayGoldSound();
        Destroy(col.gameObject);

    }

    void PlayGoldSound()
    {
        if (audioSouce.isPlaying) { return; }
        audioSouce.Play();
    }
}
