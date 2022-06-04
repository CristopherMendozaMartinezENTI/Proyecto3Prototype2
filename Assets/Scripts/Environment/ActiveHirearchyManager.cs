using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHirearchyManager : MonoBehaviour
{ 
    [Tooltip("If any of the Switches is active, object will active")]
    [SerializeField] private List<Switch> inputSwitches;
    [Tooltip("If any of the Pads is active, object will active")]
    [SerializeField] private List<PressurePad> inputPreassurePads;

    public int currentLevel { get; private set; }

    private void Update()
    {
        //Check for active input, break immediately when true

        int _activeLevel = 0;

        foreach (Switch _switch in inputSwitches)
            if (!_switch.active) //la logica del switch va al reves ¯\_(ツ)_/¯
                _activeLevel++;

        foreach (PressurePad _pad in inputPreassurePads)
            if (_pad.active)
                _activeLevel++;

        currentLevel = _activeLevel;
    }
    
}
