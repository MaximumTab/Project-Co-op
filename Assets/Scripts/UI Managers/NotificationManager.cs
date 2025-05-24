using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    /* --- How to use ---
     *  call:
     *      NotificationManager.Instance.ShowNotification("<message that'll display in notification>");
     * -----------------
     */ 
    
    public static NotificationManager Instance; 
    
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform notificationContainer;   //window that holds the notifs
    [SerializeField] private float displayDuration = 3f;
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
        yield return new WaitForSeconds(displayDuration);   //amount of time the notif is on screen
        
        notifAnimator.SetTrigger(SlideOut); 
        
        AnimatorStateInfo stateInfo = notifAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;   //animation clip duration

        yield return new WaitForSeconds(animationLength);

        Destroy(notifObj);
    }

    
    
    
}
