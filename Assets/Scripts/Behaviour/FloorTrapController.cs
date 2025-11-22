using UnityEngine;

public class FloorTrapController : MonoBehaviour
{
    public Animator animator;
  public void ActivarTrampa()
    {
        animator.SetBool("Activated", true);
    }

    public void DesactivarTrampa()
    {
        animator.SetBool("Activated", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Trampa activada");
            ActivarTrampa();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
             Debug.Log("Trampa desactivada");
            DesactivarTrampa();
        }
    }
}
