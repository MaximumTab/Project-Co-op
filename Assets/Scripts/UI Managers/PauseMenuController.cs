using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuWindow;
 
    public void PauseBtn()
    {

        Time.timeScale = 0f;
        Debug.Log("time scale is currently = " + Time.timeScale);
        if(menuWindow != null)
        {
            menuWindow.SetActive(true);
        }
        

        //note for future: remember to change timescale back to 1 if redirecting scenes
    }

    public void PlayBtn()
    {
        Time.timeScale = 1f;
        Debug.Log("time scale is currently = " + Time.timeScale);
        menuWindow.SetActive(false);
    
    }
    
}
