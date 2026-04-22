using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            LifeSystem life = other.GetComponent<LifeSystem>();
            
            // Si no está en el objeto raíz, buscamos en los padres (por si el collider está en un hijo)
            if (life == null)
            {
                life = other.GetComponentInParent<LifeSystem>();
            }

            if (life != null)
            {
                Debug.Log("Jugador ha caído en un KillTrigger. Ejecutando KillPlayer.");
                life.KillPlayer();
            }
        }
    }
}
