using System;
using UnityEngine;

public class RunaPickup : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    [SerializeField] private GameObject hitObject;
    [SerializeField] private Transform tpPoint;

    void Start()
    {
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
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
                transform.position = new Vector3(0,-1000,0);
            }
        }
    }
}
