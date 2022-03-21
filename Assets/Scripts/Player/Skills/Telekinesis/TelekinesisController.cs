using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TelekinesisController : MonoBehaviour
{
    [SerializeField] private LayerMask _grabLayer;
    [SerializeField] private Camera  _camera;

    [Header("Input Setting"), Space(10)]
    [SerializeField] private KeyCode Rotate = KeyCode.R;
    [SerializeField] private KeyCode SnapRotation = KeyCode.LeftShift;
    [SerializeField] private KeyCode Freeze = KeyCode.F;
    [SerializeField] private KeyCode SwitchAxis = KeyCode.Tab;
    [SerializeField] private KeyCode RotateZ  = KeyCode.Space;
    [SerializeField] private KeyCode RotationSpeedIncrease = KeyCode.LeftControl;
    [SerializeField] private KeyCode ResetRotation = KeyCode.LeftAlt;
    private Rigidbody _grabbedRigidbody;
    private Transform _grabbedTransform;
    private bool hasFreezedRotation;
    private Vector3 _hitOffsetLocal;
    private float  _currentGrabDistance;
    private RigidbodyInterpolation  _initialInterpolationSetting;
    private Quaternion  _rotationDifference;
    [SerializeField]
    private Transform _laserStartPoint = null;
    private Vector3 _rotationInput = Vector3.zero;

    [Header("Rotation Settings")]
    [Tooltip("Transform of the player, that rotations should be relative to")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float _rotationSenstivity = 1.5f;
    [SerializeField] private float  SnapRotationDegrees = 45f;
    [SerializeField] private float  _snappedRotationSens = 15f;
    [SerializeField] private float _rotationSpeed = 5f;
    private Quaternion _desiredRotation = Quaternion.identity;
    [SerializeField, Tooltip("Input values above this will be considered and intentional change in rotation")]
    private float  _rotationTollerance = 0.8f;

    private Vector3 _lockedRot;
    private Vector3 _forward;
    private Vector3 _up;
    private Vector3 _right;

    [Header("Scroll Wheel Object Movement"), Space(5)]
    private Vector3  _scrollWheelInput = Vector3.zero;
    [SerializeField]
    private float _scrollWheelSensitivity = 5f;
    [SerializeField, Tooltip("The min distance the object can be from the player")]
    private float _minObjectDistance = 2.5f;
    [SerializeField, Tooltip("The maximum distance at which a new object can be picked up")]
    private float _maxGrabDistance = 50f;
    private bool _distanceChanged;
    private Vector3 _zeroVector3 = Vector3.zero;
    private Vector3 _oneVector3 = Vector3.one;
    private Vector3 _zeroVector2 = Vector2.zero;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip onGrabbed;
    [SerializeField] private AudioClip onReleased;


    private bool _justReleased;
    private bool _wasKinematic;
   
    [Serializable] public class BoolEvent : UnityEvent<bool> { };
    [Serializable]public class GrabEvent : UnityEvent<GameObject> { };

    [Header("Events"), Space(10)]
    public BoolEvent OnRotation;
    public BoolEvent OnRotationSnapped;
    public BoolEvent OnAxisChanged;
    public GrabEvent OnObjectGrabbed;

    //Line Renderer variables
    public Vector3 StartPoint { get; private set; }
    public Vector3 MidPoint { get; private set; }
    public Vector3 EndPoint { get; private set; }

    private bool m_UserRotation;
    private bool _userRotation
    {
        get
        {
            return m_UserRotation;
        }
        set
        {
            if (m_UserRotation != value)
            {
                m_UserRotation = value;
                OnRotation.Invoke(value);
            }
        }
    }

    private bool m_SnapRotation;

    private bool _snapRotation
    {
        get
        {
            return m_SnapRotation;
        }
        set
        {
            if (m_SnapRotation != value)
            {
                m_SnapRotation = value;
                OnRotationSnapped.Invoke(value);
            }
        }
    }

    private bool m_RotationAxis;

    private bool _rotationAxis
    {
        get
        {
            return m_RotationAxis;
        }
        set
        {
            if (m_RotationAxis != value)
            {
                m_RotationAxis = value;
                OnAxisChanged.Invoke(value);
            }
        }
    }

    private void Start()
    {
        if(_camera == null)
        {
            Debug.LogError($"{nameof(TelekinesisController)} missing Camera", this);
            return;
        }

        if(playerTransform == null) 
        {
            playerTransform = this.transform;
            Debug.Log($"As {nameof(playerTransform)} is null, it have been set to set to this.transform", this);
        }
    }

    private void FixedUpdate()
    {
        if (_grabbedRigidbody)
        {
            Ray ray = CenterRay();

            UpdateRotationAxis();

            Debug.DrawRay(_grabbedTransform.position, _up * 5f, Color.green);
            Debug.DrawRay(_grabbedTransform.position, _right * 5f, Color.red);
            Debug.DrawRay(_grabbedTransform.position, _forward * 5f, Color.blue);

            var intentionalRotation = Quaternion.AngleAxis(_rotationInput.z, _forward) * Quaternion.AngleAxis(_rotationInput.y, _right) * Quaternion.AngleAxis(-_rotationInput.x, _up) * _desiredRotation;
            var relativeToPlayerRotation = playerTransform.rotation * _rotationDifference;

            if (_userRotation && _snapRotation)
            {
                _lockedRot += _rotationInput;
                if (Mathf.Abs(_lockedRot.x) > _snappedRotationSens || Mathf.Abs(_lockedRot.y) > _snappedRotationSens || Mathf.Abs(_lockedRot.z) > _snappedRotationSens)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        if (_lockedRot[i] > _snappedRotationSens)
                        {
                            _lockedRot[i] += SnapRotationDegrees;
                        }
                        else if (_lockedRot[i] < -_snappedRotationSens)
                        {
                            _lockedRot[i] += -SnapRotationDegrees;
                        }
                        else
                        {
                            _lockedRot[i] = 0;
                        }
                    }

                    var q = Quaternion.AngleAxis(-_lockedRot.x, _up) * Quaternion.AngleAxis(_lockedRot.y, _right) * Quaternion.AngleAxis(_lockedRot.z, _forward) * _desiredRotation;
                    var newRot = q.eulerAngles;
                    newRot.x = Mathf.Round(newRot.x / SnapRotationDegrees) * SnapRotationDegrees;
                    newRot.y = Mathf.Round(newRot.y / SnapRotationDegrees) * SnapRotationDegrees;
                    newRot.z = Mathf.Round(newRot.z / SnapRotationDegrees) * SnapRotationDegrees;
                    _desiredRotation = Quaternion.Euler(newRot);
                    _lockedRot = _zeroVector2;
                }
            }
            else
            {
                _desiredRotation = _userRotation ? intentionalRotation : relativeToPlayerRotation;
            }
            _grabbedRigidbody.angularVelocity = _zeroVector3;
            _rotationInput = _zeroVector2;
            _rotationDifference = Quaternion.Inverse(playerTransform.rotation) * _desiredRotation;
            var holdPoint = ray.GetPoint(_currentGrabDistance) + _scrollWheelInput;
            var centerDestination = holdPoint - _grabbedTransform.TransformVector(_hitOffsetLocal);

            Debug.DrawLine(ray.origin, holdPoint, Color.blue, Time.fixedDeltaTime);

            var toDestination = centerDestination - _grabbedTransform.position;

            var force = toDestination / Time.fixedDeltaTime * 0.3f / _grabbedRigidbody.mass;

            //force += _scrollWheelInput;
            _grabbedRigidbody.velocity = _zeroVector3;
            _grabbedRigidbody.AddForce(force, ForceMode.VelocityChange);

            RotateGrabbedObject();

            if (_distanceChanged)
            {
                _distanceChanged = false;
                _currentGrabDistance = Vector3.Distance(ray.origin, holdPoint);
            }

            StartPoint = _laserStartPoint.transform.position;
            MidPoint = holdPoint;
            EndPoint = _grabbedTransform.TransformPoint(_hitOffsetLocal);
        }
    }

    private void Update ()
    {
        if (!Input.GetMouseButton(0))
        {
            if (_grabbedRigidbody != null)
            {                
                ReleaseObject();
            }

            _justReleased = false;
            return;
        }

        if (_grabbedRigidbody == null && !_justReleased)
        {
            Ray ray = CenterRay();
            RaycastHit hit;
                       
            Debug.DrawRay(ray.origin, ray.direction * _maxGrabDistance, Color.blue, 0.01f);

            if (Physics.Raycast(ray, out hit, _maxGrabDistance, _grabLayer))
            {
                if (hit.rigidbody != null /*&& !hit.rigidbody.isKinematic*/)
                {
                    gameObject.GetComponent<AudioSource>().clip = onGrabbed;
                    gameObject.GetComponent<AudioSource>().Play();
                    _grabbedRigidbody = hit.rigidbody;
                    _grabbedRigidbody.transform.parent = playerTransform.transform;
                    _wasKinematic = _grabbedRigidbody.isKinematic;
                    _grabbedRigidbody.isKinematic = false;
                    if (!_grabbedRigidbody.freezeRotation)
                    {
                        _grabbedRigidbody.freezeRotation = true;
                    }
                    else
                    {
                        hasFreezedRotation = true;
                    }
                    _initialInterpolationSetting = _grabbedRigidbody.interpolation;
                    _rotationDifference = Quaternion.Inverse(playerTransform.rotation) * _grabbedRigidbody.rotation;
                    _hitOffsetLocal = hit.transform.InverseTransformVector(hit.point - hit.transform.position);
                    _currentGrabDistance = hit.distance; 
                    _grabbedTransform = _grabbedRigidbody.transform;
                    _grabbedRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                    OnObjectGrabbed.Invoke(_grabbedRigidbody.gameObject);
                    Debug.DrawRay(hit.point, hit.normal * 10f, Color.red, 10f);
                }
            }
        }
        else if(_grabbedRigidbody != null)
        {
            _userRotation = Input.GetKey(Rotate);

            if (Input.GetKeyDown(Rotate))
            {
                _desiredRotation = _grabbedRigidbody.rotation;
            }

            if (Input.GetKey(ResetRotation))
            {
                _grabbedRigidbody.MoveRotation(Quaternion.identity);  
            }

            if (Input.GetKey(Rotate))
            {
                var rotateZ = Input.GetKey(RotateZ);

                var increaseSens = Input.GetKey(RotationSpeedIncrease) ? 2.5f : 1f;

                if(Input.GetKeyDown(SwitchAxis))
                {
                    _rotationAxis = !_rotationAxis;

                    OnAxisChanged.Invoke(_rotationAxis);
                }

                if (Input.GetKeyDown(SnapRotation))
                {
                    _snapRotation = true;

                    var newRot = _grabbedRigidbody.transform.rotation.eulerAngles;

                    newRot.x = Mathf.Round(newRot.x / SnapRotationDegrees) * SnapRotationDegrees;
                    newRot.y = Mathf.Round(newRot.y / SnapRotationDegrees) * SnapRotationDegrees;
                    newRot.z = Mathf.Round(newRot.z / SnapRotationDegrees) * SnapRotationDegrees;

                    var rot = Quaternion.Euler(newRot);

                    _grabbedRigidbody.MoveRotation(rot);

                    _desiredRotation = rot;
     
                }
                else if(Input.GetKeyUp(SnapRotation))
                {
                    _snapRotation = false;
                }

                var x = Input.GetAxisRaw("Mouse X");
                var y = Input.GetAxisRaw("Mouse Y");

                if (Mathf.Abs(x) > _rotationTollerance)
                {
                    _rotationInput.x = rotateZ ? 0f : x * _rotationSenstivity * increaseSens;                   
                    _rotationInput.z = rotateZ ? x * _rotationSenstivity * increaseSens : 0f;
                }

                if(Mathf.Abs(y) > _rotationTollerance)
                {
                    _rotationInput.y = y * _rotationSenstivity * increaseSens;
                }
            }
            else
            {
                _snapRotation = false;
            }

            /*
            var direction = Input.GetAxis("Mouse ScrollWheel");
            
            //Move Object Foward Or backwards
            if (Input.GetKeyDown(KeyCode.E))
                direction = -0.1f;
            else if (Input.GetKeyDown(KeyCode.Q))
                direction = 0.1f;

            if (Mathf.Abs(direction) > 0 && CheckObjectDistance(direction))
            {
                _distanceChanged = true;
                _scrollWheelInput = playerTransform.forward * _scrollWheelSensitivity * direction;
            } 
            else
            {
                _scrollWheelInput = _zeroVector3;
            }
            */

            //Freeze
            if(Input.GetKeyDown(Freeze))
            {
                _grabbedRigidbody.collisionDetectionMode = !_wasKinematic ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.Continuous;
                _grabbedRigidbody.isKinematic = _wasKinematic = !_wasKinematic;

                _justReleased = true;
                ReleaseObject();
            }
        }
	}

    private void RotateGrabbedObject()
    {
        if (_grabbedRigidbody == null)
            return;

        _grabbedRigidbody.MoveRotation(Quaternion.Lerp(_grabbedRigidbody.rotation, _desiredRotation, Time.fixedDeltaTime * _rotationSpeed));
    }

    private void UpdateRotationAxis()
    {
        if (!_snapRotation)
        {
            _forward = playerTransform.forward;
            _right = playerTransform.right;
            _up = playerTransform.up;
            return;
        }

        if (_rotationAxis)
        {
            _forward = _grabbedTransform.forward;
            _right = _grabbedTransform.right;
            _up = _grabbedTransform.up;
            return;
        }

        NearestTranformDirection(_grabbedTransform, playerTransform, ref _up, ref _forward, ref _right);
    }

    private void NearestTranformDirection(Transform transformToCheck, Transform referenceTransform, ref Vector3 up, ref Vector3 forward, ref Vector3 right)
    {
        var directions = new List<Vector3>()
        {
            transformToCheck.forward,
            -transformToCheck.forward,
            transformToCheck.up,
            -transformToCheck.up,
            transformToCheck.right,
            -transformToCheck.right,
        };

        up = GetDirectionVector(directions, referenceTransform.up);
        directions.Remove(up);
        directions.Remove(-up);      
        forward = GetDirectionVector(directions, referenceTransform.forward);
        directions.Remove(forward);
        directions.Remove(-forward);

        right = GetDirectionVector(directions, referenceTransform.right);

    }

    private Vector3 GetDirectionVector(List<Vector3> directions, Vector3 direction)
    {
        var maxDot = -Mathf.Infinity;
        var ret = Vector3.zero;

        for (var i = 0; i < directions.Count; i++)
        {
            var dot = Vector3.Dot(direction, directions[i]);

            if (dot > maxDot)
            {
                ret = directions[i];
                maxDot  = dot;
            }
        }

        return ret;
    }     

    private Ray CenterRay()
    {
        return _camera.ViewportPointToRay(_oneVector3 * 0.5f);
    }

    private bool CheckObjectDistance(float direction)
    {
        var pointA = playerTransform.position;
        var pointB = _grabbedRigidbody.position;

        var distance = Vector3.Distance(pointA, pointB);

        if (direction > 0)
            return distance <= _maxGrabDistance;

        if (direction < 0)
            return distance >= _minObjectDistance;

        return false;
    }

    public bool IsObjectGrabbed()
    {
        if (_grabbedRigidbody != null) return true;
        else return false;
    }

    public void ReleaseObject()
    {
        gameObject.GetComponent<AudioSource>().clip = onReleased;
        gameObject.GetComponent<AudioSource>().Play();
        _grabbedRigidbody.MoveRotation(_desiredRotation);
        _grabbedRigidbody.isKinematic = _wasKinematic;
        _grabbedRigidbody.interpolation = _initialInterpolationSetting;
        if (hasFreezedRotation)
        {
            _grabbedRigidbody.freezeRotation = true;
        }
        else
        {
            _grabbedRigidbody.freezeRotation = false;
        }
        _grabbedRigidbody.transform.parent = null;
        _grabbedRigidbody = null;
        _scrollWheelInput = _zeroVector3;
        _grabbedTransform = null;
        _userRotation = false;
        _snapRotation = false;
        StartPoint = _zeroVector3;
        MidPoint = _zeroVector3;
        EndPoint = _zeroVector3;

        OnObjectGrabbed.Invoke(null);
    }
}

