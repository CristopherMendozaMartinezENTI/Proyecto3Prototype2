using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScaffolding : MonoBehaviour
{

    private Material[] matArray;
    private Material red; private Material green;
    private LineRenderer line;
    [SerializeField] private List<GameObject> ActiveScaffolding;
    [SerializeField] private List<GameObject> InnactiveScaffolding;

    void Start()
    {
        //line = this.gameObject.GetComponent<LineRenderer>();
        //matArray = line.materials;
        //red = Resources.Load<Material>(@"Materials/" + "HologramRed");
        //green = Resources.Load<Material>(@"Materials/" + "HologramGreen");
    }

    void Update() //Esto hay q optimizarlo para q no cambie el material a cada frame -> unirx
    {
        Activate(GetComponent<ActiveStateManager>().active);

        /*
        if (GetComponent<ActiveStateManager>().active)
        {
            matArray[1] = green;
            line.materials = matArray;
        }
        else
        {
            matArray[1] = red;
            line.materials = matArray;
        }
        */
    }
    public void Activate(bool state)
    {
        foreach (GameObject obj in ActiveScaffolding)
            obj.SetActive(state);
        foreach (GameObject obj in InnactiveScaffolding)
            obj.SetActive(!state);
    }
}