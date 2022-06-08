using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHudMechanics : MonoBehaviour
{
    [SerializeField] private GameObject mechanicsPanel;
    [SerializeField] private string roomName;
    [SerializeField] private bool stairs;
    [SerializeField] private bool portals;
    [SerializeField] private bool cube;
    [SerializeField] private bool pad;
    [SerializeField] private bool _switch;
    [SerializeField] private bool amff;
    [SerializeField] private bool moving;
    [SerializeField] private bool chess;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            mechanicsPanel.GetComponent<HudMechanicsManager>().SetActives(roomName, stairs, portals, cube, pad, _switch, amff, moving, chess);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
