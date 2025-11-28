using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private string whatDoorOpens;
    private RaycastController Raycast;
    private bool take;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Raycast = GameObject.Find("First Person Camera").GetComponent<RaycastController>();
    }

    private void Update()
    {
        if (Raycast.GetHitObjectName() == "PrisonGate Key(Clone)") 
        {
            if (Input.GetMouseButtonDown(0))
            {
                SaveOnInventory();
            }
        }
    }

    private void SaveOnInventory()
    {
        Destroy(gameObject);
        take = true;
    }

    public bool GetKey()
    {
      return take;
    }


}
