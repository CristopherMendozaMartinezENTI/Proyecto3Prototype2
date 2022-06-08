using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCheat : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private List<Vector3> teleports;
    private int iterator = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            player.transform.position = teleports[iterator];
            iterator++;
            if (iterator >= teleports.Count)
                iterator = 0;
        }
    }
}
