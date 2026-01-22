using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Inventory inventory;
    public LifeSystem lifeSystem;

    public bool keyFirstDoor = false;

    public bool Jera = false;

    public bool Othilla = false;

    public bool othillaPlaced = false;
    public bool jeraPlaced  = false;

    public int pociones = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        lifeSystem = FindObjectOfType<LifeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pociones < 0)
        {
            pociones = 0;
        }
        else
        {
            usarPociones();
        }
    }

    public void usarPociones()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && pociones > 0)
        {
            Debug.Log("Has usado una poción");
            lifeSystem.HealPlayer(50);
            pociones -= 1;
        }
    }
}
