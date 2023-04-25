using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FUTURE:- Move this to something better, probably a ScriptableObject instead
// of a MonoBehaviour
public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    public Sprite abilityBarIcon;
    [SerializeField]
    protected bool isReady => cooldownProgress <= 0;
    [SerializeField]
    protected float cooldownDuration;
    [SerializeField]
    protected float cooldownProgress = 0f;
    [SerializeField]
    protected bool inEffect => effectProgress > 0;  // Slightly different from isReady as we currently don't have a buff system.
    [SerializeField]
    protected float effectProgress = 0f;
    [SerializeField]
    protected float effectDuration;
    [SerializeField]
    protected float effectStartTime = 0f;
    [SerializeField]
    protected float castStartTime = 0f;
    [SerializeField]
    protected bool isCasting = false;

    protected void Update()
    {
        // Don't start the CD of 
        if(!isReady && !isCasting) 
        {
            UpdateCooldown();
        }

        if(inEffect)
        {
            UpdateEffectProgress();
        }

        OnUpdate();
    }

    // Called immediately after Update()
    protected abstract void OnUpdate();

    public Action<float, float> OnCooldownProgressUpdate;
    public Action<float, float> OnEffectProgressUpdate;
    public Action OnEffectEnd;

    public abstract void Undo();

    public abstract void Execute();

    private void UpdateCooldown()
    {
        cooldownProgress -= Time.deltaTime;

        var cooldownPercent = cooldownProgress / cooldownDuration;
        OnCooldownProgressUpdate?.Invoke(cooldownPercent, cooldownProgress);
    }

    private void UpdateEffectProgress()
    {
        effectProgress -= Time.deltaTime;
        if(effectProgress <= 0)
        {
            OnEffectEnd?.Invoke();
            return;
        }
        var effectPercent = effectProgress / effectDuration;
        OnEffectProgressUpdate?.Invoke(effectPercent, effectProgress);
    }
}
