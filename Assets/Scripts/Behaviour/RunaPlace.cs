using UnityEngine;

public class RunaPlace : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycast = FindObjectOfType<RaycastController>();
        inventario = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (raycast.GetHitObjectName() == "JeraTP")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Jera");
            }
        }
        else if (raycast.GetHitObjectName() == "OthillaTP")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Jera");
            }
        }
    }
}
