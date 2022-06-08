using UnityEngine;

//Este script va en la Main Camara y permite pinta los portales
public class PortalMainCameraRenderer : MonoBehaviour {

    private Portal[] portals;

    private void Awake () {
        portals = FindObjectsOfType<Portal> ();
    }

    private void LateUpdate()  
    {
        foreach(Portal portal in portals)
        {
            portal.PrePortalRender();
            portal.Render();
            portal.PostPortalRender();
        }
    }
}