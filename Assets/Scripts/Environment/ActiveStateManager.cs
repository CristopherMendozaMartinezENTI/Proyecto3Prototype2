using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStateManager : MonoBehaviour
{
    [Tooltip("If any of the Switches is active, object will active")]
    [SerializeField] private List<Switch> inputSwitches;
    [Tooltip("If any of the Pads is active, object will active")]
    [SerializeField] private List<PressurePad> inputPreassurePads;

    [SerializeField] private bool startActive;
    public bool active { get; private set; }

    private void Update()
    {
        //Check for active input, break immediately when true

        bool any_active = false;

        foreach (Switch _switch in inputSwitches)
            if (!_switch.active) //la logica del switch va al reves ¯\_(ツ)_/¯
            {
                any_active = true;
                break;
            }

        if (!any_active)
            foreach (PressurePad _pad in inputPreassurePads)
                if (_pad.active)
                {
                    any_active = true;
                    break;
                }

        if (any_active)
            active = !startActive;
        else
            active = startActive;
    }
}
