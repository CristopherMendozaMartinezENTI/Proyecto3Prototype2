using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldingManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> ActiveScaffolding;
    [SerializeField] private List<GameObject> InnactiveScaffolding;

    void Update() //Esto hay q optimizarlo para q no cambie el material a cada frame -> unirx
    {
        Activate(GetComponent<ActiveStateManager>().active);
    }

    public void Activate(bool state)
    {
        foreach (GameObject obj in ActiveScaffolding)
            obj.SetActive(state);
        foreach (GameObject obj in InnactiveScaffolding)
            obj.SetActive(!state);
    }
}
