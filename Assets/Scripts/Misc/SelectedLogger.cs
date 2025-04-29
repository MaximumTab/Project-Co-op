using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedLogger : MonoBehaviour
{
    public bool logEnabled = false;
    private GameObject lastSelected;

    void Update()
    {
        if (!logEnabled) return;

        GameObject current = EventSystem.current?.currentSelectedGameObject;
        if (current != lastSelected)
        {
            lastSelected = current;
            Debug.Log("Selected: " + (current != null ? current.name : "None"));
        }
    }
}
