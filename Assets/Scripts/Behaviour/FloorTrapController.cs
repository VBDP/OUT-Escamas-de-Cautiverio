using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FloorTrapController : MonoBehaviour
{
    private GeneralManager generalManager;
    private Animator anim;
    private AudioSource sfxAudioSource;
    public AudioClip clip;
    private void Start()
    {
        generalManager = FindFirstObjectByType<GeneralManager>();
        anim = GetComponent<Animator>();
        sfxAudioSource = generalManager.sfxSource;
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

            StartCoroutine(Wait(0.1f, () => ActivateTrap())); 

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trampa desactivada");
            StartCoroutine(Wait(0.5f, () => anim.SetBool("Activated", false))); 
        }
    }

    public void ActivateTrap()
    {
        anim.SetBool("Activated", true);
        sfxAudioSource.PlayOneShot(clip);

    }
}
