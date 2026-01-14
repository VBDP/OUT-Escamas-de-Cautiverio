using System;
using UnityEngine;

public class RunaPickup : MonoBehaviour
{
    private RaycastController raycast;
    private LifeSystem lifeSystem;
    private Inventory inventario;
    [SerializeField] private GameObject hitObject;
    [SerializeField] private Transform tpPoint;

    void Start()
    {
        lifeSystem = FindObjectOfType<LifeSystem>();
        raycast = FindObjectOfType<RaycastController>();
        inventario = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if (raycast.GetHitObjectName() == hitObject.name)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hitObject.name == "Jera")
                {
                    inventario.Jera = true;
                }
                else if (hitObject.name == "Othilla")
                {
                    inventario.Othilla = true;
                }
            }
        }
    }
}
