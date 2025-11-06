using System;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    private bool isOpen = false;                
    private Quaternion closedRotation;          
    private Quaternion openRotation;            
    private float rotationSpeed = 1.5f;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, 90, 0) * closedRotation; 
    }

    private void OnMouseDown()
    {
        Debug.Log("Has hecho click en la puerta");
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <  4 )
        {
            isOpen = !isOpen;
        }
        
            
        
    }

    void Update()
    {
        if (isOpen)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * rotationSpeed);
            }
       
    }
}
