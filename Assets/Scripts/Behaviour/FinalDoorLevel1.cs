using UnityEngine;

public class FinalDoorLevel1 : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    [SerializeField] private GameObject finalDoorLevel1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(raycast.GetHitObjectName() == "FinalDoorLevel1")
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (inventario.Jera && inventario.Othilla)
                {
                    Debug.Log("Level Completed!");
                }
            }
        }
    }
}
