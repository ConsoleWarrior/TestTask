using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, -11f);
    [SerializeField] private float smooth = 5f;

    void Start()
    {
        // если цель не указана - ищем игрока сами
        if (target == null)
        {
            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
            if (player != null)
                target = player.transform;
        }

        if (target != null)
            transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset, smooth * Time.deltaTime);
    }
}
