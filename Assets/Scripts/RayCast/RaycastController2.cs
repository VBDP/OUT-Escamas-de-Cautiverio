using UnityEngine;

public class RaycastController2 : MonoBehaviour
{
    public string objectName;
    public string objectTag;
    public Outline outline;
    
    public void Update()
    {
       // Si el raycast de la cámara golpea algo, guarda el nombre y la etiqueta del objeto golpeado         
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 5))
        {
                   objectName = hit.collider.gameObject.name;
                   objectTag = hit.collider.gameObject.tag; 
                   outline = hit.collider.gameObject.GetComponent<Outline>();
                   
        }
    }

        //Para recuperar la información del raycast continuamente
    public string GetHitObjectName() //Recupera el nombre del objeto golpeado
    {
        return objectName;
    }
    public string GetHitObjectTag() //Recupera la etiqueta del objeto golpeado
    {
        return objectTag;
    }
    public Outline GetHitObjectOutline() //Recupera el componente Outline del objeto golpeado
    {
        return outline;
    }
}
