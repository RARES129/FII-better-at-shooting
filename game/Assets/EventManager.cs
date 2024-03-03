using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class EventManager : MonoBehaviour
{
    public static event Action<TargetDummy> EntityDestroyed;

    public static void TriggerEntityDestroyed(TargetDummy destroyedEntity)
    {
        EntityDestroyed?.Invoke(destroyedEntity);
    }
}
