using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    None,
    MouseKeyboard,
    Controller
}

public class InputDetector : MonoBehaviour
{
    public static InputType CurrentInput { get; private set; } = InputType.None;
    public static event System.Action<InputType> OnInputTypeChanged;

    private void OnEnable()
    {
        InputSystem.onActionChange += HandleInputAction;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= HandleInputAction;
    }


    private void HandleInputAction(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var action = obj as InputAction;
        if (action == null || action.activeControl == null) return;

        var device = action.activeControl.device;

        if (device is Gamepad)
        {
            UpdateInputType(InputType.Controller);
        }
        else if (device is Keyboard || device is Mouse)
        {
            UpdateInputType(InputType.MouseKeyboard);
        }
    }

    private void UpdateInputType(InputType newInput)
    {
        if (CurrentInput != newInput)
        {
            CurrentInput = newInput;
            OnInputTypeChanged?.Invoke(newInput);
        }
    }
}
