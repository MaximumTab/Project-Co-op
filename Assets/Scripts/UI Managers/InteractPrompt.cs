using UnityEngine;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    public static InteractPrompt Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private string keyboardButton = "Ｆ";
    [SerializeField] private string controllerButton = "↠";

    private string currentDescription = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        HidePrompt();
        InputDetector.OnInputTypeChanged += UpdateButtonOnly;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            InputDetector.OnInputTypeChanged -= UpdateButtonOnly;
    }

    private void UpdateButtonOnly(InputType input)
    {
        if (string.IsNullOrEmpty(currentDescription)) return;
        UpdatePrompt(currentDescription);
    }

    public void ShowPrompt(string description)
    {
        UpdatePrompt(description);
        SetPromptVisibility(true);
    }

    public void HidePrompt()
    {
        SetPromptVisibility(false);
        currentDescription = "";
    }

    private void UpdatePrompt(string description)
    {
        if (buttonText == null || descriptionText == null) return;

        currentDescription = description;

        string icon = InputDetector.CurrentInput switch
        {
            InputType.Controller => controllerButton,
            InputType.MouseKeyboard => keyboardButton,
            _ => ""
        };

        buttonText.text = icon;
        descriptionText.text = description;
    }

    private void SetPromptVisibility(bool visible)
    {
        buttonText?.gameObject.SetActive(visible);
        descriptionText?.gameObject.SetActive(visible);
    }
}
