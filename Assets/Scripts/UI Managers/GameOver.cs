using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void Restart()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ReturnTitle()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void FadeController()
    {
            
    }

    
    
}
