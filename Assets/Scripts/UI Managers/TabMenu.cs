using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private GameObject TabMenuWindow;
    [SerializeField] public Selectable firstSelectable;

    private bool isPaused = false;
    private InputSystem_Actions inputActions;
    private InputAction tabAction;

    public static bool IsPaused { get; private set; }

    private void OnEnable()
    {
        inputActions = InputManager.Instance.Actions;
        inputActions.UI.Enable();

        tabAction = inputActions.UI.SkillMenu;
        tabAction.Enable();
    }

    private void OnDisable()
    {
        if (tabAction != null)
            tabAction.Disable();
    }

    private void Update()
    {
        if (tabAction != null && tabAction.triggered && !PauseMenuController.IsPaused)
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isPaused = !isPaused;
        IsPaused = isPaused;
        Debug.Log("isPaused is currently = " + isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            TabMenuWindow.SetActive(true);
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            SkillParent.Instance.SkillpointText();

            if (InputDetector.CurrentInput == InputType.Controller && firstSelectable != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
            }
        }
        else
        {
            Time.timeScale = 1;
            TabMenuWindow.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}