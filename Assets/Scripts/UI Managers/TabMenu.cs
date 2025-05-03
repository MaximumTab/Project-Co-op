using UnityEngine;
using UnityEngine.InputSystem;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private GameObject TabMenuWindow;
    private bool isPaused = false;
    private InputSystem_Actions inputActions;

    private void OnEnable()
    {
        inputActions = InputManager.Instance.Actions;
        inputActions.UI.Enable();
        inputActions.UI.SkillMenu.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.UI.SkillMenu.performed -= ToggleMenu;
        }
    }

    void ToggleMenu(InputAction.CallbackContext ctx) => ToggleMenu();

    void ToggleMenu()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        isPaused = !isPaused;
        Debug.Log("isPaused is currently = " + isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            TabMenuWindow.SetActive(true);
            SkillParent.Instance.SkillpointText();
        }
        else
        {
            Time.timeScale = 1;
            TabMenuWindow.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
