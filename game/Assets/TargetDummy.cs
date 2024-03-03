using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour {

    public delegate void EntityDestroyedEventHandler(GameObject destroyedEntity);

    public event EntityDestroyedEventHandler OnEntityDestroyed;

    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Perform any actions when the enemy dies, such as playing death animations, spawning loot, etc.
        EventManager.TriggerEntityDestroyed(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Raise the event when the entity is destroyed
        if (OnEntityDestroyed != null)
        {
            OnEntityDestroyed(gameObject);
        }
    }
}
