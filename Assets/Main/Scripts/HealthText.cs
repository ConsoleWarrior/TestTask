using TMPro;
using UnityEngine;

// надпись над головой: зеленое HP и красное "-1 HP" при уроне
public class HealthText : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float damageShowTime = 1f;

    private float hideTime;

    void LateUpdate()
    {
        // персонаж разворачивается, а текст всегда смотрит в камеру
        transform.rotation = Camera.main.transform.rotation;

        if (damageText.gameObject.activeSelf && Time.time > hideTime)
            damageText.gameObject.SetActive(false);
    }

    public void Show(int health, int damage)
    {
        hpText.text = "HP: " + health;

        if (damage > 0)
        {
            damageText.text = "-" + damage + " HP";
            damageText.gameObject.SetActive(true);
            hideTime = Time.time + damageShowTime;
        }
    }
}
