using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuWindow;
    private bool isPaused = false;
    private InputSystem_Actions inputActions;
    private InputAction pauseAction;

    private void OnEnable()
    {
        inputActions = InputManager.Instance.Actions;
        inputActions.UI.Enable();

        pauseAction = inputActions.UI.PauseMenu;
        pauseAction.Enable();
    }

    private void Update()
    {
        if (pauseAction != null && pauseAction.triggered)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log("isPaused is currently = " + isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenuWindow.SetActive(true);
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuWindow.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
