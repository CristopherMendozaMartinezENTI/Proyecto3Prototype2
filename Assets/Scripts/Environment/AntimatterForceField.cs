using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntimatterForceField : MonoBehaviour
{
    [SerializeField] private GameObject collisionTrailsPrefab;
    [SerializeField] private List<GameObject> ActiveScaffolding;
    [SerializeField] private List<GameObject> InnactiveScaffolding;

    private GameObject playerTelekinesis;
    private GameObject _collisionTrailsTmp;
    private bool destroy;

    private void Start()
    {
        playerTelekinesis = GameObject.Find("TelekinesisGaunlet");
    }

    public void OnTriggerStay(Collider other)
    {
        if(destroy && other.GetComponent<PropPhysicsController>() != null)
        {
            _collisionTrailsTmp = Instantiate(collisionTrailsPrefab, other.transform.position, other.transform.rotation);
            other.GetComponent<PropPhysicsController>().ResetPos();
            if (playerTelekinesis.GetComponent<TelekinesisController>().IsObjectGrabbed())
                playerTelekinesis.GetComponent<TelekinesisController>().ReleaseObject();
        }
    }

    private void Update()
    {
        Activate(GetComponent<ActiveStateManager>().active);

        if (destroy)
            Destroy(_collisionTrailsTmp, 5.0f);
    }

    public void Activate(bool state)
    {
        destroy = state;
        foreach (GameObject obj in ActiveScaffolding)
            obj.SetActive(state);
        foreach (GameObject obj in InnactiveScaffolding)
            obj.SetActive(!state);
    }
}
