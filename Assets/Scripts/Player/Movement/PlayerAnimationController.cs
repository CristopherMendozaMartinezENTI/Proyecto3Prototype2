using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private GameObject playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Animator>().Play("Run");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.GetComponent<Animator>().Play("RunBackwards");
        }
        else if(Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Animator>().Play("RunRight");
        }
        else if(Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<Animator>().Play("RunLeft");
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<Animator>().Play("Jump");
        }
        else if (playerController.GetComponent<Controller>().GetVelocity() == Vector3.zero)
        {
            gameObject.GetComponent<Animator>().Play("Iddle");
        }
    }
}
