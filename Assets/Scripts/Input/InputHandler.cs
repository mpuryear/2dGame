using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Ability abilityLMB;
    public Ability abilityRMB;
    public Ability abilityShift;
    public Ability ability1;
    public Ability ability2;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            abilityLMB.Execute();
        }

        if(Input.GetMouseButtonDown(1) | Input.GetMouseButton(2))
        {
            abilityRMB.Execute();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            abilityShift.Execute();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha1))
        {
            ability1.Execute();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha2))
        {
            ability2.Execute();
        }
    }
}
