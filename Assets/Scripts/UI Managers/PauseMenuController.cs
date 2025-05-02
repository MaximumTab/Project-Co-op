using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        inputActions.Player.Disable();
        inputActions.UI.Enable();
        inputActions.UI.PauseMenu.performed += ctx => TogglePause(); 
    }

    private void OnDisable()
    {
        inputActions.UI.PauseMenu.performed -= ctx => TogglePause();
        inputActions.Player.Enable();
        inputActions.UI.Disable();  
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
            inputActions.Player.Disable();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuWindow.SetActive(false);
            inputActions.Player.Enable();
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
}
