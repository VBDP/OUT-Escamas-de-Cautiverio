using UnityEngine;

public class PrisonGate : MonoBehaviour,DoorInterface
{
    DoorInterface doorInterface;
    bool isOpen = false;

    public void Start()
    {
        doorInterface = this as DoorInterface;
    }

public void Update()
    {
        isOpened();
        doorInterface.OpenCloseDoor();
    }

    void DoorInterface.OpenCloseDoor()
    {
       if(!isOpen && transform.localRotation.y > -0.1f)
        {   
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - 0.1f, transform.localRotation.eulerAngles.z);  
        }
        else
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 0.1f, transform.localRotation.eulerAngles.z);
        }
    }

    void isOpened()
    {
        if(transform.localRotation.y <= -0.1f)
        {
            isOpen = true;
        }
        else if(transform.localRotation.y >= 0f)
        {
            isOpen = false;
        }

    }
}
