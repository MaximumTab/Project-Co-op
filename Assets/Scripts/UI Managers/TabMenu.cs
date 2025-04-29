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
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.SkillMenu.performed += ctx => ToggleMenu();
    }

    private void OnDisable()
    {
        inputActions.Player.SkillMenu.performed -= ctx => ToggleMenu();
        inputActions.Disable();
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
