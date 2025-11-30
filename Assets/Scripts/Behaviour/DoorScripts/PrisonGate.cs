using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PrisonGate : MonoBehaviour,DoorInterface
{
    [SerializeField] private RaycastController Raycast; //Adquirimos el Raycast desde la escena.
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI InteractionText;
    [SerializeField] private Image keyImage;
    KeyController key;
    private string objectName;
    private string objectTag;
    private Outline objectOutline;
    private Animator animator;
    private float DoorRotation = 0f;
    private float dot;


    private bool haveKey = false;
        public void Start() { objectOutline = GetComponent<Outline>(); animator = GetComponent<Animator>(); key = GameObject.Find("PrisonGate Key(Clone)").GetComponent<KeyController>();}
    public void Update()
    {
        haveKey = key.GetKey();
         OutlineChanger(); //Cambia el color del outline.
         ChangeInteractionText();

        if (haveKey) 
        { 
        CalculatePlayerPosition(); //Calculamos si está delante o detrás de la puerta.
        ((DoorInterface)this).OpenCloseDoor(); //Si el usuario está delante de la puerta, la abre, si está detrás la cierra
        }
    }

    public void OutlineChanger() { if (Raycast.GetHitObjectName() == "Prison Gate") { objectOutline.OutlineColor = Color.white; objectOutline.OutlineWidth = 2.0f; } else { objectOutline.OutlineColor = new Color(0, 0, 0, 0); } }
    public void CalculatePlayerPosition() { Vector3 dir = (player.position - transform.position).normalized; dot = Vector3.Dot(transform.right, dir);}
    public void ChangeInteractionText() { if (Raycast.GetHitObjectName() == "Prison Gate") { if (!haveKey) { InteractionText.text = "Necesitas una llave para abrir esta puerta"; } } else { InteractionText.text = ""; } }
    void DoorInterface.OpenCloseDoor() { if (Raycast.GetHitObjectName() == "Prison Gate" && Input.GetMouseButton(0)) { if (animator.GetFloat("DoorRotation") < 5 && dot >= 0) { DoorRotation += 1.5f * Time.deltaTime; animator.SetFloat("DoorRotation", DoorRotation); } else if (animator.GetFloat("DoorRotation") >= 0 && dot < 0) { if (dot > -2) { DoorRotation -= 1.5f * Time.deltaTime; animator.SetFloat("DoorRotation", DoorRotation); } } } }

    public void interactionTextForKey() { InteractionText.text = "Click to grab the key"; }
    
}
