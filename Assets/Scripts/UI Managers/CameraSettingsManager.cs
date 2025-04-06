using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CameraSettingsManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private static CameraController cameraController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            cameraController = FindAnyObjectByType<CameraController>();
        }

        if (PlayerPrefs.HasKey("CameraXSensitivity") || PlayerPrefs.HasKey("CameraYSensitivity"))   //if these preferences exist
        {
            LoadPlayerPrefs();  //load in the last recorded player pref data on startup if one exists
        }
        else
        { SetCamSensitivity(); } //else, we make it for the first time
    }

    public void SetCamSensitivity()
    {
        if (cameraController != null)
        {
            cameraController.SetXSensitivity(slider.value); //sets the variables
            cameraController.SetYSensitivity(slider.value);
      
            PlayerPrefs.SetFloat("CameraXSensitivity", cameraController.GetYSensitivity()); //sets the preference
            PlayerPrefs.SetFloat("CameraYSensitivity", cameraController.GetXSensitivity());
        }
        else    //i.e. we're in the title scene, no cameraController exists
        {
            PlayerPrefs.SetFloat("CameraXSensitivity", slider.value);   //just sets the preference
            PlayerPrefs.SetFloat("CameraYSensitivity", slider.value);
        }
    }

    void LoadPlayerPrefs()
    {
       slider.value = PlayerPrefs.GetFloat("CameraXSensitivity");   //loads in the preference into the value of the slider                                                         
       SetCamSensitivity(); //then we overwrite it with the new value
    }

}