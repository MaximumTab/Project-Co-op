using UnityEngine;

public class InputIconSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] keyboardIcons;
    [SerializeField] private GameObject[] controllerIcons;

    private void OnEnable()
    {
        InputDetector.OnInputTypeChanged += UpdateIcons;
        UpdateIcons(InputDetector.CurrentInput); 
    }

    private void OnDisable()
    {
        InputDetector.OnInputTypeChanged -= UpdateIcons;
    }

    private void UpdateIcons(InputType inputType)
    {
        bool usingController = inputType == InputType.Controller;

        foreach (var obj in keyboardIcons)
            obj.SetActive(!usingController);

        foreach (var obj in controllerIcons)
            obj.SetActive(usingController);
    }
}
