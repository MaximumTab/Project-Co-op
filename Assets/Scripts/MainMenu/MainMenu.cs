using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Selectable firstSelectable;
    private InputSystem_Actions inputActions;

    private void Start()
    {
        inputActions = InputManager.Instance.Actions;
        inputActions.UI.Enable();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);

        InputDetector.OnInputTypeChanged += HandleInputTypeChange;
    }

    private void OnDestroy()
    {
        InputDetector.OnInputTypeChanged -= HandleInputTypeChange;
    }

    private void HandleInputTypeChange(InputType inputType)
    {
        if (inputType == InputType.MouseKeyboard)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if (inputType == InputType.Controller)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
