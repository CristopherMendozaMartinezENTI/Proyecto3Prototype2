using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GravitationalForce { xRedFoward, xRedBackward, yGreenFoward, yGreenBackward, zBlueForward, zBlueBackward, None }
public class PropPhysicsController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GravitationalForce gravityOrientation;
    [SerializeField] private float gravityForce = 9.81f;
    [SerializeField] private GameObject RespawFxPrefab;
    private Vector3 initialPos;
    private GameObject _RespawFxTmp;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void FixedUpdate()
    {
        switch (gravityOrientation)
        {
            case GravitationalForce.yGreenFoward:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(-Physics.gravity, ForceMode.Acceleration);
                return;
            case GravitationalForce.yGreenBackward:
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                return;
            case GravitationalForce.zBlueBackward:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, -gravityForce), ForceMode.Acceleration);
                return;
            case GravitationalForce.zBlueForward:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, gravityForce), ForceMode.Acceleration);
                return;
            case GravitationalForce.xRedBackward:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-gravityForce, 0.0f, 0.0f), ForceMode.Acceleration);
                return;
            case GravitationalForce.xRedFoward:
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(gravityForce, 0.0f, 0.0f), ForceMode.Acceleration);
                return;
            case GravitationalForce.None:
                return;
        }
    }

    private void Update()
    {
        Destroy(_RespawFxTmp, 2.0f);
    }

    public void ResetPos()
    {
        _RespawFxTmp = Instantiate(RespawFxPrefab, initialPos, transform.rotation);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = initialPos;
    }

    public Vector3 GetVelocity()
    {
        return this.gameObject.GetComponent<Rigidbody>().velocity;
    }

    public Vector3 GetMovementVelocity()
    {
        return this.gameObject.GetComponent<Rigidbody>().angularVelocity;
    }

    public bool IsGrounded()
    {
        if (this.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero) return true;
        else return false;
    }
}
