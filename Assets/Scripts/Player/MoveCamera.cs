using UnityEngine;

public class MoveCamera : MonoBehaviour 
{
    [SerializeField] private Transform playerHead;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera Cam;
    void Update() {
        transform.position = playerHead.transform.position;
    }
}
