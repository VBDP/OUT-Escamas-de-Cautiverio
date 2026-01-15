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

    void Start()
    {
        inventario = FindFirstObjectByType<Inventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (runeType == RuneType.Jera && inventario.Jera)
            StartMove();

        if (runeType == RuneType.Othilla && inventario.Othilla)
            StartMove();
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

            // suavizado (ease in-out)
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
    }
}
