using UnityEngine;

public class Inventory : MonoBehaviour
{
    private GeneralManager generalManager;
    private LifeSystem lifeSystem;

    public bool keyFirstDoor = false;
    public bool Jera = false;
    public bool Othilla = false;

    public bool othillaPlaced = false;
    public bool jeraPlaced = false;

    public int pociones = 0;

    private AudioSource sfx;

    [SerializeField] private AudioClip potionDrink;

    void Start()
    {
        lifeSystem = FindFirstObjectByType<LifeSystem>();
        generalManager = FindFirstObjectByType<GeneralManager>();

        sfx = generalManager.sfxSource;
    }

    void Update()
    {
        if (pociones < 0)
        {
            pociones = 0;
        }

        // Solo se ejecuta cuando se presiona la tecla
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            UsarPociones();
        }
    }

    public void UsarPociones()
    {
        if (pociones > 0)
        {
            Debug.Log("Has usado una poción");

            // ✅ Singleton
            DataSaverForLogs.Instance.SetUsedPotions();

            if (lifeSystem.currentHealth < 100)
            {
                lifeSystem.HealPlayer(50);

                pociones--;

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