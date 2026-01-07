using UnityEngine;
using UnityEngine.UI;

public class DamageOnTouch : MonoBehaviour
{
    public int customDamage = 50;
    private LifeSystem lifeSystem;

    void Start()
    {
        lifeSystem = FindObjectOfType<LifeSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            if (lifeSystem.CurrentHealth - customDamage > 0)
            {
                lifeSystem.DamagePlayer(customDamage);
                lifeSystem.LifeImageFillAmount();
            }
            else
            {
                lifeSystem.KillPlayer();
            }
                    
        }
    }
}
