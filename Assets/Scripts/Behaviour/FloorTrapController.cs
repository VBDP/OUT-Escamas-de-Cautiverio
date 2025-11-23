using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FloorTrapController : MonoBehaviour
{
    Animator anim;
    AudioSource AudioSource;
   public AudioClip clip;
        private void Start()
    {
        anim = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
    }
    public void ActivarTrampa()
    {
        anim.SetBool("Activated", true);
    }

    public void DesactivarTrampa()
    {
        anim.SetBool("Activated", false);
    }

    IEnumerator Wait(float t, System.Action a) { yield return new WaitForSeconds(t); a(); } //Co-rutina para la espera.

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trampa activada");
            
            StartCoroutine(Wait(0.5f, () => ActivateTrap())); //Espera 1 segundo

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
             Debug.Log("Trampa desactivada");
            StartCoroutine(Wait(0.5f, () => anim.SetBool("Activated", false))); //Espera 1 segundo
        }
    }

    public void ActivateTrap()
    {
        anim.SetBool("Activated", true);
        AudioSource.Play();

    }
}
