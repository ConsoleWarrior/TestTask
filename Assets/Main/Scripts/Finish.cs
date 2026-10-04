using System;
using UnityEngine;

// коллайдер на объекте должен быть с галкой Is Trigger
public class Finish : MonoBehaviour
{
    public static event Action Reached;

    private bool reached;

    void OnTriggerEnter(Collider other)
    {
        if (reached || other.GetComponent<PlayerHealth>() == null)
            return;

        reached = true;
        Debug.Log("Финиш!");
        Reached?.Invoke();
    }
}
