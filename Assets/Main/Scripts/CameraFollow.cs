using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, -11f);
    [SerializeField] private float smooth = 5f;

    void Awake()
    {
        // у игрока своя камера, если на сцене есть еще Main Camera (например стандартная) - выключаем ее
        foreach (Camera cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
        {
            if (cam.gameObject != gameObject && cam.CompareTag("MainCamera"))
                cam.gameObject.SetActive(false);
        }
    }

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
