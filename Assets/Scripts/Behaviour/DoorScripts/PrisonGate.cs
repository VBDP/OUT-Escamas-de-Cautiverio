using UnityEngine;

public class PrisonGate : MonoBehaviour,DoorInterface
{
    DoorInterface doorInterface;
    bool isOpen = false;
    public RaycastController raycastController;
    public bool haveKey = false;
    public Inventory inventory;

    public void Start()
    {
        doorInterface = this as DoorInterface;
        raycastController = GameObject.Find("First Person Camera").GetComponent<RaycastController>();
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        if(raycastController)
        {
            Debug.LogError("RaycastController found on First Person Camera");
        }
        if(inventory)
        {
            Debug.LogError("Inventory found on Player");
        }
    }

public void Update()
    {
       
                isOpened();
        doorInterface.OpenCloseDoor();
    }

    void DoorInterface.OpenCloseDoor()
    {
        if(raycastController.GetHitObjectName() == "door01")
        {
            Debug.Log("Prison Gate interacted");
            if(Input.GetMouseButton(0))
            {
                if(isOpen == false)
                {
                    transform.Rotate(this.transform.localRotation.x,-1f,this.transform.localRotation.z);
                }
                else if(isOpen == true)
                {
                    transform.Rotate(this.transform.localRotation.x,1f,this.transform.localRotation.z);
                }
            }
        }
    }

    void isOpened()
    {
        if(transform.localRotation.y <= -0.7f && haveKey)
        {
            isOpen = true;
        }
        else if(transform.localRotation.y >= 0f && !haveKey)
        {
            isOpen = false;
        }
        else{
            isOpen = false;
        }

    }
}
