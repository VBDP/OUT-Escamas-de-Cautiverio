using UnityEngine;

public class RunePosition : MonoBehaviour
{
    [SerializeField] private RuneType runeType;
    [SerializeField] private Transform tpPoint;
    [SerializeField] private GameObject runeObject;

    public enum RuneType
{
    Jera,
    Othilla
}


    private Inventory inventario;

    void Start()
    {
        inventario = FindFirstObjectByType<Inventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (runeType)
        {
            case RuneType.Jera:
                if (inventario.Jera)
                    TeleportRune();
                break;

            case RuneType.Othilla:
                if (inventario.Othilla)
                    TeleportRune();
                break;
        }
    }

    private void TeleportRune()
    {
        runeObject.transform.SetPositionAndRotation(
            tpPoint.position,
            tpPoint.rotation
        );
    }
}
