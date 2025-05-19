using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance; 
    
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform notificationContainer;   //window that holds the notifs
    [SerializeField] private float displayDuration = 3f;
    // [SerializeField] private float fadeDuration = 0.5f;
    //[SerializeField] private Animator animator;
    
    private static readonly int SlideOut = Animator.StringToHash("SlideOut");

    void Awake()
    { 
        if (Instance == null){ Instance = this;}
        else {Destroy(gameObject);}
        
    }
    
    public void ShowNotification(string message)
    {
        GameObject notifInstance = Instantiate(notificationPrefab, notificationContainer);
        TextMeshProUGUI notifText = notifInstance.GetComponentInChildren<TextMeshProUGUI>(); 
        if (notifText != null) {notifText.text = message;}    //if there is text, set to incoming argument
        Animator notifAnimator = notifInstance.GetComponentInChildren<Animator>();
        StartCoroutine(HandleNotification(notifInstance, notifAnimator));
    }

    private IEnumerator HandleNotification(GameObject notifObj, Animator notifAnimator)
    {
        // // Logic for fading or sliding
        // CanvasGroup canvasGroup = notifInstance.GetComponent<CanvasGroup>();
        //
        // // Fade in
        // float t = 0f;
        // while (t < fadeDuration)
        // {
        //     t += Time.deltaTime;
        //     canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
        //     yield return null;
        // }
        //
        // yield return new WaitForSeconds(displayDuration);
        //
        // // Fade out
        // t = 0f;
        // while (t < fadeDuration)
        // {
        //     t += Time.deltaTime;
        //     canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
        //     yield return null;
        // }
        //
        // Destroy(notifInstance);
        
        
        
        
        //Animator animator = notifObj.GetComponent<Animator>();
    
       // animator.SetTrigger("SlideIn");
        
       
       
       
       
        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Trigger slide-out animation
        notifAnimator.SetTrigger(SlideOut);
        
        AnimatorStateInfo stateInfo = notifAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        // Wait for the slide-out animation to finish
       // float animationLength = 0.1f; // might need to access animation clip itself or to check if this clip has finished
        //yield return new WaitForSeconds(displayDuration);

        Destroy(notifObj);
        
        
    
    }

    
    
    
}
