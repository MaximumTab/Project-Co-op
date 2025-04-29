using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabMenu : MonoBehaviour
{
    [SerializeField] private GameObject TabMenuWindow;
    private bool isPaused = false;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
    }

    private void OnEnable()
    {
        inputActions.Player.Disable();
        
        inputActions.UI.SkillMenu.performed += ctx => ToggleMenu();
    }

    private void OnDisable()
    {
        inputActions.UI.SkillMenu.performed -= ctx => ToggleMenu();
        inputActions.Player.Enable();
    }

    void Start()
    {
        //ToggleMenu(); dont show on start
    }

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
            inputActions.Player.Disable();
            SkillParent.Instance.SkillpointText();
        }
        else
        {
            Time.timeScale = 1;
            TabMenuWindow.SetActive(false);
            inputActions.Player.Enable();
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
