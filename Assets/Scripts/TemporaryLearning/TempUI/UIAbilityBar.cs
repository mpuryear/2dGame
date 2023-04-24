using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityBar : MonoBehaviour
{
    [SerializeField]
    private UIAbilityButton abilityButtonShift;
    [SerializeField]
    private UIAbilityButton abilityButton1;
    [SerializeField]
    private UIAbilityButton abilityButton2;
    [SerializeField]
    private UIAbilityButton abilityButtonLMB;

    [SerializeField]
    private UIAbilityButton abilityButtonRMB;

    void Awake()
    {
        
    }

    public void Populate()
    {
        var roll = GameObject.FindGameObjectWithTag("Player").GetComponent<RollAbility>();
        abilityButtonShift.SetAbility(roll);

        var sprint = GameObject.FindGameObjectWithTag("Player").GetComponent<SprintAbility>();
        abilityButton1.SetAbility(sprint);

        var rewind = GameObject.FindGameObjectWithTag("Player").GetComponent<RewindTimeAbility>();
        abilityButton2.SetAbility(rewind);

        var attack = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackAbility>();
        abilityButtonLMB.SetAbility(attack);

        var blink = GameObject.FindGameObjectWithTag("Player").GetComponent<BlinkAbility>();
        abilityButtonRMB.SetAbility(blink);
    }
}