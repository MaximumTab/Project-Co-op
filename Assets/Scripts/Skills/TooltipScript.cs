using TMPro;
using UnityEngine;

public class TooltipScript : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    private void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip(string message, RectTransform target = null)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = message;

        if (InputDetector.CurrentInput == InputType.Controller && target != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.transform.parent as RectTransform,
                target.position,
                null,
                out pos
            );

            tooltipPanel.transform.localPosition = pos + new Vector2(0, 30); 
        }
    }

    public void ShowErrorTooltip(string message)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = $"<color=red>{message}</color>";
    }


    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    void Update()
    {
        if (tooltipPanel.activeSelf && InputDetector.CurrentInput == InputType.MouseKeyboard)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipPanel.transform.parent as RectTransform,
                Input.mousePosition,
                null,
                out pos
            );
            tooltipPanel.transform.localPosition = pos + new Vector2(0,30);
        }
    }

}