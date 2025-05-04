using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public string message;
    public TooltipScript tooltipManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipManager.ShowTooltip(message, GetComponent<RectTransform>());
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.HideTooltip();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (InputDetector.CurrentInput == InputType.Controller)
        {
            tooltipManager.ShowTooltip(message, GetComponent<RectTransform>());
        }
    }


    public void OnDeselect(BaseEventData eventData)
    {
        if (InputDetector.CurrentInput == InputType.Controller)
        {
            tooltipManager.HideTooltip();
        }
    }
}
