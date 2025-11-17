using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public float rayDistance  = 10f;
    public LineRenderer lineRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         // Crear un rayo desde la posición de la cámara hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Ejecutar el raycast
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Debug.Log("Impactó con: " + hit.collider.name);
            
            // Opcional: dibujar el rayo en la escena para depuración
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
        else
        {
            // Raycast no impactó, dibujar rayo máximo
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green);
        }
    }
}
