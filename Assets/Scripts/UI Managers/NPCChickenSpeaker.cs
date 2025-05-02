using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NPCChickenSpeaker : MonoBehaviour
{
    [SerializeField] private TextMeshPro textBubble;
    [TextArea]
    [SerializeField] private string dialogueLine = "";
    [SerializeField] private float displayTime = 6f;
    [SerializeField] private InputActionReference interactAction;

    private bool playerInRange = false;
    private Transform playerTransform;

    private void Start()
    {
        interactAction?.action.Enable();
        textBubble?.gameObject.SetActive(false);
        InteractPrompt.Instance?.HidePrompt();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        playerTransform = other.transform;

        InteractPrompt.Instance?.ShowPrompt("Speak to chicken");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerTransform = null;

        if (textBubble != null && textBubble.gameObject.activeSelf)
            StartCoroutine(HideAfterDelay());

        InteractPrompt.Instance?.HidePrompt();
    }

    private void Update()
    {
        if (textBubble != null && textBubble.gameObject.activeSelf && playerTransform != null)
        {
            Vector3 direction = textBubble.transform.position - playerTransform.position;
            direction.y = 0;
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

        InteractPrompt.Instance?.HidePrompt();
    }

    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        if (textBubble != null)
            textBubble.gameObject.SetActive(false);

        if (playerInRange)
        InteractPrompt.Instance?.ShowPrompt("Speak to chicken");
    }
}
