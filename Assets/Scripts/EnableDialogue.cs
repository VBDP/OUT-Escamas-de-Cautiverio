using UnityEngine;

public class EnableDialogue : MonoBehaviour
{
  [SerializeField] private GameObject canvas;
  [SerializeField] private GameObject player;
  [SerializeField] private Outline outline;
  private RaycastController raycast;
  private Dialogues dialogues;
  private DialogueInteraction dialogueInteraction;
  float distancia;

  void Start()
  {
    raycast = GameObject.Find("First Person Camera").GetComponent<RaycastController>();
    dialogues = GetComponent<Dialogues>();
    dialogueInteraction = canvas.GetComponent<DialogueInteraction>();
  }
  void Update()
  {
    outline.OutlineColor = new Color(0, 0, 0, 0);
    if (raycast.GetHitObjectName() == "NPC")
    {
      outline.OutlineColor = new Color(1, 1, 1, 1);
      if (Input.GetMouseButtonDown(0))
      {
        canvas.SetActive(true);
      }
    }

    if (Input.GetKeyUp(KeyCode.Escape))
    {
      canvas.SetActive(false);
      dialogues.Reset();
    }
    distancia = Vector3.Distance(player.transform.position, this.transform.position);

    if (distancia <= 3f)
    {
      if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
      {
        dialogueInteraction.Choice(0);
      }
      if (Input.GetKeyDown(KeyCode.Keypad1))
      {
        dialogueInteraction.Choice(0);
      }
      if (Input.GetKeyDown(KeyCode.Keypad2))
      {
        dialogueInteraction.Choice(1);
      }
      if (Input.GetKeyDown(KeyCode.Keypad2))
      {
        dialogueInteraction.Choice(2);
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
        dialogues.Reset();
        dialogueInteraction.TalkAgain();
      }
    }

    if (distancia > 3f)
    {
      canvas.SetActive(false);
      dialogues.Reset();
    }

  }
}