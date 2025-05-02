using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCChickenSpeaker : MonoBehaviour
{
    [SerializeField] private TextMeshPro textBubble;
    [TextArea]
    [SerializeField] private string dialogueLine = "BAWK. The corn’s cursed.";
    [SerializeField] private float displayTime = 6f;
    [SerializeField] private InputActionReference interactAction;

    private bool playerInRange = false;
    private Transform playerTransform;

    private void Start()
    {
        if (interactAction != null)
            interactAction.action.Enable();

        if (textBubble != null)
            textBubble.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered with: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone!");
            playerInRange = true;
            playerTransform = other.transform;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left trigger zone!");
            playerInRange = false;
            playerTransform = null;

            // Start countdown only when leaving range
            if (textBubble != null && textBubble.gameObject.activeSelf)
                StartCoroutine(HideAfterDelay());
        }
    }


    private void Update()
    {
        // Always rotate toward player if text is visible and we still know their position
        if (textBubble != null && textBubble.gameObject.activeSelf && playerTransform != null)
        {
            Vector3 direction = textBubble.transform.position - playerTransform.position;
            direction.y = 0; // Keep upright
            textBubble.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (!playerInRange || interactAction == null) return;

        if (interactAction.action.WasPressedThisFrame())
            TriggerDialogue();
    }


    private void TriggerDialogue()
    {
        StopAllCoroutines();

        if (textBubble != null)
        {
            textBubble.text = dialogueLine;
            textBubble.gameObject.SetActive(true);
        }
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        if (textBubble != null)
            textBubble.gameObject.SetActive(false);
    }
}
