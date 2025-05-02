using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider xSlider;
    [SerializeField] private Slider ySlider;

    private CameraController cameraController;

    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();

        if (!cameraController)
        {
            Debug.LogError("CameraController not found in scene!");
            return;
        }

        xSlider.minValue = 0.1f;
        xSlider.maxValue = 3f;
        xSlider.value = cameraController.GetXSensitivity();
        xSlider.onValueChanged.AddListener(cameraController.SetXSensitivity);

        ySlider.minValue = 0.1f;
        ySlider.maxValue = 3f;
        ySlider.value = cameraController.GetYSensitivity();
        ySlider.onValueChanged.AddListener(cameraController.SetYSensitivity);
    }
}
