using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuWindow;
    private bool isPaused = false; // Boolean to track whether the game is paused or not

    //[SerializeField] private Slider slider;
    //private CameraController cameraController;


    private void Start()
    {
        //cameraController = FindFirstObjectByType<CameraController>();
       // cameraController.GetXSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (isPaused)
        {
           // Debug.Log("Reaches is paused condition");
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

    /*
    public void ChangeCamSensitivity()
    {
        
        if (cameraController != null)
        {
            cameraController.SetXSensitivity(slider.value);
            cameraController.SetYSensitivity(slider.value);
        }
        else
        {
            Debug.Log("camera controller is null");
        }

    }
    */


}
