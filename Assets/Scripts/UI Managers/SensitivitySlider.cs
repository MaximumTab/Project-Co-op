using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;


    private CameraController cameraController;

    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();

        if (!cameraController)
        {
            Debug.LogError("CameraController not found in scene!");
            return;
        }
        
        sensitivitySlider.onValueChanged.AddListener(cameraController.SetSensitivity);
        sensitivitySlider.minValue = 0.5f;
        sensitivitySlider.maxValue = 3f;
        sensitivitySlider.value = cameraController.GetSensitivity();
    }
}
