using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuWindow;
    private bool isPaused = false; // Boolean to track whether the game is paused or not


    // Update is called once per frame
    void Update()
    {
        // Check if the 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }


    void TogglePause()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        // Toggle the pause state
        isPaused = !isPaused;
        Debug.Log("ispaused is currently = " + isPaused);

        // Set the time scale based on the pause state
        if (isPaused)
        {
            Debug.Log("Reaches is paused condition");
            Time.timeScale = 0; // Pauses the game
            pauseMenuWindow.SetActive(true);
            
        }
        else
        {
            Time.timeScale = 1; // Resumes the game
            pauseMenuWindow.SetActive(false);

            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        }

    }

}
