using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { Normal, Gravity }

public class ObjectManipulation : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float maxGrabDistance = 3.0f;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GunType _gunType;
    [Header("UI Elements")]
    private float currentGrabDistance;
    private Vector2 rotationInput;
    private Vector3 hitOffsetLocal;
    private Vector3 rotationDifferenceEuler;
    private RigidbodyInterpolation initialInterpolationSetting;
    private GameObject player;
    private Rigidbody grabbedRigidbody;

    private void Start()
    {
        player = GameObject.Find("Player");
        _gunType = GunType.Gravity;
    }

    private void FixedUpdate()
    {
        // We are holding an object
        if (grabbedRigidbody)
        {
            switch (_gunType)
            {
                case GunType.Normal:
                    //We Move the object towards the player
                    player.GetComponent<PlayerKeyboardInput>().enabled = false;
                    grabbedRigidbody.transform.position = Vector3.MoveTowards(grabbedRigidbody.transform.position, player.transform.position, 0.5f * Time.fixedDeltaTime);
                    if (Vector3.Distance(grabbedRigidbody.transform.position, player.transform.position) < 0.1f)
                    {
                        grabbedRigidbody.transform.position *= -1.0f;
                    }
                    return;
                case GunType.Gravity:
                    //We place the object in the space in relation with the camera position 
                    Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
                    grabbedRigidbody.MoveRotation(Quaternion.Euler(rotationDifferenceEuler + transform.rotation.eulerAngles));
                    Vector3 holdPoint = ray.GetPoint(currentGrabDistance);
                    Vector3 currentEuler = grabbedRigidbody.rotation.eulerAngles;
                    grabbedRigidbody.transform.RotateAround(holdPoint, transform.right, rotationInput.y);
                    grabbedRigidbody.transform.RotateAround(holdPoint, transform.up, -rotationInput.x);
                    grabbedRigidbody.angularVelocity = Vector3.zero;
                    rotationInput = Vector2.zero;
                    rotationDifferenceEuler = grabbedRigidbody.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                    Vector3 centerDestination = holdPoint - grabbedRigidbody.transform.TransformVector(hitOffsetLocal);
                    Vector3 toDestination = centerDestination - grabbedRigidbody.transform.position;
                    Vector3 force = toDestination / Time.fixedDeltaTime;
                    grabbedRigidbody.velocity = Vector3.zero;
                    grabbedRigidbody.AddForce(force, ForceMode.VelocityChange);
                    if (Input.GetKeyDown(KeyCode.LeftControl))
                    {
                        grabbedRigidbody.isKinematic = true;
                    }
                    return;
            }
        } 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //_gunType = GunType.Normal;
            //normalWebText.SetActive(true);
            //gravityWebText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //_gunType = GunType.Gravity;
            //normalWebText.SetActive(false);
            //gravityWebText.SetActive(true);
        }

        if (!Input.GetMouseButton(0))
        {
            // Reset the rigidbody to how it was before we grabbed it
            if (grabbedRigidbody != null)
            {
                playerCamera.GetComponent<CameraController>().enabled = true;
                player.GetComponent<PlayerKeyboardInput>().enabled = true;
                grabbedRigidbody.interpolation = initialInterpolationSetting;
                grabbedRigidbody.transform.parent = null;
                grabbedRigidbody = null;
            }
            return;
        }

        // We are not holding an object
        if (grabbedRigidbody == null)
        {
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hit;

            if (Physics.SphereCast(ray, maxGrabDistance))
            {
                if (Physics.Raycast(ray, out hit, maxGrabDistance))
                {
                    if (hit.rigidbody != null)
                    {
                        playerCamera.GetComponent<CameraController>().enabled = false;
                        //hit.rigidbody.isKinematic = false;
                        grabbedRigidbody = hit.rigidbody;
                        initialInterpolationSetting = grabbedRigidbody.interpolation;
                        rotationDifferenceEuler = hit.transform.rotation.eulerAngles - transform.rotation.eulerAngles;
                        hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
                        currentGrabDistance = Vector3.Distance(ray.origin, hit.point);
                        grabbedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                        grabbedRigidbody.transform.parent = player.transform;
                    }
                }

            }
        }
        else
        {
            //Rotate object
            if (Input.GetKey(KeyCode.E))
            {
                rotationInput += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(ray.origin, maxGrabDistance);
    }
}
