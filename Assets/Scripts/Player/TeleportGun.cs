using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportGun : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hologram;
    [SerializeField] private Transform objectToTeleport, cam;
    [SerializeField] private float range, cooldown;
    private bool readyToTeleport;
    private RaycastHit rayHit;

    private void Awake()
    {
        readyToTeleport = true;
    }
    void Update()
    {
        if (Physics.Raycast(cam.position,cam.forward,out rayHit, range) && readyToTeleport)
        {
            hologram.SetActive(true);
            hologram.transform.position = rayHit.point;

            if (Input.GetKeyDown(KeyCode.Mouse0)){
                objectToTeleport.transform.position = rayHit.point;
                readyToTeleport = false;
                Invoke("ResetTeleport", cooldown);
            }

            Debug.DrawRay(cam.position, cam.forward, Color.red);
        }
        else
            hologram.SetActive(false);
    }
    private void ResetTeleport()
    {
        readyToTeleport = true;
    }
}
