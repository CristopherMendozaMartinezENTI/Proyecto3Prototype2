using UnityEngine;

//Script para los sonidos del player
public class AudioControl_mod : MonoBehaviour
{
    private bool a = false;
    private Controller controller;
    private Animator animator;
    private Controller mover;
    private Transform tr;
    private string floorTag;
    private Vector3 littleForward = new Vector3(0.2f, 0.2f, 0);

    [SerializeField] private float landVelocityThreshold = 5f;
    [SerializeField] private float footstepDistance = 0.5f;
    private float currentFootstepDistance = 0f;

    private float currentFootStepValue = 0f;

    private void Start()
    {
        controller = GetComponent<Controller>();
        animator = GetComponentInChildren<Animator>();
        mover = GetComponent<Controller>();
        tr = transform;
        controller.OnLand += OnLand;
        controller.OnJump += OnJump;
    }

    private void Update()
    {

        Vector3 _velocity = controller.GetVelocity();
        Vector3 _horizontalVelocity = MathVector.RemoveDotVector(_velocity, tr.up);
        FootStepUpdate(_horizontalVelocity.magnitude);

        RaycastHit hit;
        if (Physics.Raycast((transform.position + littleForward), Vector3.down, out hit))
        {
            //This is the colliders tag
            floorTag = hit.collider.tag;
        }

        if (floorTag == "Wooden")
        {
            AkSoundEngine.SetSwitch("Footsteps", "Wood", gameObject);
        }
        else if (floorTag == "Grass")
        {
            AkSoundEngine.SetSwitch("Footsteps", "Grass", gameObject);
        }
        else if (floorTag == "Rocks" || floorTag == "Untagged")
        {
            AkSoundEngine.SetSwitch("Footsteps", "Rocks", gameObject);
        }
    }

    private void FootStepUpdate(float _movementSpeed)
    {
        float _speedThreshold = 0.05f;
        
        currentFootstepDistance += Time.deltaTime * _movementSpeed;
        if (currentFootstepDistance > footstepDistance)
        {
            if (controller.IsGrounded() && _movementSpeed > _speedThreshold)
            {
                AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
                currentFootstepDistance = 0f;
            }
        }
    }

    private void OnLand(Vector3 _v)
    {
        if (MathVector.GetDotProduct(_v, tr.up) > -landVelocityThreshold)
            return;
        AkSoundEngine.PostEvent("Play_Land", gameObject);
    }

    private void OnJump(Vector3 _v)
    {
        AkSoundEngine.PostEvent("Play_Jump", gameObject);
    }
}


