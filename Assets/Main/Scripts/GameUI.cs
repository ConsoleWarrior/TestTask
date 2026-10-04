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
        PlayerHealth.Died += OnDied;
        Finish.Reached += OnFinish;
    }

    void OnDisable()
    {
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

    void OnDied()
    {
        // перезапуск - кнопкой Restart в этом меню
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
