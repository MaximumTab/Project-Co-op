using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  [SerializeField] private GameObject controlsMenu;

  public void PlayGame()
  {
    SceneManager.LoadSceneAsync(1);
  }
    public void QuitGame()
  {
    Application.Quit();
  }

    public void OpenControls()
    {
        controlsMenu.SetActive(true);
    }

    public void CloseControls()
    {
        controlsMenu.SetActive(false);
    }
}
