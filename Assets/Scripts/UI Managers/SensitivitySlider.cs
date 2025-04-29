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

        xSlider.minValue = 0.01f;
        xSlider.maxValue = 2f;
        xSlider.value = cameraController.GetXSensitivity();
        xSlider.onValueChanged.AddListener(cameraController.SetXSensitivity);

        ySlider.minValue = 0.01f;
        ySlider.maxValue = 2f;
        ySlider.value = cameraController.GetYSensitivity();
        ySlider.onValueChanged.AddListener(cameraController.SetYSensitivity);
    }
}
