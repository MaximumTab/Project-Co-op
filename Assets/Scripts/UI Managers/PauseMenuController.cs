using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuWindow;
    private bool isPaused = false;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.PauseMenu.performed += ctx => TogglePause(); 
    }

    private void OnDisable()
    {
        inputActions.Player.PauseMenu.performed -= ctx => TogglePause();
        inputActions.Disable();
    }

    void TogglePause()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        isPaused = !isPaused;
        Debug.Log("isPaused is currently = " + isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenuWindow.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuWindow.SetActive(false);

            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
