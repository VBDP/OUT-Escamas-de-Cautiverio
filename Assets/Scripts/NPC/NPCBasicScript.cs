using UnityEngine;
using System.Collections.Generic;

using TMPro;

public class NPCBasicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcTextBox;
    [SerializeField] private RaycastController raycast;
    [SerializeField] private Outline outline;
    [SerializeField] private List<string> frases;
    private int count = 0;


    void Update()
    {
        outline.OutlineColor = new Color(0, 0, 0, 0);
        if (raycast.GetHitObjectName() == "NPC")
        {
            outline.OutlineColor = Color.white;
            if (Input.GetMouseButtonDown(0))
            {
                npcTextBox.text = frases[count];
                if (count < frases.Count - 1)
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
            }
        }
    }
}
