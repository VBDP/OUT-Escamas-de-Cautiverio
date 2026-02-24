using UnityEngine;
using UnityEngine.UI;

public class DamageOnTouch : MonoBehaviour
{
    public int customDamage = 50;
    private LifeSystem lifeSystem;
    private bool haveBeenHit;

    void Start()
    {
        lifeSystem = FindFirstObjectByType<LifeSystem>();
        haveBeenHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (lifeSystem.currentHealth - customDamage > 0)
            {
                lifeSystem.DamagePlayer(customDamage);
                Debug.Log("Player hit by " + gameObject.name + " and took " + customDamage + " damage. Current health: " + lifeSystem.currentHealth);
            }
            else
            {
                lifeSystem.KillPlayer();
            }
        }
    }

        private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (!haveBeenHit)
            {
                if (lifeSystem.currentHealth - customDamage > 0)
                {
                    lifeSystem.DamagePlayer(customDamage);
                    Debug.Log("Player hit by " + gameObject.name + " and took " + customDamage + " damage. Current health: " + lifeSystem.currentHealth);
                }
                else
                {
                    lifeSystem.KillPlayer();
                }
                haveBeenHit = true;
            }

        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            haveBeenHit = false;
        }
    }
}
