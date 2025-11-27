using UnityEngine;
using UnityEngine.UI;
public class PrisonGate : MonoBehaviour,DoorInterface
{

    DoorInterface doorInterface;
    private RaycastController raycastController;
    private bool haveKey = false;
    private bool isOpen = false;
    private Outline outline;
    public TMPro.TextMeshProUGUI InteractionText;

//----------------------------------------------------------------------------------------------------------------------
    public void Start()
    {       
        outline = GetComponent<Outline>();
        doorInterface = this as DoorInterface;
        raycastController = GameObject.Find("First Person Camera").GetComponent<RaycastController>();
    }

public void Update()
    {
        doorInterface.OpenCloseDoor();
        outlinePuerta();
        comprobarApertura();
    }

//----------------------------------------------------------------------------------------------------------------------
    void DoorInterface.OpenCloseDoor()
    {
       
    }

    void outlinePuerta()
    {
         if(raycastController.GetHitObjectName() == "Prison Gate")
        {
           outline.OutlineColor = Color.white;
        }
        else
        {
            outline.OutlineColor = new Color(0,0,0,0);
        }
    }

    void comprobarApertura()
    {
        if(transform.localRotation.y <= -0.7f || transform.localRotation.y >= 0.7f)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }

//----------------------------------------------------------------------------------------------------------------------
}
