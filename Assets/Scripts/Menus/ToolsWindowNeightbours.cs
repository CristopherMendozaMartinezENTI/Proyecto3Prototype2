using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class ToolsWindowNeightbours : EditorWindow
{
    [MenuItem("GOTL_Tools/NeightBours/LinkerNeightBours")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ToolsWindowNeightbours));
    }

    [MenuItem("GOTL_Tools/NeightBours/DelinkNeightBours")]
    public static void Delinker()
    {
        GameObject[] selection = Selection.gameObjects;

        if (selection.Length < 2) Debug.LogWarning("No has seleccionat suficients Objectes: min 2");
        else if (selection.Length > 2) Debug.LogWarning("Has seleccionat Masses Objectes: max 2");
        else
        {
            if (!selection[0].GetComponent<Platforms>() || !selection[1].GetComponent<Platforms>())
            {
                Debug.Log("Un dels objectes seleccionats no es una plataforma");
            }
            else
            {
                int id = -1;
                foreach (FloatingPlatforms obj in selection[0].GetComponent<Platforms>().otherNeighbours)
                {
                    if (obj.platformNeighbour == selection[1])
                    {
                        id = selection[0].GetComponent<Platforms>().otherNeighbours.IndexOf(obj);
                    }
                }

                if (id == -1)
                {
                    Debug.LogWarning("No s'ha trobat el link a la plataforma : " + selection[0].name);
                }
                else
                {
                    selection[0].GetComponent<Platforms>().otherNeighbours.RemoveAt(id);
                }

                id = -1;

                foreach (FloatingPlatforms obj in selection[1].GetComponent<Platforms>().otherNeighbours)
                {
                    if (obj.platformNeighbour == selection[0])
                    {
                        id = selection[1].GetComponent<Platforms>().otherNeighbours.IndexOf(obj);
                    }
                }

                if (id == -1)
                {
                    Debug.LogWarning("No s'ha trobat el link a la plataforma : " + selection[1].name);
                }
                else
                {
                    selection[1].GetComponent<Platforms>().otherNeighbours.RemoveAt(id);
                }
            }
        }
    }
    [MenuItem("GOTL_Tools/NeightBours/CleanUpNeightbours")]
    public static void CleanUpNeightbours()
    {
        GameObject toClean = null;
        if (Selection.gameObjects.Length > 0) toClean = Selection.gameObjects[0];
        List<FloatingPlatforms> cleanedObjects = new List<FloatingPlatforms>();

        if (toClean && !toClean.GetComponent<Platforms>())
        {
            Debug.LogWarning("L'objecte seleccionat no conte l'script platfomrs");
        }
        else if (toClean)
        {
            foreach (FloatingPlatforms obj in toClean.GetComponent<Platforms>().otherNeighbours)
            {
                if (!obj.platformNeighbour || obj.platformNeighbour.GetComponent<Platforms>().otherNeighbours.Count == 0)
                    cleanedObjects.Add(obj);
                else
                {
                    cleanedObjects.Add(obj);
                    foreach (FloatingPlatforms inObj in obj.platformNeighbour.GetComponent<Platforms>().otherNeighbours)
                    {
                        if (inObj.platformNeighbour == toClean)
                        {
                            cleanedObjects.Remove(obj);
                            break;
                        }
                    }
                }
            }
        }

        foreach(FloatingPlatforms del in cleanedObjects)
        {
            if (toClean.GetComponent<Platforms>().otherNeighbours.Contains(del))
                toClean.GetComponent<Platforms>().otherNeighbours.Remove(del);
        }
    }

    [MenuItem("GOTL_Tools/NeightBours/ClearNeightbours")]
    public static void ClearNeightbours()
    {
        GameObject toClear = Selection.gameObjects[0];

        if (!toClear.GetComponent<Platforms>())
        {
            Debug.LogWarning("L'objecte seleccionat no conte l'script platfomrs");
        }
        else
        {
            toClear.GetComponent<Platforms>().otherNeighbours.Clear();
        }
    }

    [MenuItem("GOTL_Tools/NeightBours/FixNeightbours")]
    public static void FixNeightbours()
    {
        Platforms[] platforms = GameObject.FindObjectsOfType<Platforms>();
        List<FloatingPlatforms> toDelete = new List<FloatingPlatforms>();
        foreach(Platforms platform in platforms)
        {
            if (platform.otherNeighbours.Count > 0)
            {
                foreach(FloatingPlatforms obj in platform.otherNeighbours)
                {
                    if (!obj.platformNeighbour)
                    {
                        toDelete.Add(obj);
                    }
                }
            }

            foreach(FloatingPlatforms obj in toDelete)
            {
                if (platform.otherNeighbours.Contains(obj))
                {
                    platform.otherNeighbours.Remove(obj);
                }
            }
        }
    }


    

    public Object platform1;
    public Object platform2;
    public int camera;
    
    void OnGUI()
    {
        EditorGUILayout.LabelField("Select the two platforms");

        EditorGUILayout.BeginHorizontal();
        platform1 = EditorGUILayout.ObjectField(platform1, typeof(GameObject), true);
        
        if (GUILayout.Button("Apply Selected"))
        {
            if (Selection.gameObjects.Length != 0 && Selection.gameObjects[0].GetComponent<Platforms>()) 
                 platform1 = Selection.gameObjects[0];
            if (Selection.gameObjects.Length > 1 && Selection.gameObjects[1].GetComponent<Platforms>())
                platform2 = Selection.gameObjects[1];
        }
        if (GUILayout.Button("Clear"))
        {
            platform1 = null;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        platform2 = EditorGUILayout.ObjectField(platform2, typeof(GameObject), true);
        if (GUILayout.Button("Apply Selected"))
        {
            if (Selection.gameObjects.Length != 0 && Selection.gameObjects[0].GetComponent<Platforms>())
                platform2 = Selection.gameObjects[0];
            if (Selection.gameObjects.Length > 1 && Selection.gameObjects[1].GetComponent<Platforms>())
                platform1 = Selection.gameObjects[1];
        }
        if (GUILayout.Button("Clear"))
        {
            platform2 = null;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LinkedCamera");
        camera = EditorGUILayout.IntSlider(camera, 1, 8);
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("FIX LINK"))
        {
            if (!platform1 || !platform2)
            {
                Debug.LogWarning("Omple els espais amb les dues Plataformes que vulguis linkar");
            }
            else
            {
                GameObject first = (GameObject)platform1;
                GameObject second = (GameObject)platform2;

                if (!first.GetComponent<Platforms>() || !second.GetComponent<Platforms>())
                {
                    Debug.LogWarning("Un o els dos objectes seleccionats no es una plataforma");
                }
                else
                {
                    FloatingPlatforms firstNeightbour = new FloatingPlatforms();
                    firstNeightbour.platformNeighbour = second;
                    firstNeightbour.cameraView = camera;

                    if (!first.GetComponent<Platforms>().otherNeighbours.Contains(firstNeightbour))
                    {
                        first.GetComponent<Platforms>().otherNeighbours.Add(firstNeightbour);
                    }

                    FloatingPlatforms secondPlatform = new FloatingPlatforms();
                    secondPlatform.platformNeighbour = first;
                    secondPlatform.cameraView = camera;

                    if (!second.GetComponent<Platforms>().otherNeighbours.Contains(secondPlatform))
                    {
                        second.GetComponent<Platforms>().otherNeighbours.Add(secondPlatform);
                    }
                }
            }
        }
    }
}

