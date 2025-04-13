using UnityEngine;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private GameObject TabMenuWindow;
    private bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        // Check if the 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }


    void ToggleMenu()
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
        TabMenuWindow.SetActive(true);

        // 💡 Update skillpoint text when the tab opens
        SkillParent.Instance.SkillpointText();
    }
    else
    {
        Time.timeScale = 1; // Resumes the game
        TabMenuWindow.SetActive(false);

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}
}