using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    [SerializeField] private PrisonGate prison;

    [SerializeField] private string whatDoorOpens;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioClip;
    private RaycastController Raycast;
    private bool take;
    private TextMeshProUGUI keyText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Raycast = GameObject.Find("First Person Camera").GetComponent<RaycastController>();
        prison = GameObject.Find("Prison Gate").GetComponent<PrisonGate>();
    }

    private void Update()
    {
        if (Raycast.GetHitObjectName() == "PrisonGate Key(Clone)") 
        {
            prison.interactionTextForKey();
            if (Input.GetMouseButtonDown(0))
            {
                SaveOnInventory();
                this.GetComponent<Renderer>().enabled = false;
                this.transform.Find("Luz").gameObject.SetActive(false);
                AudioSource.PlayOneShot(AudioClip);  
                Destroy(gameObject,1f);

                
            }
        }
    }

    private void SaveOnInventory()
    {
      take = true;
    }

    public bool GetKey()
    {
      return take;
    }


}
