#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlatformsMenu: EditorWindow
{
    [MenuItem("GOTL_Tools/Platforms/Fix_Diagonal")]
    public static void Fix_Platform()
    {
        EditorWindow.GetWindow(typeof(PlatformsMenu));
    }

    public Object platform1;
    public Object platform2;
    public int camera = 1;
    public bool endDynamicPlatform = false;

    void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Correct Platform");

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
        EditorGUILayout.LabelField("Platform To Fix");

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

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Move Dynamic Platform Destination");
        endDynamicPlatform = EditorGUILayout.Toggle(endDynamicPlatform);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("LinkedCamera");
        camera = EditorGUILayout.IntSlider(camera, 1, 8);
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("FIX LINK"))
        {
            GameObject cameraObj = GameObject.Find(camera.ToString());
            Vector3 dir = cameraObj.transform.TransformDirection(Vector3.forward);
            Vector3 origin;

            Vector3 check = Vector3.one * 10000;

            Vector3 objectPivot = ((GameObject)platform2).transform.position;

            if (endDynamicPlatform)
                objectPivot += ((GameObject)platform2).GetComponent<DynamicPlatforms>().recorrido;

            float inc = (objectPivot.y - ((GameObject)platform1).transform.position.y) / dir.y;

            for (int j = 0; j < 4; j++)
            {
                origin = ((GameObject)platform1).transform.position + (j == 0 ? Vector3.forward : j == 1 ? Vector3.right : j == 2 ? Vector3.back : Vector3.left);
                if (Vector3.Distance(check, objectPivot) > Vector3.Distance(origin + (dir * inc), objectPivot))
                    check = origin + (dir * inc);
            }

            if (endDynamicPlatform)
                ((GameObject)platform2).GetComponent<DynamicPlatforms>().recorrido = check - ((GameObject)platform2).transform.position;
            else
                ((GameObject)platform2).transform.position = check;
        }
    }

}
#endif
