using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuWindow;
    [SerializeField] private Selectable firstSelectable; // e.g., SFX slider
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
       // Debug.Log("isPaused is currently = " + isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenuWindow.SetActive(true);
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            if (InputDetector.CurrentInput == InputType.Controller && firstSelectable != null)
            {
                EventSystem.current.SetSelectedGameObject(null); // clear first to force reselection
                EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
            }
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuWindow.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            EventSystem.current.SetSelectedGameObject(null);
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

