using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private string whatDoorOpens;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioClip;
    private RaycastController Raycast;
    private bool take;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Raycast = GameObject.Find("First Person Camera").GetComponent<RaycastController>();
        
    }

    private void Update()
    {
        if (Raycast.GetHitObjectName() == "PrisonGate Key(Clone)") 
        {
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
