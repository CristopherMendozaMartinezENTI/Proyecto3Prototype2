#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ToolsSwitchMenu
{
    [MenuItem("GOTL_Tools/Switch/Link")]
    private static void LinkSwitch()
    {
        List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);

        if (selectedObjects.Count < 2)
        {
            Debug.LogWarning("NO HI HA SUFICIENTS OBJECTES SELECCIONATS");
        }
        else
        {
            List<Switch> Switches = new List<Switch>();

            foreach (GameObject obj in selectedObjects)
            {
                if (obj.GetComponent<Switch>())
                {
                    Switches.Add(obj.GetComponent<Switch>());

                }
            }
            foreach(Switch obj in Switches)
            {
                if (selectedObjects.Contains(obj.gameObject))
                    selectedObjects.Remove(obj.gameObject);
            }


            while(selectedObjects.Count > 0)
            {
                if (!selectedObjects[0].GetComponent<DynamicPlatforms>())
                {
                    Debug.Log("L'OBJECTE " + selectedObjects[0].name + "NO CONTE SCRIPTS COMPATIBLES AMB EL SWITCH");
                }
                else
                {
                    foreach(Switch mySwitch in Switches)
                    {
                        mySwitch.links.Add(selectedObjects[0]);
                        Debug.Log("S'HA REALITZAT EL LINK ENTRE" + mySwitch + " I " + selectedObjects[0]);
                    }
                }

                selectedObjects.RemoveAt(0);
            }
        }
    }

    [MenuItem("GOTL_Tools/Switch/Delink")]
    private static void DelinkSwitch()
    {
        List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);

        if (selectedObjects.Count < 2)
        {
            Debug.Log("NO HI HA SUFICIENTS OBJECTES SELECCIONATS");
        }
        else
        {
            List<Switch> Switches = new List<Switch>();

            foreach (GameObject obj in selectedObjects)
            {
                if (obj.GetComponent<Switch>())
                {
                    Switches.Add(obj.GetComponent<Switch>());

                }
            }
            foreach (Switch obj in Switches)
            {
                if (selectedObjects.Contains(obj.gameObject))
                    selectedObjects.Remove(obj.gameObject);
            }

            while (selectedObjects.Count > 0)
            {
                foreach (Switch mySwitch in Switches)
                {
                    if (mySwitch.links.Contains(selectedObjects[0]))
                    {
                        mySwitch.links.Remove(selectedObjects[0]);
                    }
                    else
                    {
                        Debug.LogWarning(mySwitch.name + "NO ESTABA LINKAT AMB : " + selectedObjects[0]);
                    }
                }

                selectedObjects.RemoveAt(0);
            }
        }
    }

    [MenuItem("GOTL_Tools/Switch/Clear")]
    private static void ClearSwitch()
    {
        List<GameObject> selectedObjects = new List<GameObject>(Selection.gameObjects);

        if (selectedObjects.Count == 0)
        {
            Debug.Log("NO HI HA OBJECTES SELECCIONATS");
        }
        else if (selectedObjects.Count == 1)
        {
            if (selectedObjects[0].GetComponent<Switch>())
            {
                selectedObjects[0].GetComponent<Switch>().links.Clear();
                Debug.Log("S'HA NETEJAT CORRECTAMENT EL SWITCH : " + selectedObjects[0].name);
            }
            else
            {
                Debug.LogWarning("L'OBJECTE SELECCIONAT NO CONTE L'SCRIPT SWITCH");
            }
        }
        else
        {
            foreach(GameObject obj in selectedObjects)
            {
                if (obj.GetComponent<Switch>())
                {
                    obj.GetComponent<Switch>().links.Clear();
                    Debug.Log("S'HA NETEJAT CORRECTAMENT EL SWITCH : " + obj.name);
                }
                else
                {
                    Debug.LogWarning("L'OBJECTE " + obj.name +" NO CONTE L'SCRIPT SWITCH");
                }
            }
        }
    }

    
}
#endif