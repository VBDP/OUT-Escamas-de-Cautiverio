using UnityEngine;
using UnityEngine.UI;

public class DamageOnTouch : MonoBehaviour
{
    public int FloorTrapDamage;
    public LifeSystem LifeSystem;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            if (LifeSystem.CurrentHealth - FloorTrapDamage > 0)
            {
                LifeSystem.DamagePlayer(FloorTrapDamage);
                LifeSystem.LifeImageFillAmount();
            }
            else
            {
                LifeSystem.KillPlayer();
            }
                

            
        }
    }
}
