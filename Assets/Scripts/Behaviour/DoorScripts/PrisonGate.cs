using System;
using TMPro;
using UnityEngine; 
public class PrisonGate : MonoBehaviour,DoorInterface
{
    [SerializeField] private RaycastController Raycast; //Adquirimos el Raycast desde la escena.
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI InteractionText;
    private string objectName;
    private string objectTag;
    private Outline objectOutline;
    private Animator animator;
    private float DoorRotation = 0f;
    private float dot;

    private bool haveKey = false;
        public void Start() { objectOutline = GetComponent<Outline>(); animator = GetComponent<Animator>();}
    public void Update()
    {
        OutlineChanger(); //Cambia el color del outline.
        CalculatePlayerPosition(); //Calculamos si está delante o detrás de la puerta.
        ChangeInteractionText();
        ((DoorInterface)this).OpenCloseDoor(); //Si el usuario está delante de la puerta, la abre, si está detrás la cierra
    }

    void OutlineChanger() { if (Raycast.GetHitObjectName() == "Prison Gate") { objectOutline.OutlineColor = Color.white; objectOutline.OutlineWidth = 2.0f; } else { objectOutline.OutlineColor = new Color(0, 0, 0, 0); } }
    public void CalculatePlayerPosition() { Vector3 dir = (player.position - transform.position).normalized; dot = Vector3.Dot(transform.right, dir); Debug.Log(dot); }
    void ChangeInteractionText() { if (Raycast.GetHitObjectName() == "Prison Gate") { if (!haveKey) { InteractionText.text = "Necesitas una llave para abrir esta puerta"; } } else { InteractionText.text = ""; } }
    void DoorInterface.OpenCloseDoor() { if (Raycast.GetHitObjectName() == "Prison Gate" && Input.GetMouseButton(0)) { if (animator.GetFloat("DoorRotation") < 5 && dot >= 0) { DoorRotation += 1.5f * Time.deltaTime; animator.SetFloat("DoorRotation", DoorRotation); } else if (animator.GetFloat("DoorRotation") >= 0 && dot < 0) { DoorRotation -= 1.5f * Time.deltaTime; animator.SetFloat("DoorRotation", DoorRotation); } } }
}
