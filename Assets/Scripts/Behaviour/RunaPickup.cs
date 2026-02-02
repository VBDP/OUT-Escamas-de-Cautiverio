using System;
using UnityEngine;
using UnityEngine.UI;

public class RunaPickup : MonoBehaviour
{
    private RaycastController raycast;
    private Inventory inventario;
    private GeneralManager generalManager;
    [SerializeField] private GameObject hitObject;
    [SerializeField] private Transform tpPoint;
    [SerializeField] private GameObject blockLeft;
    [SerializeField] private GameObject blockRight;

    [SerializeField] private Image space1;
    [SerializeField] private Image space2;
    [SerializeField] private Sprite othillaSprite;
    [SerializeField] private Sprite jeraSprite;
    private AudioSource sfx;
    [SerializeField] private  AudioClip runePickup;

    void Start()
    {
        generalManager = FindObjectOfType<GeneralManager>();
        raycast = FindFirstObjectByType<RaycastController>();
        inventario = FindFirstObjectByType<Inventory>();
        blockLeft.SetActive(false);
        blockRight.SetActive(false);
        sfx = generalManager.sfxSource;
    }

    private void Update()
    {
        if (raycast.GetHitObjectName() == hitObject.name)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                if (hitObject.name == "Jera")
                {
                    sfx.PlayOneShot(runePickup);
                    inventario.Jera = true;
                    blockRight.SetActive(true);

                    if (space1.sprite == null)
                    {
                        space1.sprite = jeraSprite;
                        space1.gameObject.SetActive(true);
                    }
                    else
                    {
                        space2.sprite = jeraSprite;
                        space2.gameObject.SetActive(true);
                    }
                }
                else if (hitObject.name == "Othilla")
                {
                    sfx.PlayOneShot(runePickup);
                    inventario.Othilla = true;
                    blockLeft.SetActive(true);
                    if (space1.sprite == null)
                    {
                        space1.sprite = othillaSprite;
                        space1.gameObject.SetActive(true);
                    }
                    else
                    {
                        space2.sprite = othillaSprite;
                        space2.gameObject.SetActive(true);
                    }
                }

                transform.position = new Vector3(tpPoint.position.x, tpPoint.position.y, tpPoint.position.z + 1000);
            }
        }
    }
}