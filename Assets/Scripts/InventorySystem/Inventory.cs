using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GeneralManager generalManager;
    private LifeSystem lifeSystem;

    public bool keyFirstDoor = false;

    public bool Jera = false;

    public bool Othilla = false;

    public bool othillaPlaced = false;
    public bool jeraPlaced  = false;

    public int pociones = 0;

    private AudioSource sfx;

    [SerializeField] private AudioClip potionDrink;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifeSystem = FindObjectOfType<LifeSystem>();
        generalManager = FindObjectOfType<GeneralManager>();
        sfx = generalManager.sfxSource;
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
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            if (pociones > 0)
            {
                Debug.Log("Has usado una poción");

                if (lifeSystem.currentHealth < 100)
                {
                    lifeSystem.HealPlayer(50);
                    pociones -= 1;
                    generalManager.ChangePotionText(pociones.ToString());
                    sfx.PlayOneShot(potionDrink);
                }
                else
                {
                    Debug.Log("Ya tenías toda la vida");
                }
            }
            else
            {
                Debug.Log("No tienes pociones");
            }
        }
    }
}
