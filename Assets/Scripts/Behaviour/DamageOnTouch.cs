using UnityEngine;
using UnityEngine.UI;

public class DamageOnTouch : MonoBehaviour
{
    public int customDamage = 50;
    private LifeSystem LifeSystem;

    void Start()
    {
        LifeSystem = GameObject.Find("Player").GetComponent<LifeSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            if (LifeSystem.CurrentHealth - customDamage > 0)
            {
                LifeSystem.DamagePlayer(customDamage);
                LifeSystem.LifeImageFillAmount();
            }
            else
            {
                LifeSystem.KillPlayer();
            }
                    
        }
    }
}
