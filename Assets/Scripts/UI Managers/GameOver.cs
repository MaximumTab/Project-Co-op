using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Selectable firstSelectable;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectable.gameObject);
        InputDetector.OnInputTypeChanged += HandleInputTypeChange;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
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

    public void Restart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ReturnTitle()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
