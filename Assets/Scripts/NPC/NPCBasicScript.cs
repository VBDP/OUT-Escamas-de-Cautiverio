using UnityEngine;
using System.Collections.Generic;

using TMPro;

public class NPCBasicScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCTextBox;
    [SerializeField] private RaycastController raycast;
    [SerializeField] private Outline outline;
    [SerializeField] private List<string> Frases;
        private int count = 0;
    

    void Update()
    {
        outline.OutlineColor = new Color(0,0,0,0);
        if (raycast.GetHitObjectName() == "NPC")
        {
            outline.OutlineColor= Color.white;
            if (Input.GetMouseButtonDown(0))
            {
                NPCTextBox.text = Frases[count];
                if (count < Frases.Count - 1)
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
