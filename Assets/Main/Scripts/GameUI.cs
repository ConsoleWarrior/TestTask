using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private TMP_Text healthText;

    private bool gameOver;

    void Awake()
    {
        // без EventSystem кнопки не нажимаются, если на сцене его нет - создаем
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<InputSystemUIInputModule>();
        }
    }

    void OnEnable()
    {
        PlayerHealth.HealthChanged += OnHealthChanged;
        PlayerHealth.Died += OnDied;
        Finish.Reached += OnFinish;
    }

    void OnDisable()
    {
        PlayerHealth.HealthChanged -= OnHealthChanged;
        PlayerHealth.Died -= OnDied;
        Finish.Reached -= OnFinish;
    }

    void Start()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
    }

    void Update()
    {
        if (gameOver || Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (pauseMenu.activeSelf)
                Continue();
            else
                Pause();
        }
    }

    void OnHealthChanged(int health)
    {
        healthText.text = "HP: " + health;
    }

    void OnDied()
    {
        // сам рестарт делает PlayerHealth, тут только показываем меню
        gameOver = true;
        pauseMenu.SetActive(false);
        loseMenu.SetActive(true);
    }

    void OnFinish()
    {
        gameOver = true;
        pauseMenu.SetActive(false);
        winMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    // методы ниже повешены на кнопки (OnClick в инспекторе)
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // в редакторе Quit не работает
#endif
    }
}
