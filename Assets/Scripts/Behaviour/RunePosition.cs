using UnityEngine;
using System.Collections;

public enum RuneType
{
    Jera,
    Othilla
}


public class RunePosition : MonoBehaviour
{
    [SerializeField] private RuneType runeType;
    [SerializeField] private Transform tpPoint;
    [SerializeField] private GameObject runeObject;
    [SerializeField] private float moveDuration = 1.2f;

    private Inventory inventario;
    private Coroutine moveCoroutine;

    public bool jera;
    public bool othilla;

    void Start()
    {
        inventario = FindFirstObjectByType<Inventory>();
        jera = false;
        othilla = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (runeType == RuneType.Jera && inventario.Jera)
        {
            StartMove();
            inventario.jeraPlaced = true;
        }
        else if (runeType == RuneType.Othilla && inventario.Othilla)
        {
            StartMove();
            inventario.othillaPlaced = true;
        }
    }

    void StartMove()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRuneSmooth());
    }

    IEnumerator MoveRuneSmooth()
    {
        Vector3 startPos = runeObject.transform.position;
        Quaternion startRot = runeObject.transform.rotation;

        float time = 0f;

        while (time < moveDuration)
        {
            float t = time / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            runeObject.transform.position =
                Vector3.Lerp(startPos, tpPoint.position, t);

            runeObject.transform.rotation =
                Quaternion.Slerp(startRot, tpPoint.rotation, t);

            time += Time.deltaTime;
            yield return null;
        }

        runeObject.transform.SetPositionAndRotation(
            tpPoint.position,
            tpPoint.rotation
        );

        // 🔒 Desactivar el componente RunaPickup
        var pickup = runeObject.GetComponent<RunaPickup>();
        if (pickup != null)
        {
            pickup.enabled = false;
        }
    }
}