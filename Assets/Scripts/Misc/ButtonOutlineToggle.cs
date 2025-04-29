using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingOutline : MonoBehaviour
{
    [SerializeField] private RectTransform outline;
    [SerializeField] private Vector2 padding = new Vector2(10, 10);

    void Update()
    {
        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null || outline == null) return;

        RectTransform target = selected.GetComponent<RectTransform>();
        if (target == null) return;

        outline.gameObject.SetActive(true);
        outline.position = target.position;
        outline.sizeDelta = target.sizeDelta + padding;
    }
}
