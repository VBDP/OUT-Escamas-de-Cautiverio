using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public string objectName;
    public string objectTag;
    public Outline outline;
    public GameObject objectPrefab;
    private GeneralManager generalManager;

    public void Start()
    {
        generalManager = FindObjectOfType<GeneralManager>();
    }

    public void Update()
    {
        // Si el raycast de la cámara golpea algo, guarda el nombre y la etiqueta del objeto golpeado         
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 10, LayerMask.GetMask("Interactable")))
        {
            objectName = hit.collider.gameObject.name;
            objectTag = hit.collider.gameObject.tag;
            outline = hit.collider.gameObject.GetComponent<Outline>();
            objectPrefab = hit.collider.gameObject;

        }
        else
        {
            generalManager.ClearInteractionText();
            objectName = null;
            objectTag = null;
            outline = null;
            objectPrefab = null;
        }
    }

    //Para recuperar la información del raycast continuamente
    public string GetHitObjectName() //Recupera el nombre del objeto golpeado
    {
        if (objectName != null)
        {
            return objectName;
        }
        else
        {
            return "No hit";
        }
    }
    public string GetHitObjectTag() //Recupera la etiqueta del objeto golpeado
    {
        if (objectTag != null)
        {
            return objectTag;
        }
        else
        {
            return "No hit";
        }
    }
    public Outline GetHitObjectOutline() //Recupera el componente Outline del objeto golpeado
    {
        if (outline != null)
        {
            return outline;
        }
        else
        {
            return null;
        }
    }

    public GameObject GetHitGameObject()
    {
        if (objectPrefab != null)
        {
            return objectPrefab;
        }
        else
        {
            return null;
        }
    }
}
