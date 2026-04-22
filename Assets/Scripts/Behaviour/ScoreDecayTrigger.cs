using UnityEngine;

public class ScoreDecayTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador por su tag
        if (other.CompareTag("Player"))
        {
            GeneralManager gm = Object.FindFirstObjectByType<GeneralManager>();
            
            if (gm != null)
            {
                gm.StartScoreDecay();
                Debug.Log("Score decay started by trigger.");
                
                // Destruimos este trigger para que no se active más de una vez
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("ScoreDecayTrigger: No se encontró el GeneralManager en la escena.");
            }
        }
    }
}
