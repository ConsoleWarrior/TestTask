using UnityEngine;

// коллайдер на объекте должен быть с галкой Is Trigger
public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    void OnTriggerStay(Collider other)
    {
        // пока игрок стоит в ловушке - бьет раз в invulnerableTime (это решает PlayerHealth)
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
            player.TakeDamage(damage);
    }
}
