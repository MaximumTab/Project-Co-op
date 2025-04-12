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

    public void ShowTooltip(string message)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = message;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    void Update()
{
    if (tooltipPanel.activeSelf)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tooltipPanel.transform.parent as RectTransform,
            Input.mousePosition,
            null,
            out pos
        );
        tooltipPanel.transform.localPosition = pos + new Vector2(0, 30); 
    }
}

}