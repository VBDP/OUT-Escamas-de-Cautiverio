using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PrisonGate : MonoBehaviour, DoorInterface
{
    private RaycastController raycast;
    [SerializeField] private Transform player;
    private GeneralManager generalManager;
    [SerializeField] private Image keyImage;
    KeyController key;
    private string objectName;
    private string objectTag;
    private Outline objectOutline;
    private Animator animator;
    private float doorRotation = 0f;
    private float dot;
    private bool haveKey;

    public void Start()
    {
        objectOutline = GetComponent<Outline>();
        animator = GetComponent<Animator>();
        key = GameObject.Find("PrisonGate Key(Clone)").GetComponent<KeyController>();
        raycast = FindObjectOfType<RaycastController>();
        generalManager = FindObjectOfType<GeneralManager>();
        haveKey = false;
    }

    public void Update()
    {
        haveKey = key.GetKey();
        OutlineChanger(); //Cambia el color del outline.
        ChangeInteractionText(); 
        if (haveKey)
        {
            CalculatePlayerPosition(); //Calculamos si est� delante o detr�s de la puerta.
            ((DoorInterface)this).OpenCloseDoor(); //Si el usuario est� delante de la puerta, la abre, si est� detr�s la cierra 
        }
    }

    public void OutlineChanger()
    {
        if (raycast.GetHitObjectName() == "Prison Gate")
        {
            objectOutline.OutlineColor = Color.white; objectOutline.OutlineWidth = 2.0f;
        }
        else
        {
            objectOutline.OutlineColor = new Color(0, 0, 0, 0);
        }
    }
    public void CalculatePlayerPosition()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dot = Vector3.Dot(transform.right, dir);
    }
    public void ChangeInteractionText()
    {
        if (raycast.GetHitObjectName() == "Prison Gate")
        {
            if (!haveKey)
            {
                generalManager.SetInteractionText("Necesitas una llave para abrir esta puerta");
            }
            else
            {
                generalManager.SetInteractionText("Click para abrir/cerrar la puerta");
            }      
        }
    }

    void DoorInterface.OpenCloseDoor()
    {
        if (raycast.GetHitObjectName() == "Prison Gate" && Input.GetMouseButton(0))
        {
            if (animator.GetFloat("DoorRotation") < 5 && dot >= 0)
            {
                doorRotation += 1.5f * Time.deltaTime; animator.SetFloat("DoorRotation", doorRotation);
            }
            else if (animator.GetFloat("DoorRotation") >= 0 && dot < 0)
            {
                if (dot > -2)
                {
                    doorRotation -= 1.5f * Time.deltaTime; animator.SetFloat("DoorRotation", doorRotation);
                }
            }
        }
    }
}
