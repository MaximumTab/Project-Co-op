using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;
    public TooltipScript tooltipManager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipManager.ShowTooltip(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipManager.HideTooltip();
    }
}
