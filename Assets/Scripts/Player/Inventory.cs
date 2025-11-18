using UnityEngine;

public class Inventory : MonoBehaviour
{

    public bool KeyFirstDoor = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetKeyFirstDoor()
    {
        KeyFirstDoor = true;    
    }

    public bool returnKeyFirstDoor(bool keyFirstDoor)
    {
        return KeyFirstDoor;
    }
}
