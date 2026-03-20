using UnityEngine;

public class ZoneTimerManager : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        DataManager.Instance.EnterZone(other.tag);
    }

    void OnTriggerExit(Collider other)
    {
        DataManager.Instance.ExitZone(other.tag);
    }
}