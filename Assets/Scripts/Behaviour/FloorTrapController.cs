using UnityEngine;

public class FloorTrapController : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void ActivarTrampa()
    {
        anim.SetBool("Activated", true);
    }

    public void DesactivarTrampa()
    {
        anim.SetBool("Activated", false);
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
