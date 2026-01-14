using System;
using UnityEngine;

public class RunaPickup : MonoBehaviour
{
    private RaycastController raycast;
    private LifeSystem lifeSystem;
    [SerializeField] private GameObject hitObject;
    [SerializeField] private Transform tpPoint;

    void Start()
    {
        lifeSystem = FindObjectOfType<LifeSystem>();
        raycast = FindObjectOfType<RaycastController>();
    }

    private void Update()
    {
        if (raycast.GetHitObjectName() == hitObject.name)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hitObject.transform.position = tpPoint.position;
                hitObject.transform.rotation = tpPoint.rotation;
            }
        }
    }
}
